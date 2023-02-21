// Decompiled with JetBrains decompiler
// Type: Androids.HediffGiver_Machinelike
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System;
using Verse;

namespace Androids
{
  public class HediffGiver_Machinelike : HediffGiver
  {
    public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
    {
      if (hediff.def == RimWorld.HediffDefOf.BloodLoss)
      {
        HealthUtility.AdjustSeverity(pawn, HediffDefOf.ChjCoolantLoss, hediff.Severity);
        hediff.Severity = 0.0f;
        return true;
      }
      if (!(hediff is Hediff_Injury) || !(hediff is HediffWithComps hediffWithComps))
        return false;
      hediffWithComps.comps.RemoveAll((Predicate<HediffComp>) (hediffComp => hediffComp is HediffComp_Infecter));
      return true;
    }
  }
}
