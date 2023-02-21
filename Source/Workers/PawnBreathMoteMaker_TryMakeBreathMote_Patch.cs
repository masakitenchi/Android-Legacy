// Decompiled with JetBrains decompiler
// Type: Androids.PawnBreathMoteMaker_TryMakeBreathMote_Patch
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using HarmonyLib;
using RimWorld;
using Verse;

namespace Androids
{
  [HarmonyPatch(typeof (PawnBreathMoteMaker), "TryMakeBreathMote")]
  public class PawnBreathMoteMaker_TryMakeBreathMote_Patch
  {
    [HarmonyPrefix]
    public static bool Listener(Pawn ___pawn) => !___pawn.def.HasModExtension<MechanicalPawnProperties>();
  }
}
