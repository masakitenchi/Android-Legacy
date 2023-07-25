// Decompiled with JetBrains decompiler
// Type: Androids.Recipe_RepairKit
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Androids
{
    public class Recipe_RepairKit : RecipeWorker
    {
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            if (pawn.def.HasModExtension<MechanicalPawnProperties>() || pawn.def.HasModExtension<AndroidPawnProperties>())
            {
                if ((double)pawn.health.hediffSet.BleedRateTotal > 0.0 || (double)pawn.health.summaryHealth.SummaryHealthPercent < 1.0 || pawn.health.hediffSet.GetMissingPartsCommonAncestors().Count > 0 || pawn.health.hediffSet.hediffs.Any<Hediff>((Predicate<Hediff>)(hediff => hediff.def.HasModExtension<MechanicalHediffProperties>() && hediff.CurStage.becomeVisible)))
                    yield return (BodyPartRecord)null;
            }
            else if ((double)pawn.health.hediffSet.BleedRateTotal > 0.0)
                yield return (BodyPartRecord)null;
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
            if (!pawn.def.HasModExtension<MechanicalPawnProperties>() && !pawn.def.HasModExtension<AndroidPawnProperties>())
                return;
            List<Hediff> hediffList = new List<Hediff>();
            foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
            {
                if (hediff is Hediff_MissingPart || hediff is Hediff_Injury || hediff.def.HasModExtension<MechanicalHediffProperties>())
                    hediffList.Add(hediff);
            }
            foreach (Hediff hediff in hediffList)
                pawn.health.RemoveHediff(hediff);
        }
    }
}
