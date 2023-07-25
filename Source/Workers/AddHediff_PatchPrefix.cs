// Decompiled with JetBrains decompiler
// Type: Androids.AddHediff_PatchPrefix
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using HarmonyLib;
using Verse;

namespace Androids
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "AddHediff")]
    [HarmonyPatch(new System.Type[] { typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult) })]
    public class AddHediff_PatchPrefix
    {
        [HarmonyPrefix]
        public static bool Listener(ref Pawn ___pawn, ref Hediff hediff, BodyPartRecord part) => !___pawn.def.HasModExtension<MechanicalPawnProperties>() && !___pawn.IsAndroid() || !hediff.def.makesSickThought;
    }
}
