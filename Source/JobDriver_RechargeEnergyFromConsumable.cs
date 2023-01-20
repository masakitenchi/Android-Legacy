// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_RechargeEnergyFromConsumable
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System;
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
    public bool isUsedFromInventory = false;
    public bool thingIsSplitOff = false;

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
      return targetThingA != null && (targetThingA.ParentHolder is Pawn_CarryTracker || targetThingA.ParentHolder is Pawn_InventoryTracker);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
      this.FailOnDestroyedNullOrForbidden<JobDriver_RechargeEnergyFromConsumable>(TargetIndex.A);
      if (!this.TargetB.IsValid)
        this.AddFailCondition((Func<bool>) (() => this.energyNeed == null));
      if (!this.isUsedFromInventory)
      {
        yield return Toils_Reserve.Reserve(TargetIndex.A);
        if (this.TargetB.IsValid)
          yield return Toils_Reserve.Reserve(TargetIndex.B);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting<Toil>(TargetIndex.A);
        yield return Toils_Reserve.Release(TargetIndex.A);
      }
      else
        yield return new Toil()
        {
          initAction = (Action) (() =>
          {
            if (this.thingIsSplitOff || this.pawn.carryTracker.CarriedThing == this.TargetThingA)
              return;
            Thing thing = this.TargetThingA.SplitOff(this.job.count);
            this.thingIsSplitOff = true;
            GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near);
            this.TargetThingA = thing;
          })
        };
      yield return Toils_Haul.StartCarryThing(TargetIndex.A, subtractNumTakenFromJobCount: true);
      this.AddFinishAction((Action) (() =>
      {
        Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
        Thing carriedThing = new Thing();
        int num;
        if (carryTracker != null)
        {
          carriedThing = carryTracker.CarriedThing;
          num = carriedThing != null ? 1 : 0;
        }
        else
          num = 0;
        if (num == 0)
          return;
        if (this.isUsedFromInventory)
          this.pawn.inventory.innerContainer.TryAddOrTransfer(carriedThing);
        else
          carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out Thing _);
      }));
      if (this.TargetB.IsValid)
      {
        yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnForbidden<Toil>(TargetIndex.B);
        yield return Toils_General.Wait(100).WithProgressBarToilDelay(TargetIndex.A);
        Toil rechargeToil = new Toil();
        rechargeToil.AddFinishAction((Action) (() =>
        {
          Thing carriedThing = this.pawn.carryTracker.CarriedThing;
          if (carriedThing == null)
            return;
          carriedThing.TryGetComp<EnergySourceComp>()?.RechargeEnergyNeed((Pawn) this.TargetB.Thing);
          this.pawn.carryTracker.DestroyCarriedThing();
        }));
        yield return rechargeToil;
        yield return Toils_Reserve.Release(TargetIndex.B);
        rechargeToil = (Toil) null;
      }
      else
      {
        yield return Toils_General.Wait(100).WithProgressBarToilDelay(TargetIndex.A);
        Toil rechargeToil = new Toil();
        rechargeToil.AddFinishAction((Action) (() =>
        {
          Thing carriedThing = this.pawn.carryTracker.CarriedThing;
          if (carriedThing == null)
            return;
          carriedThing.TryGetComp<EnergySourceComp>()?.RechargeEnergyNeed(this.pawn);
          this.pawn.carryTracker.DestroyCarriedThing();
        }));
        yield return rechargeToil;
        rechargeToil = (Toil) null;
      }
    }
  }
}
