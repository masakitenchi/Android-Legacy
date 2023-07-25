// Decompiled with JetBrains decompiler
// Type: Androids.HediffGiver_Constant
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Collections.Generic;
using Verse;

namespace Androids
{
    public class HediffGiver_Constant : HediffGiver
    {
        private static List<Hediff> addedHediffs = new List<Hediff>();

        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            if (!pawn.IsHashIntervalTick(120) || pawn.health.hediffSet.HasHediff(this.hediff))
                return;
            Hediff hediff = HediffMaker.MakeHediff(this.hediff, pawn);
            HediffGiver_Constant.addedHediffs.Add(hediff);
            this.TryApply(pawn, HediffGiver_Constant.addedHediffs);
        }
    }
}
