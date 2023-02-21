// Decompiled with JetBrains decompiler
// Type: Androids.AndroidValueStatPart
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Androids
{
  public class AndroidValueStatPart : StatPart
  {
    public override string ExplanationPart(StatRequest req)
    {
      if (!(req.Thing is Pawn thing))
        return (string) null;
      IEnumerable<Hediff> relevantHediffs = this.GetRelevantHediffs(thing);
      if (relevantHediffs == null)
        return (string) null;
      List<Hediff> hediffList = new List<Hediff>(relevantHediffs);
      if (hediffList.Count == 0)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine((string) "AndroidMarketValueStatPartLabel".Translate());
      foreach (Hediff hediff in hediffList)
        stringBuilder.AppendLine("    " + hediff.LabelCap + ": +" + string.Format(this.parentStat.formatString, (object) (float) Math.Ceiling((double) PriceUtility.PawnQualityPriceFactor(thing) * (double) this.CalculateMarketValueFromHediff(hediff, thing.RaceProps.baseBodySize))));
      return stringBuilder.ToString();
    }

    public override void TransformValue(StatRequest req, ref float val)
    {
      if (!(req.Thing is Pawn thing))
        return;
      IEnumerable<Hediff> relevantHediffs = this.GetRelevantHediffs(thing);
      if (relevantHediffs == null)
        return;
      foreach (Hediff hediff in new List<Hediff>(relevantHediffs))
        val += (float) Math.Ceiling((double) PriceUtility.PawnQualityPriceFactor(thing) * (double) this.CalculateMarketValueFromHediff(hediff, thing.RaceProps.baseBodySize));
    }

    private float CalculateMarketValueFromHediff(Hediff hediff, float bodySize = 1f)
    {
      if (hediff == null)
      {
        Log.Error("Hediff is 'null'. This should not happen!");
        return 0.0f;
      }
      AndroidUpgradeHediffProperties modExtension = hediff.def.GetModExtension<AndroidUpgradeHediffProperties>();
      if (modExtension == null)
        return 0.0f;
      float a = 0.0f;
      if (modExtension.def == null)
      {
        Log.Error("Hediff '" + hediff.LabelCap + "' got 'null' properties despite having the 'AndroidUpgradeHediffProperties' DefModExtension!");
        return 0.0f;
      }
      foreach (ThingOrderRequest cost in modExtension.def.costList)
      {
        if (!cost.nutrition)
        {
          if (modExtension.def.costsNotAffectedByBodySize.Contains(cost.thingDef))
            a += cost.thingDef.BaseMarketValue * cost.amount;
          else
            a += cost.thingDef.BaseMarketValue * cost.amount * bodySize;
        }
      }
      return (float) Math.Ceiling((double) a);
    }

    private IEnumerable<Hediff> GetRelevantHediffs(Pawn pawn) => pawn.health.hediffSet.hediffs.Where<Hediff>((Func<Hediff, bool>) (hediff =>
    {
      AndroidUpgradeHediffProperties modExtension = hediff.def.GetModExtension<AndroidUpgradeHediffProperties>();
      if (modExtension == null || modExtension.def.costList.Count <= 0)
        return false;
      return modExtension.def.costList.Count != 1 || !modExtension.def.costList[0].nutrition;
    }));
  }
}
