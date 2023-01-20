// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_RefillFuelEnergySource
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System;
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
      this.FailOnDestroyedNullOrForbidden<JobDriver_RefillFuelEnergySource>(this.FuelIndex);
      yield return Toils_Reserve.Reserve(this.FuelIndex);
      yield return Toils_Goto.GotoThing(this.FuelIndex, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting<Toil>(this.FuelIndex);
      yield return Toils_Reserve.Release(this.FuelIndex);
      yield return Toils_Haul.StartCarryThing(this.FuelIndex, subtractNumTakenFromJobCount: true);
      yield return Toils_General.Wait(100).WithProgressBarToilDelay(this.FuelIndex);
      Toil refuelToil = new Toil();
      refuelToil.AddFinishAction((Action) (() =>
      {
        Thing carriedThing = this.pawn.carryTracker.CarriedThing;
        if (carriedThing == null)
          return;
        this.TargetThingA.TryGetComp<EnergySource_Fueled>()?.LoadFuel(carriedThing);
        this.pawn.carryTracker.DestroyCarriedThing();
      }));
      yield return refuelToil;
    }
  }
}
