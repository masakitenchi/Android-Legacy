// Decompiled with JetBrains decompiler
// Type: Androids.HediffGiver_Machinelike
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

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
