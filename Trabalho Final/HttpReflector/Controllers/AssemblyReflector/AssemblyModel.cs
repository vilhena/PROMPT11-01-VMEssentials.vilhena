using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using HttpReflector.Controllers.Exception;
using HttpReflector.Controllers.Model;

namespace HttpReflector.Controllers
{
    public class AssemblyModel
    {
        private Dictionary<string, ReflectContext> _contexts;
        private Dictionary<string, ReflectContext> Contexts
        {
            get { return _contexts; }
            set { _contexts = value; }
        }

        public AssemblyModel()
        {
            Contexts = new Dictionary<string, ReflectContext>();
        }

        public void AddContext(string context, string path)
        {
            if (!this.Contexts.ContainsKey(context))
            {
                var directoryInfo = new DirectoryInfo(path);
                if (!directoryInfo.Exists)
                    throw new InvalidPathAssemblyModelException(path);
                var newCtx = new ReflectContext()
                                 {
                                     Name = context, 
                                     Folder = path,
                                     Assemblies = new Dictionary<string, ReflectAssembly>()
                                 };

                foreach (FileInfo file in directoryInfo.GetFiles("*.dll"))
                {
                    System.Reflection.Assembly asm = null;
                    try
                    {
                        asm = System.Reflection.Assembly.LoadFrom(file.FullName);
                    }
                    catch (BadImageFormatException)
                    {
                        //Ignores Invalid type of Exception
                    }

                    if (asm != null)
                    {
                        var newassembly = new ReflectAssembly
                                           {
                                               Name = asm.GetName().Name,
                                               FullName = asm.GetName().FullName,
                                               Version = asm.ImageRuntimeVersion,
                                               Namespaces = new Dictionary<string, ReflectNamespace>(),
                                           };

                        var sb = new StringBuilder();
                        asm.GetName().GetPublicKey().ToList().ForEach(s => sb.AppendFormat("{0:x}", s));


                        foreach (var type in asm.GetTypes())
                        {
                            if (type.Namespace != null)
                            {
                                // Adds to assembly
                                if (!newassembly.Namespaces.ContainsKey(type.Namespace))
                                    newassembly.Namespaces.Add(type.Namespace,
                                                               new ReflectNamespace()
                                                                   {
                                                                       Name = type.Namespace,
                                                                       Types = new Dictionary<string, ReflectType>() 
                                                                   });

                                var currNamespace = newassembly.Namespaces[type.Namespace];

                                var newType = new ReflectType()
                                                  {
                                                      Name = type.Name,
                                                      NameSpace = currNamespace,
                                                      Assembly = newassembly,
                                                      Methods = new Dictionary<string, ReflectMethod>(),
                                                      Fields = new Dictionary<string, ReflectField>(),
                                                      Properties = new Dictionary<string, ReflectProperty>()
                                                  };


                                
                                //TODO: Listas com os membros do tipo, agrupadas por tipo de membro (e.g. método, campo, propriedade).


                                // Adds to List
                                if (!currNamespace.Types.ContainsKey(newType.Name))
                                    currNamespace.Types.Add(newType.Name, newType);
                            }
                        }

                        newCtx.Assemblies.Add(newassembly.Name, newassembly);
                    }
                }

                Contexts.Add(context, newCtx);
            }
        }

        public List<ReflectContext> ListContexts()
        {
            return Contexts.Values.ToList();
        }


        public List<ReflectAssembly> ListContextAssemblies(string context)
        {
            return GetContext(context).Assemblies.Values.ToList();
        }


        public ReflectAssembly GetAssembly(string context, string assembly)
        {
            var ctx = GetContext(context);
            if(!ctx.Assemblies.ContainsKey(assembly))
                throw new InvalidAssemblyModelException(assembly);
            return GetContext(context).Assemblies[assembly];
        }

        public ReflectContext GetContext(string context)
        {
            if(!Contexts.ContainsKey(context))
                throw new InvalidContextModelException(context);
            return Contexts[context];
        }
    }


}
