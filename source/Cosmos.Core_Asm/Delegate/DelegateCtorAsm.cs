using Cosmos.IL2CPU;
using Cosmos.IL2CPU.X86.IL;
using IL2CPU.Reflection;

using XSharp;
using XSharp.Assembler;

namespace Cosmos.Core_Asm
{
    public class DelegateCtorAsm : AssemblerMethod
    {
        public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
        {
            // method signature: $this, object @object, IntPtr method
            var xAssembler = aAssembler;
            var xMethodInfo = (_MethodInfo)aMethodInfo;
            XS.Comment("Save target ($this) to field");
            XS.Comment("-- ldarg 0");
            Ldarg.DoExecute(xAssembler, xMethodInfo, 0);
            XS.Comment("-- ldarg 1");
            Ldarg.DoExecute(xAssembler, xMethodInfo, 1);
            XS.Comment("-- stfld _target");
            Stfld.DoExecute(xAssembler, xMethodInfo, "System.Object System.Delegate._target", xMethodInfo.MethodInfo.DeclaringType, true, false);
            XS.Comment("Save method pointer to field");
            XS.Comment("-- ldarg 0");
            Ldarg.DoExecute(xAssembler, xMethodInfo, 0);
            XS.Comment("-- ldarg 2");
            Ldarg.DoExecute(xAssembler, xMethodInfo, 2);
            XS.Comment("-- stfld _methodPtr");
            Stfld.DoExecute(xAssembler, xMethodInfo, "System.IntPtr System.Delegate._methodPtr", xMethodInfo.MethodInfo.DeclaringType, true, false);
            XS.Comment("Saving ArgSize to field");
            uint xSize = 0;
            foreach (var xArg in xMethodInfo.MethodInfo.DeclaringType.GetMethod("Invoke").ParameterTypes)
            {
                xSize += ILOp.Align(ILOp.SizeOfType(xArg), 4);
            }

            XS.Comment("-- ldarg 0");
            Ldarg.DoExecute(xAssembler, xMethodInfo, 0);
            XS.Comment("-- push argsize");
            XS.Push(xSize);
            XS.Comment("-- stfld ArgSize");
            Stfld.DoExecute(xAssembler, xMethodInfo, "$$ArgSize$$", xMethodInfo.MethodInfo.DeclaringType, true, false);
        }
    }
}
