// Decompiled with JetBrains decompiler
// Type: Androids.HediffGiver_MachineWearAndTear
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Androids.Integration;

namespace Androids
{
    public class HediffGiver_MachineWearAndTear : HediffGiver
    {
        public float potencyToIncreasePerDay = 1f;
        public float chanceToContract = 0.5f;

        public float SeverityIncrement => this.potencyToIncreasePerDay * CheckIntervalMultiplier;

        // One quadrum = 15 days = 900k ticks
        // One day = 60000 ticks

        public int CheckInterval => AndroidsModSettings.Instance.droidWearDownQuadrum ? GenDate.TicksPerQuadrum : GenDate.TicksPerDay;

        public int CheckIntervalMultiplier => AndroidsModSettings.Instance.droidWearDownQuadrum ? 15 : 1;

        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            if (!AndroidsModSettings.Instance.droidWearDown || cause != null || this.partsToAffect == null || !pawn.IsHashIntervalTick(CheckInterval))
                return;
            foreach (BodyPartDef partDef in this.partsToAffect)
            {
                //Log.Message($"{partDef.defName} to affect");
                BodyPartRecord part = pawn.health.hediffSet.GetNotMissingParts().Where(x => x.def == partDef).RandomElementWithFallback();
                if (part != null)
                {
                    //Log.Message($"Affecting {part.def.defName}");
                    Hediff hediff1 = pawn.health.hediffSet.hediffs.FirstOrDefault(partHediff => partHediff.Part == part && partHediff.def == this.hediff);
                    if (Rand.Chance(this.chanceToContract))
                    {
                        if (hediff1 == null)
                        {
                            Hediff hediff2 = HediffMaker.MakeHediff(this.hediff, pawn, part);
                            pawn.health.AddHediff(hediff2);
                        }
                        else
                        { hediff1.Severity += this.SeverityIncrement; }
                    }
                }
            }
        }
    }
}
