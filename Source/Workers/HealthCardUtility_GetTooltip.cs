// Decompiled with JetBrains decompiler
// Type: Androids.HealthCardUtility_GetTooltip
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    [HarmonyPatch(typeof(HealthCardUtility))]
    [HarmonyPatch("GetTooltip")]
    public static class HealthCardUtility_GetTooltip
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo tipStringExtraGetter = AccessTools.Property(typeof(Hediff), "TipStringExtra").GetGetMethod();
            MethodInfo labelHelper = AccessTools.Method(typeof(HealthCardUtility_GetTooltip), "TransformBleedingToLeakingIfAndroid");
            foreach (CodeInstruction code in instructions)
            {
                yield return code;
                if (code.opcode == OpCodes.Callvirt && code.operand == (object)tipStringExtraGetter)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, labelHelper);
                }
            }
        }

        public static string TransformBleedingToLeakingIfAndroid(string original, Pawn pawn) => pawn.IsAndroid() || pawn.def.HasModExtension<MechanicalPawnProperties>() ? original.Replace((string)"BleedingRate".Translate(), (string)"LeakingRate".Translate()) : original;
    }
}
