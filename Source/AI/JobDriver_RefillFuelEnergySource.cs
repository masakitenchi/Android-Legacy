// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_RefillFuelEnergySource
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Androids
{
    public class JobDriver_RefillFuelEnergySource : JobDriver
    {
        public TargetIndex FuelIndex => TargetIndex.B;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (!this.TargetB.IsValid || !this.pawn.CanReserve(this.TargetB))
                return false;
            this.pawn.Reserve(this.TargetB, this.job, errorOnFailed: errorOnFailed);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedNullOrForbidden(FuelIndex);
            yield return Toils_Reserve.Reserve(FuelIndex);
            yield return Toils_Goto.GotoThing(FuelIndex, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(FuelIndex);
            yield return Toils_Reserve.Release(FuelIndex);
            yield return Toils_Haul.StartCarryThing(FuelIndex, false, true, false);
            yield return Toils_General.Wait(100).WithProgressBarToilDelay(FuelIndex);
            Toil toil = new Toil();
            toil.AddFinishAction(delegate
            {
                Thing carriedThing = pawn.carryTracker.CarriedThing;
                if (carriedThing != null)
                {
                    base.TargetThingA.TryGetComp<EnergySource_Fueled>()?.LoadFuel(carriedThing);
                    pawn.carryTracker.DestroyCarriedThing();
                }
            });
            yield return toil;
        }
    }
}
