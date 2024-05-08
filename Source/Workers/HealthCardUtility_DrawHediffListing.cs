// Decompiled with JetBrains decompiler
// Type: Androids.HealthCardUtility_DrawHediffListing
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace Androids
{
    [HarmonyPatch(typeof(HealthCardUtility))]
    [HarmonyPatch("DrawHediffListing")]
    public static class HealthCardUtility_DrawHediffListing
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo labelHelper = AccessTools.Method(typeof(HealthCardUtility_DrawHediffListing), "TransformToLeakingIfDroid");
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == "BleedingRate")
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, labelHelper);
                }
                else
                    yield return instruction;
            }
        }

        public static string TransformToLeakingIfDroid(Pawn pawn) => pawn.IsAndroid() || pawn.def.HasModExtension<MechanicalPawnProperties>() ? "LeakingRate" : "BleedingRate";
    }
}
