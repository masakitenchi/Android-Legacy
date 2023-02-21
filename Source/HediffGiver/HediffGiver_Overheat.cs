// Decompiled with JetBrains decompiler
// Type: Androids.HediffGiver_Overheat
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Verse;

namespace Androids
{
  public class HediffGiver_Overheat : HediffGiver
  {
    public HediffDef contributingHediff;
    public float startToOverheatAt = 0.5f;

    public override void OnIntervalPassed(Pawn pawn, Hediff cause)
    {
      HediffSet hediffSet = pawn.health.hediffSet;
      Hediff firstHediffOfDef = hediffSet.GetFirstHediffOfDef(this.contributingHediff);
      if ((firstHediffOfDef == null ? 0 : ((double) firstHediffOfDef.Severity >= (double) this.startToOverheatAt ? 1 : 0)) != 0)
        HealthUtility.AdjustSeverity(pawn, this.hediff, hediffSet.BleedRateTotal * 0.005f);
      else
        HealthUtility.AdjustSeverity(pawn, this.hediff, -0.0125f);
    }
  }
}
