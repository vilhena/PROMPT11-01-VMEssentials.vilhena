﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using HttpReflector.Controllers.Exception;
using HttpReflector.Controllers.Model;
using HttpReflector.Utils;

namespace HttpReflector.Controllers
{
    public static class AssemblyModel
    {
        private static Dictionary<string, ReflectContext> Contexts { get; set; }

        static AssemblyModel()
        {
            Contexts = new Dictionary<string, ReflectContext>();
        }

        private static string PublicKeyToString(IEnumerable<byte> key)
        {
            var sb = new StringBuilder();
            key.ToList().ForEach(s => sb.AppendFormat("{0:x}", s));
            return sb.ToString();
        }

        public static void AddContext(string context, string path)
        {
            if (AssemblyModel.Contexts.ContainsKey(context)) 
                return;

            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
                throw new InvalidPathAssemblyModelException(path);

            var newCtx = new ReflectContext()
                             {
                                 Name = context,
                                 Folder = path,
                                 Assemblies = new Dictionary<string, ReflectAssembly>(),
                                 Namespaces = new Tree<string, ReflectNamespace>()
                             };

            Contexts.Add(context, newCtx);

            foreach (var file in directoryInfo.GetFiles("*.dll"))
            {
                Assembly asm = null;

                try { asm = Assembly.LoadFrom(file.FullName); }
                catch
                {
                    continue;
                } //Ignores Invalid type of Exception

                
                var newassembly = new ReflectAssembly
                                      {
                                          Context = newCtx,
                                          Name = asm.GetName().Name,
                                          FullName = asm.GetName().FullName,
                                          Version = asm.ImageRuntimeVersion,
                                          //Namespaces = new Tree<string, ReflectNamespace>(),
                                          PublicKey = PublicKeyToString(asm.GetName().GetPublicKey())
                                      };

                newCtx.Assemblies.Add(newassembly.Name, newassembly);


                try
                {
                    foreach (var type in asm.GetTypes()) { }
                }
                catch
                {
                    //Inheritance security rules violated by type: 'System.Web.DynamicData.ControlFilterExpression'. Derived types must either match the security accessibility of the base type or be less accessible.":"System.Web.DynamicData.ControlFilterExpression"}
                    continue;
                }

                foreach (var type in asm.GetTypes())
                {
                    //adds to context namespace list
                    if (type.Namespace != null && type.IsPublic)
                    {
                        ReflectNamespace newNamesspace;

                        if (!newCtx.Namespaces.ContainsKey(type.Namespace.Split('.')))
                        {
                            newNamesspace =
                                new ReflectNamespace()
                                    {
                                        Name = type.Namespace,
                                        Assembly = newassembly,
                                        Types = new Dictionary<string, Lazy<ReflectType>>()
                                    };

                            if (!newCtx.Namespaces.ContainsKey(newNamesspace.Name.Split('.')))
                            {
                                newCtx.Namespaces.Add(newNamesspace.Name.Split('.'), newNamesspace);
                                //fills null Objects with Namespaces must be on reverse order
                                var namespaceStack = new Stack<string>(newNamesspace.Name.Split('.').Reverse());
                                var namespaceList = new StringBuilder();
                                //namespaceList.Append(namespaceStack.Pop());

                                foreach (var name in namespaceStack)
                                {
                                    if (namespaceList.Length == 0)
                                        namespaceList.Append(name);
                                    else
                                    {
                                        namespaceList.Append(".").Append(name);
                                    }

                                    var value = newCtx.Namespaces.GetValue(namespaceList.ToString().Split('.'));
                                    if (value == null)
                                    {
                                        newCtx.Namespaces.SetValue(namespaceList.ToString().Split('.'),
                                                                   new ReflectNamespace()
                                                                       {
                                                                           Assembly = newassembly,
                                                                           Name = namespaceList.ToString(),
                                                                           Types =
                                                                               new Dictionary<string, Lazy<ReflectType>>()
                                                                       });
                                    }
                                    
                                    
                                }
                            }
                        }
                        else
                        {
                            newNamesspace = newCtx.Namespaces.GetValue(type.Namespace.Split('.'));
                        }
                        // Adds to List
                        if (!newNamesspace.Types.ContainsKey(type.Name))
                        {
                            var reflectionType = type;
                            newNamesspace.Types.Add(type.Name,
                                                    new Lazy<ReflectType>(() => AddNewReflectType(reflectionType,
                                                                                                  newassembly,
                                                                                                  newNamesspace)));
                        }
                    }

                }
            }
        }

        private static ReflectType AddNewSimpleReflectType(Type type, ReflectAssembly newassembly, ReflectNamespace currNamespace)
        {
            var newType = new ReflectType()
            {
                Name = type.Name,
                FullName = type.FullName,
                Namespace = currNamespace,
                Assembly = newassembly,
                Contructors = new List<ReflectMethod>(),
                Methods = new List<ReflectMethod>(),
                Fields = new List<ReflectField>(),
                Properties = new List<ReflectProperty>(),
                Events = new List<ReflectEvent>(),
                CSharpName = ToGenericTypeString(type)
            };

            return newType;
        }

