// Decompiled with JetBrains decompiler
// Type: Androids.Recipe_RepairKit
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using System.Linq;

namespace Androids
{
  public class Recipe_RepairKit : RecipeWorker
  {
    public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
    {
      if (pawn.def.HasModExtension<MechanicalPawnProperties>())
      {
        if ((double) pawn.health.hediffSet.BleedRateTotal > 0.0 || (double) pawn.health.summaryHealth.SummaryHealthPercent < 1.0 || pawn.health.hediffSet.GetMissingPartsCommonAncestors().Count > 0 || pawn.health.hediffSet.hediffs.Any<Hediff>((Predicate<Hediff>) (hediff => hediff.def.HasModExtension<MechanicalHediffProperties>() && hediff.CurStage.becomeVisible)))
          yield return (BodyPartRecord) null;
      }
      else if ((double) pawn.health.hediffSet.BleedRateTotal > 0.0)
        yield return (BodyPartRecord) null;
    }

    public override void ApplyOnPawn(
      Pawn pawn,
      BodyPartRecord part,
      Pawn billDoer,
      List<Thing> ingredients,
      Bill bill)
    {
      Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjCoolantLoss);
      if (firstHediffOfDef != null)
        pawn.health.RemoveHediff(firstHediffOfDef);
      List<Hediff> hediffList = pawn.health.hediffSet.hediffs.Where(hediff => hediff is Hediff_MissingPart or Hediff_Injury || hediff.def.HasModExtension<MechanicalHediffProperties>()).ToList();
      foreach (Hediff hediff in hediffList)
        pawn.health.RemoveHediff(hediff);
    }
  }
}
