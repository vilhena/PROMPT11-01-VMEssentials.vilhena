using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace JITRules
{
    class JITBuilder
    {
        public static void patch(string dll, string method)
        {
            //AssemblyDefinition targetasmdef =
            //     AssemblyFactory.GetAssembly("ReflexilDemo.exe");
            //MethodDefinition computedefinition =
            //    GetMethodDefinition(targetasmdef, "ReflexilDemo.DemoForm",
            //    "ComputeAndDisplay", 2);

            //CilWorker worker = computedefinition.Body.CilWorker;
            //Instruction insld =
            //    worker.Create(OpCodes.Ldstr, "The result is!");
            //worker.InsertBefore(
            //    computedefinition.Body.Instructions[11], insld);

            //Instruction insshow = computedefinition.Body.Instructions[12];
            //MethodReference oldshowref = insshow.Operand as MethodReference;

            //AssemblyNameReference scope =
            //    oldshowref.DeclaringType.Scope as AssemblyNameReference;
            //DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();
            //AssemblyDefinition sourceasmdef = resolver.Resolve(scope);
            //MethodDefinition newshowref = GetMethodDefinition(sourceasmdef,
            //    "System.Windows.Forms.MessageBox", "Show", 2);

            //insshow.Operand =
            //    computedefinition.DeclaringType.Module.Import(newshowref);
            //AssemblyFactory.SaveAssembly(targetasmdef,
            //    "Reflexil.Manualy.Patched.exe");

            AppDomain currentDomain = Thread.GetDomain();

            AssemblyName myAsmName = Assembly.;
            myAsmName.Name = "MyAssembly";




 
           
        }
    }
}