        private static ReflectType AddNewReflectType(Type type, ReflectAssembly newassembly, ReflectNamespace currNamespace)
        {
    
            var newType = new ReflectType()
                              {
                                  Name = type.Name,
                                  FullName = type.FullName,
                                  Namespace = currNamespace,
                                  Assembly = newassembly,
                                  Contructors = new List<ReflectMethod>(),
                                  Methods = new List<ReflectMethod>(),
                                  Fields = new List<ReflectField>(),
                                  Properties = new List<ReflectProperty>(),
                                  Events = new List<ReflectEvent>(),
                                  CSharpName = ToGenericTypeString(type)
                              };

            FillConstructors(type, newType, newassembly,currNamespace);
            FillMethods(type, newType, newassembly, currNamespace);
            FillFields(type, newType, newassembly, currNamespace);
            FillProperties(type, newType, newassembly, currNamespace);
            FillEvents(type, newType);

            return newType;
        }

        private static void FillEvents(Type type, ReflectType newType)
        {
            foreach (var eventInfo in type.GetEvents())
            {
                // Finds or crates a new Type on this assembly
                Tuple<ReflectNamespace,ReflectAssembly> asmns = FindNamespaceAndAssembly(eventInfo.EventHandlerType);


                var newEvent = new ReflectEvent()
                                   {
                                       Name = eventInfo.Name,
                                       Type = 
                                           AddNewSimpleReflectType(eventInfo.EventHandlerType, asmns.Item2, asmns.Item1)
                                   };

                newType.Events.Add(newEvent);
            }
        }

        private static Tuple<ReflectNamespace, ReflectAssembly> FindNamespaceAndAssembly(Type type)
        {
            var asm = FindAssembly(type.Assembly.GetName().Name);
            if(asm == null)
                asm = new ReflectAssembly
                          {
                              Name = type.Assembly.GetName().Name,
                              FullName = type.Assembly.GetName().FullName,
                              Version = type.Assembly.ImageRuntimeVersion,
                              //Namespaces = new Tree<string, ReflectNamespace>(),
                              PublicKey =
                                  PublicKeyToString(
                                      type.Assembly.GetName().GetPublicKey()),
                              Context = new ReflectContext()
                                            {
                                                Assemblies = new Dictionary<string, ReflectAssembly>(),
                                                Folder = "NotFound",
                                                Name = "NotFound",
                                                Namespaces = new Tree<string, ReflectNamespace>()
                                            }
                          };

            var ns = FindNamespace(type.Namespace);
            if (ns == null)
                ns = new ReflectNamespace()
                         {
                             Assembly = asm,
                             Name = type.Namespace,
                             Types = new Dictionary<string, Lazy<ReflectType>>()
                         };
            return new Tuple<ReflectNamespace, ReflectAssembly>(ns, asm);
        }


        private static void FillProperties(Type type, ReflectType newType, ReflectAssembly newassembly, ReflectNamespace currNamespace)
        {
            foreach (var propertyInfo in type.GetProperties())
            {
                // Finds or crates a new Type on this assembly
                Tuple<ReflectNamespace, ReflectAssembly> asmns = FindNamespaceAndAssembly(propertyInfo.PropertyType);

                newType.Properties.Add(new ReflectProperty()
                                           {
                                               Name = propertyInfo.Name,
                                               Type =
                                                   AddNewSimpleReflectType(propertyInfo.PropertyType, asmns.Item2, asmns.Item1)
                                           });
            }
        }

        private static void FillFields(Type type, ReflectType newType, ReflectAssembly newassembly, ReflectNamespace currNamespace)
        {
            foreach (var fieldInfo in type.GetFields())
            {
                // Finds or crates a new Type on this assembly
                Tuple<ReflectNamespace, ReflectAssembly> asmns = FindNamespaceAndAssembly(fieldInfo.FieldType);
                

                newType.Fields.Add(new ReflectField()
                                       {
                                           Name = fieldInfo.Name,
                                           Type =
                                                   AddNewSimpleReflectType(fieldInfo.FieldType, asmns.Item2, asmns.Item1)
                                       });

            }
        }

        private static void FillMethods(Type type, ReflectType newType, ReflectAssembly newassembly, ReflectNamespace currNamespace)
        {
            foreach (var methodInfo in type.GetMethods())
            {
                // Finds or crates a new Type on this assembly
                Tuple<ReflectNamespace, ReflectAssembly> asmns = FindNamespaceAndAssembly(methodInfo.ReturnType);

                var newMethod =
                    new ReflectMethod()
                        {
                            Name = methodInfo.Name,
                            Parameters = new List<ReflectParameter>(),
                            Return =
                                AddNewSimpleReflectType(methodInfo.ReturnType, asmns.Item2, asmns.Item1)
                        };
                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    Tuple<ReflectNamespace, ReflectAssembly> asmns2 = FindNamespaceAndAssembly(parameterInfo.ParameterType);

                    newMethod.Parameters.Add(new ReflectParameter()
                                                 {
                                                     Name = parameterInfo.Name,
                                                     Type = AddNewSimpleReflectType(parameterInfo.ParameterType, asmns2.Item2, asmns2.Item1)
                                                 });
                }

                newType.Methods.Add(newMethod);
            }
        }

