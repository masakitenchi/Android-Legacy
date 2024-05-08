// Decompiled with JetBrains decompiler
// Type: Androids.JobGiver_RefillFuelEnergySource
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using Verse;
using Verse.AI;

namespace Androids
{
    public class JobGiver_RefillFuelEnergySource : ThinkNode_JobGiver
    {
        public JobDef refillJob;

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_RefillFuelEnergySource fuelEnergySource = (JobGiver_RefillFuelEnergySource)base.DeepCopy(resolve);
            fuelEnergySource.refillJob = this.refillJob;
            return fuelEnergySource;
        }

        public override float GetPriority(Pawn pawn) => FuelUtility.FueledEnergySourceNeedRefilling(pawn) != null ? 10f : 0.0f;

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.Downed)
                return null;
            if (pawn.InBed())
                return null;
            Thing thing = FuelUtility.FueledEnergySourceNeedRefilling(pawn);
            if (thing == null)
                return null;
            EnergySource_Fueled comp = thing.TryGetComp<EnergySource_Fueled>();
            if (!comp.autoRefuel)
                return null;
            Thing suitableFuelForPawn = FuelUtility.FindSuitableFuelForPawn(pawn, comp);
            if (suitableFuelForPawn == null)
                return null;
            return new Job(this.refillJob, (LocalTargetInfo)thing, (LocalTargetInfo)suitableFuelForPawn)
            {
                count = comp.CalculateFuelNeededToRefill(suitableFuelForPawn)
            };
        }
    }
}
