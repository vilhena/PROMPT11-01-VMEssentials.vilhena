using System;
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
                                 Namespaces = new Dictionary<string, ReflectNamespace>()
                             };

            Contexts.Add(context, newCtx);

            foreach (var file in directoryInfo.GetFiles("*.dll"))
            {
                Assembly asm = null;

                try { asm = Assembly.LoadFrom(file.FullName); }
                catch (BadImageFormatException)
                {
                    continue;
                } //Ignores Invalid type of Exception

                
                var newassembly = new ReflectAssembly
                                      {
                                          Context = newCtx,
                                          Name = asm.GetName().Name,
                                          FullName = asm.GetName().FullName,
                                          Version = asm.ImageRuntimeVersion,
                                          Namespaces = new Dictionary<string, ReflectNamespace>(),
                                          PublicKey = PublicKeyToString(asm.GetName().GetPublicKey())
                                      };

                newCtx.Assemblies.Add(newassembly.Name, newassembly);


                foreach (var type in asm.GetTypes())
                {
                    //adds to context namespace list
                    if (type.Namespace != null)
                    {
                        ReflectNamespace newNamesspace;

                        if (!newCtx.Namespaces.ContainsKey(type.Namespace) ||
                            !newassembly.Namespaces.ContainsKey(type.Namespace))
                        {
                            newNamesspace =
                                new ReflectNamespace()
                                    {
                                        Name = type.Namespace,
                                        Assembly = newassembly,
                                        Types = new Dictionary<string, Lazy<ReflectType>>()
                                    };

                            // Adds to assembly
                            if (!newassembly.Namespaces.ContainsKey(type.Namespace))
                                newassembly.Namespaces.Add(newNamesspace.Name, newNamesspace);

                            if (!newCtx.Namespaces.ContainsKey(newNamesspace.Name))
                                newCtx.Namespaces.Add(newNamesspace.Name, newNamesspace);
                        }
                        else
                        {
                            newNamesspace = newCtx.Namespaces[type.Namespace];
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
                NameSpace = currNamespace,
                Assembly = newassembly,
                Contructors = new List<ReflectMethod>(),
                Methods = new List<ReflectMethod>(),
                Fields = new List<ReflectField>(),
                Properties = new List<ReflectProperty>(),
                Events = new List<ReflectEvent>()
            };

            return newType;
        }

        private static ReflectType AddNewReflectType(Type type, ReflectAssembly newassembly, ReflectNamespace currNamespace)
        {
    
            var newType = new ReflectType()
                              {
                                  Name = type.Name,
                                  FullName = type.FullName,
                                  NameSpace = currNamespace,
                                  Assembly = newassembly,
                                  Contructors = new List<ReflectMethod>(),
                                  Methods = new List<ReflectMethod>(),
                                  Fields = new List<ReflectField>(),
                                  Properties = new List<ReflectProperty>(),
                                  Events = new List<ReflectEvent>()
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
                              Namespaces = new Dictionary<string, ReflectNamespace>(),
                              PublicKey =
                                  PublicKeyToString(
                                      type.Assembly.GetName().GetPublicKey())
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
            foreach (var ctxs in Contexts.Values)
            {
                if (ctxs.Namespaces.ContainsKey(nameSpace))
                {
                    return ctxs.Namespaces[nameSpace];
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

        public static List<ReflectNamespace> ListNamespaces(string context)
        {
            return GetContext(context).Namespaces.Values.ToList();
        }

        public static ReflectNamespace GetNamespace(string context, string namespacePrefix)
        {
            var ctx = GetContext(context);
            if (!ctx.Namespaces.ContainsKey(namespacePrefix))
                throw new InvalidNamespaceModelException(context, namespacePrefix);
            return GetContext(context).Namespaces[namespacePrefix];
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