        private static void FillConstructors(Type type, ReflectType newType, ReflectAssembly newassembly, ReflectNamespace currNamespace)
        {
            foreach (var constructorInfo in type.GetConstructors())
            {
                var newMethod =
                    new ReflectMethod()
                        {
                            Name = constructorInfo.Name,
                            Parameters = new List<ReflectParameter>(),
                            Return = null
                        };

                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    Tuple<ReflectNamespace, ReflectAssembly> asmns2 = FindNamespaceAndAssembly(parameterInfo.ParameterType);

                    newMethod.Parameters.Add(new ReflectParameter()
                    {
                        Name = parameterInfo.Name,
                        Type = AddNewSimpleReflectType(parameterInfo.ParameterType, asmns2.Item2, asmns2.Item1)
                    });
                }

                newType.Contructors.Add(newMethod);
            }
        }


        public static List<ReflectContext> ListContexts()
        {
            return Contexts.Values.ToList();
        }


        public static List<ReflectAssembly> ListContextAssemblies(string context)
        {
            return GetContext(context).Assemblies.Values.ToList();
        }

        public static ReflectAssembly FindAssembly(string assemblyName)
        {
            foreach (var ctxs in Contexts.Values)
            {
                if (ctxs.Assemblies.ContainsKey(assemblyName))
                {
                    return ctxs.Assemblies[assemblyName];
                }
            }
            return null;
        }

        public static ReflectNamespace FindNamespace(string nameSpace)
        {
            if (nameSpace == null)
                return null;
            foreach (var ctxs in Contexts.Values)
            {
                if (ctxs.Namespaces.ContainsKey(nameSpace.Split('.')))
                {
                    return ctxs.Namespaces.GetValue(nameSpace.Split('.'));
                }
            }
            return null;
        }

        public static ReflectAssembly GetAssembly(string context, string assembly)
        {
            var ctx = GetContext(context);
            if(!ctx.Assemblies.ContainsKey(assembly))
                throw new InvalidAssemblyModelException(context, assembly);
            return GetContext(context).Assemblies[assembly];
        }

        public static ReflectContext GetContext(string context)
        {
            if(!Contexts.ContainsKey(context))
                throw new InvalidContextModelException(context);
            return Contexts[context];
        }

        public static string ToGenericTypeString(Type type)
        {
            if (!type.IsGenericType) 
                return type.Name;

            try //there are some types that IsGenericType is true but GetGenericTypeDefinition throws
            {
                string genericTypeName = type.GetGenericTypeDefinition().Name;


                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
                    

                string genericArgs = string.Join(",",
                                                 type.GetGenericArguments().Select(ta => ToGenericTypeString(ta)).
                                                     ToArray());
                return genericTypeName + "<" + genericArgs + ">";
            }
            catch
            {
                return type.Name;
            }
        }

        public static List<ReflectNamespace> ListSubNamespaces(string context, string namespacePrefix)
        {
            var ctx = GetContext(context);
            if (!ctx.Namespaces.ContainsKey(namespacePrefix.Split('.')))
                throw new InvalidNamespaceModelException(context, namespacePrefix);
            return GetContext(context).Namespaces.GetAllChildrenValues(namespacePrefix.Split('.')).ToList();
        }

        public static List<ReflectNamespace> ListNamespaces(string context)
        {
            //var namespaces = new List<ReflectNamespace>();
            //foreach (var assembly in GetContext(context).Assemblies.Values)
            //{
            //    namespaces.AddRange(assembly.Namespaces.GetAllChildrenValues(new List<string>()));
            //}

            return GetContext(context).Namespaces.GetAllChildrenValues(new List<string>()).ToList();
        }

        public static ReflectNamespace GetNamespace(string context, string namespacePrefix)
        {
            var ctx = GetContext(context);
            if (!ctx.Namespaces.ContainsKey(namespacePrefix.Split('.')))
                throw new InvalidNamespaceModelException(context, namespacePrefix);
            return GetContext(context).Namespaces.GetValue(namespacePrefix.Split('.'));
        }

        public static ReflectType GetCtsType(string context, string namespacePrefix, string shortName)
        {
            var reflectNamespace = GetNamespace(context, namespacePrefix);
            if (!reflectNamespace.Types.ContainsKey(shortName))
                throw new InvalidCtsTypeModelException(context, namespacePrefix, shortName);
            return reflectNamespace.Types[shortName].Value;
        }

    }


}
