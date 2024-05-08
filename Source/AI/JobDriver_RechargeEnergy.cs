// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_RechargeEnergy
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Androids
{
    public class JobDriver_RechargeEnergy : JobDriver
    {
        private const TargetIndex PowerDestIndex = TargetIndex.A;
        private const TargetIndex AlternateDestIndex = TargetIndex.B;
        public Need_Energy energyNeed;
        public int ticksSpentCharging;

        public Building powerBuilding => this.TargetA.Thing as Building;

        public int MaxTicksSpentCharging => this.energyNeed.EnergyTracker == null ? 300 : this.energyNeed.EnergyTracker.EnergyProperties.ticksSpentCharging;

        public float PowerNetEnergyDrainedPerTick => this.energyNeed.EnergyTracker == null ? 1.32f : this.energyNeed.EnergyTracker.EnergyProperties.powerNetDrainRate;

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            this.energyNeed = this.GetActor().needs.TryGetNeed<Need_Energy>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.ticksSpentCharging, "ticksSpentCharging");
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
            AddFailCondition(() => energyNeed == null);
            yield return Toils_Reserve.Reserve(TargetIndex.A);
            if (!TargetB.IsValid)
            {
                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
            }
            else
            {
                yield return Toils_Reserve.Reserve(TargetIndex.B);
                yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
            }
            Toil toil = new Toil();
            toil.tickAction = delegate
            {
                CompPowerBattery compPowerBattery = powerBuilding.PowerComp?.PowerNet?.batteryComps?.FirstOrDefault((CompPowerBattery battery) => battery.StoredEnergy > PowerNetEnergyDrainedPerTick);
                if (compPowerBattery != null)
                {
                    compPowerBattery.DrawPower(PowerNetEnergyDrainedPerTick);
                    energyNeed.CurLevel += energyNeed.MaxLevel / MaxTicksSpentCharging;
                }
                ticksSpentCharging++;
            };
            toil.AddEndCondition(delegate
            {
                if (powerBuilding == null || powerBuilding.PowerComp == null)
                {
                    return JobCondition.Incompletable;
                }
                if (powerBuilding.PowerComp?.PowerNet.CurrentStoredEnergy() < PowerNetEnergyDrainedPerTick)
                {
                    return JobCondition.Incompletable;
                }
                if (energyNeed.CurLevelPercentage >= 0.99f)
                {
                    return JobCondition.Succeeded;
                }
                return (ticksSpentCharging <= MaxTicksSpentCharging) ? JobCondition.Ongoing : JobCondition.Incompletable;
            });
            if (!TargetB.IsValid)
            {
                toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.ClosestTouch);
            }
            else
            {
                toil.FailOnCannotTouch(TargetIndex.B, PathEndMode.OnCell);
            }
            toil.WithProgressBar(TargetIndex.A, () => energyNeed.CurLevelPercentage);
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            yield return toil;
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (!this.pawn.CanReserve(this.TargetA))
                return false;
            if (this.TargetB.IsValid)
            {
                if (!this.pawn.CanReserve(this.TargetB))
                    return false;
                this.pawn.Reserve(this.TargetB, this.job, errorOnFailed: errorOnFailed);
            }
            this.pawn.Reserve(this.TargetA, this.job, errorOnFailed: errorOnFailed);
            return true;
        }
    }
}
