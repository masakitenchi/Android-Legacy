// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_FillAndroidPrinter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Diagnostics;

namespace Androids
{
    public class JobDriver_FillAndroidPrinter : JobDriver
    {
        private const TargetIndex CarryThingIndex = TargetIndex.A;
        private const TargetIndex DestIndex = TargetIndex.B;

        private Building_AndroidPrinter Printer => (Building_AndroidPrinter)(Thing)this.job.GetTarget(TargetIndex.B);

        public override string GetReport() => (string)"ReportHaulingTo".Translate((NamedArgument)(this.pawn.carryTracker.CarriedThing == null ? TargetThingA : (Entity)this.pawn.carryTracker.CarriedThing).LabelCap, (NamedArgument)this.job.targetB.Thing.LabelShort);

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (!this.pawn.CanReserve(this.TargetA) || !this.pawn.CanReserve(this.TargetB))
                return false;
            this.pawn.Reserve(this.TargetA, this.job, errorOnFailed: errorOnFailed);
            this.pawn.Reserve(this.TargetB, this.job, errorOnFailed: errorOnFailed);
            return true;
        }

        [DebuggerHidden]
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
            this.FailOn(() => Printer.printerStatus != CrafterStatus.Filling);
            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Reserve.ReserveQueue(TargetIndex.A);
            yield return Toils_Reserve.Reserve(TargetIndex.B);
            yield return Toils_Reserve.ReserveQueue(TargetIndex.B);
            Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
            yield return getToHaulTarget;
            yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
            yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
            yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
            yield return Toils_Haul.CarryHauledThingToContainer();
        }
    }
}
