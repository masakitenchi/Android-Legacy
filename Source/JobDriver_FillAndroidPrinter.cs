// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_FillAndroidPrinter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace Androids
{
  public class JobDriver_FillAndroidPrinter : JobDriver
  {
    private const TargetIndex CarryThingIndex = TargetIndex.A;
    private const TargetIndex DestIndex = TargetIndex.B;

    private Building_AndroidPrinter Printer => (Building_AndroidPrinter) (Thing) this.job.GetTarget(TargetIndex.B);

    public override string GetReport() => (string) "ReportHaulingTo".Translate((NamedArgument) (this.pawn.carryTracker.CarriedThing == null ? (Entity) this.TargetThingA : (Entity) this.pawn.carryTracker.CarriedThing).LabelCap, (NamedArgument) this.job.targetB.Thing.LabelShort);

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
      if (!this.pawn.CanReserve(this.TargetA) || !this.pawn.CanReserve(this.TargetB))
        return false;
      this.pawn.Reserve(this.TargetA, this.job, errorOnFailed: errorOnFailed);
      this.pawn.Reserve(this.TargetB, this.job, errorOnFailed: errorOnFailed);
      return true;
    }

    [DebuggerHidden]
    protected override IEnumerable<Toil> MakeNewToils()
    {
      this.FailOnDestroyedOrNull<JobDriver_FillAndroidPrinter>(TargetIndex.A);
      this.FailOnDestroyedNullOrForbidden<JobDriver_FillAndroidPrinter>(TargetIndex.B);
      this.FailOn<JobDriver_FillAndroidPrinter>((Func<bool>) (() => this.Printer.printerStatus != CrafterStatus.Filling));
      yield return Toils_Reserve.Reserve(TargetIndex.A);
      yield return Toils_Reserve.ReserveQueue(TargetIndex.A);
      yield return Toils_Reserve.Reserve(TargetIndex.B);
      yield return Toils_Reserve.ReserveQueue(TargetIndex.B);
      Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting<Toil>(TargetIndex.A);
      yield return getToHaulTarget;
      yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting<Toil>(TargetIndex.A);
      yield return Toils_Haul.StartCarryThing(TargetIndex.A, subtractNumTakenFromJobCount: true);
      yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
      Toil carryToContainer = Toils_Haul.CarryHauledThingToContainer();
      yield return carryToContainer;
    }
  }
}
