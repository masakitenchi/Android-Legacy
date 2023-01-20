// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_RechargeEnergy
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
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
    public int ticksSpentCharging = 0;

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
      this.FailOnDestroyedNullOrForbidden<JobDriver_RechargeEnergy>(TargetIndex.A);
      this.AddFailCondition((Func<bool>) (() => this.energyNeed == null));
      yield return Toils_Reserve.Reserve(TargetIndex.A);
      if (!this.TargetB.IsValid)
      {
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
      }
      else
      {
        yield return Toils_Reserve.Reserve(TargetIndex.B);
        yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
      }
      Toil rechargeToil = new Toil();
      rechargeToil.tickAction = (Action) (() =>
      {
        CompPower powerComp = this.powerBuilding.PowerComp;
        CompPowerBattery compPowerBattery1;
        if (powerComp == null)
        {
          compPowerBattery1 = (CompPowerBattery) null;
        }
        else
        {
          PowerNet powerNet = powerComp.PowerNet;
          if (powerNet == null)
          {
            compPowerBattery1 = (CompPowerBattery) null;
          }
          else
          {
            List<CompPowerBattery> batteryComps = powerNet.batteryComps;
            compPowerBattery1 = batteryComps != null ? batteryComps.FirstOrDefault<CompPowerBattery>((Predicate<CompPowerBattery>) (battery => (double) battery.StoredEnergy > (double) this.PowerNetEnergyDrainedPerTick)) : (CompPowerBattery) null;
          }
        }
        CompPowerBattery compPowerBattery2 = compPowerBattery1;
        if (compPowerBattery2 != null)
        {
          compPowerBattery2.DrawPower(this.PowerNetEnergyDrainedPerTick);
          this.energyNeed.CurLevel += this.energyNeed.MaxLevel / (float) this.MaxTicksSpentCharging;
        }
        ++this.ticksSpentCharging;
      });
      rechargeToil.AddEndCondition((Func<JobCondition>) (() =>
      {
        if (this.powerBuilding == null || this.powerBuilding.PowerComp == null)
          return JobCondition.Incompletable;
        float? nullable = this.powerBuilding.PowerComp?.PowerNet.CurrentStoredEnergy();
        float energyDrainedPerTick = this.PowerNetEnergyDrainedPerTick;
        if ((double) nullable.GetValueOrDefault() < (double) energyDrainedPerTick & nullable.HasValue)
          return JobCondition.Incompletable;
        if ((double) this.energyNeed.CurLevelPercentage >= 0.99000000953674316)
          return JobCondition.Succeeded;
        return this.ticksSpentCharging > this.MaxTicksSpentCharging ? JobCondition.Incompletable : JobCondition.Ongoing;
      }));
      if (!this.TargetB.IsValid)
        rechargeToil.FailOnCannotTouch<Toil>(TargetIndex.A, PathEndMode.ClosestTouch);
      else
        rechargeToil.FailOnCannotTouch<Toil>(TargetIndex.B, PathEndMode.OnCell);
      rechargeToil.WithProgressBar(TargetIndex.A, (Func<float>) (() => this.energyNeed.CurLevelPercentage));
      rechargeToil.defaultCompleteMode = ToilCompleteMode.Never;
      yield return rechargeToil;
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
