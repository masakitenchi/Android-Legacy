// Decompiled with JetBrains decompiler
// Type: Androids.HediffGiver_Overheat
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

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
      if (firstHediffOfDef != null && (double) firstHediffOfDef.Severity >= (double) this.startToOverheatAt)
        HealthUtility.AdjustSeverity(pawn, this.hediff, hediffSet.BleedRateTotal * 0.005f);
      else
        HealthUtility.AdjustSeverity(pawn, this.hediff, -0.0125f);
    }
  }
}
