// Decompiled with JetBrains decompiler
// Type: Androids.HediffGiver_MachineWearAndTear
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Androids.Integration;
using System;
using System.Linq;
using Verse;

namespace Androids
{
  public class HediffGiver_MachineWearAndTear : HediffGiver
  {
    public float potencyToIncreasePerDay = 1f;
    public float chanceToContract = 0.5f;

    public float PotencyPerTick => this.potencyToIncreasePerDay / 60000f;

    public int CheckInterval => AndroidsModSettings.Instance.droidWearDownQuadrum ? 900000 : 60000;

    public override void OnIntervalPassed(Pawn pawn, Hediff cause)
    {
      if (!AndroidsModSettings.Instance.droidWearDown || cause != null || this.partsToAffect == null)
        return;
      foreach (BodyPartDef bodyPartDef in this.partsToAffect)
      {
        BodyPartDef def = bodyPartDef;
        BodyPartRecord bodyPart = pawn.RaceProps.body.AllParts.FirstOrDefault<BodyPartRecord>((Func<BodyPartRecord, bool>) (part => part.def == def));
        if (bodyPart != null)
        {
          Hediff hediff1 = pawn.health.hediffSet.hediffs.FirstOrDefault<Hediff>((Func<Hediff, bool>) (partHediff => partHediff.Part == bodyPart && partHediff.def == this.hediff));
          if (hediff1 == null)
          {
            if (pawn.IsHashIntervalTick(this.CheckInterval) && Rand.Chance(this.chanceToContract))
            {
              Hediff hediff2 = HediffMaker.MakeHediff(this.hediff, pawn, bodyPart);
              pawn.health.AddHediff(hediff2);
            }
          }
          else
            hediff1.Severity += this.PotencyPerTick;
        }
      }
    }
  }
}
