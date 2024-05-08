// Decompiled with JetBrains decompiler
// Type: Androids.HealthCardUtility_DrawHediffRow
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace Androids
{
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(HealthCardUtility))]
    [HarmonyPatch("DrawHediffRow")]
    public static class HealthCardUtility_DrawHediffRow
    {
        private static readonly Texture2D leakingIcon = ContentFinder<Texture2D>.Get("UI/Leaking");

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo bleedingIconField = AccessTools.Field(typeof(HealthCardUtility), "BleedingIcon");
            AccessTools.Property(typeof(Hediff), "LabelColor").GetGetMethod();
            MethodInfo iconHelper = AccessTools.Method(typeof(HealthCardUtility_DrawHediffRow), "TransformIconColorBlueIfAndroid");
            AccessTools.Method(typeof(HealthCardUtility_DrawHediffRow), "TransformLabelColorRedToBlueIfAndroid");
            foreach (CodeInstruction code in instructions)
            {
                yield return code;
                if (code.opcode == OpCodes.Ldsfld && code.operand == (object)bleedingIconField)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, iconHelper);
                }
            }
        }

        public static Texture2D TransformIconColorBlueIfAndroid(Texture2D original, Pawn pawn) => pawn.IsAndroid() || pawn.def.HasModExtension<MechanicalPawnProperties>() ? leakingIcon : original;

        public static Color TransformLabelColorRedToBlueIfAndroid(Color original, Pawn pawn) => pawn.IsAndroid() || pawn.def.HasModExtension<MechanicalPawnProperties>() ? Color.blue : original;
    }
}
