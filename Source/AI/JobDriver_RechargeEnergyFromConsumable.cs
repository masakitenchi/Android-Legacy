// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_RechargeEnergyFromConsumable
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Androids
{
    public class JobDriver_RechargeEnergyFromConsumable : JobDriver
    {
        private const TargetIndex PowerDestIndex = TargetIndex.A;
        private const TargetIndex OtherPawnIndex = TargetIndex.B;
        public Need_Energy energyNeed;
        public bool isUsedFromInventory;
        public bool thingIsSplitOff;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.isUsedFromInventory, "isUsedFromInventory");
            Scribe_Values.Look<bool>(ref this.thingIsSplitOff, "thingIsSplitOff");
        }

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            this.energyNeed = this.GetActor().needs.TryGetNeed<Need_Energy>();
            if (this.startTick != Find.TickManager.TicksGame || !this.TargetThingAIsCarriedOrInInventory())
                return;
            this.isUsedFromInventory = true;
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

        public override string GetReport() => this.TargetB.IsValid ? this.ReportStringProcessed(this.job.def.GetModExtension<ExtraReportStringProperties>().extraReportString) : base.GetReport();

        private bool TargetThingAIsCarriedOrInInventory()
        {
            Thing targetThingA = this.TargetThingA;
            if (targetThingA == null)
                return false;
            return targetThingA.ParentHolder is Pawn_CarryTracker || targetThingA.ParentHolder is Pawn_InventoryTracker;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
            if (!TargetB.IsValid)
            {
                AddFailCondition(() => energyNeed == null);
            }
            if (!isUsedFromInventory)
            {
                yield return Toils_Reserve.Reserve(TargetIndex.A);
                if (TargetB.IsValid)
                {
                    yield return Toils_Reserve.Reserve(TargetIndex.B);
                }
                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
                yield return Toils_Reserve.Release(TargetIndex.A);
            }
            else
            {
                yield return new Toil
                {
                    initAction = delegate
                    {
                        if (!thingIsSplitOff && pawn.carryTracker.CarriedThing != TargetThingA)
                        {
                            Thing thing = TargetThingA.SplitOff(job.count);
                            thingIsSplitOff = true;
                            GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near);
                            TargetThingA = thing;
                        }
                    }
                };
            }
            yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
            AddFinishAction(delegate
            {
                Pawn_CarryTracker carryTracker = pawn.carryTracker;
                if (carryTracker != null)
                {
                    Thing carriedThing3 = carryTracker.CarriedThing;
                    if (carriedThing3 != null)
                    {
                        if (isUsedFromInventory)
                        {
                            pawn.inventory.innerContainer.TryAddOrTransfer(carriedThing3);
                        }
                        else
                        {
                            carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out var _);
                        }
                    }
                }
            });
            if (TargetB.IsValid)
            {
                yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnForbidden(TargetIndex.B);
                yield return Toils_General.Wait(100).WithProgressBarToilDelay(TargetIndex.A);
                Toil toil = new Toil();
                toil.AddFinishAction(delegate
                {
                    Thing carriedThing2 = pawn.carryTracker.CarriedThing;
                    if (carriedThing2 != null)
                    {
                        carriedThing2.TryGetComp<EnergySourceComp>()?.RechargeEnergyNeed((Pawn)TargetB.Thing);
                        pawn.carryTracker.DestroyCarriedThing();
                    }
                });
                yield return toil;
                yield return Toils_Reserve.Release(TargetIndex.B);
                yield break;
            }
            yield return Toils_General.Wait(100).WithProgressBarToilDelay(TargetIndex.A);
            Toil toil2 = new Toil();
            toil2.AddFinishAction(delegate
            {
                Thing carriedThing = pawn.carryTracker.CarriedThing;
                if (carriedThing != null)
                {
                    carriedThing.TryGetComp<EnergySourceComp>()?.RechargeEnergyNeed(pawn);
                    pawn.carryTracker.DestroyCarriedThing();
                }
            });
            yield return toil2;
        }
    }
}
