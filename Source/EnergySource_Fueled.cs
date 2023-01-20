// Decompiled with JetBrains decompiler
// Type: Androids.EnergySource_Fueled
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Androids
{
  public class EnergySource_Fueled : EnergySourceComp, IExtraDisplayStats
  {
    public double fuelAmountLoaded = 0.0;
    public bool autoRefuel = true;

    public double FuelUsedPerInterval => this.EnergyProps.fuelAmountUsedPerInterval;

    public float MissingFuel => this.EnergyProps.maxFuelAmount - (float) this.fuelAmountLoaded;

    public float MissingFuelPercentage => this.MissingFuel / this.EnergyProps.maxFuelAmount;

    public bool LoadFuel(Thing fuel, bool doNotDestroy = false)
    {
      if (fuel.stackCount <= 0)
        return false;
      ThingOrderRequest thingOrderRequest = this.EnergyProps.fuels.FirstOrDefault<ThingOrderRequest>((Predicate<ThingOrderRequest>) (req => req.thingDef == fuel.def));
      if (thingOrderRequest == null)
        return false;
      int num = Math.Min((int) Math.Ceiling((double) this.MissingFuel / (double) thingOrderRequest.amount), fuel.stackCount);
      if (num <= 0)
        return false;
      this.fuelAmountLoaded += (double) (int) Math.Ceiling((double) num * (double) thingOrderRequest.amount);
      if (this.fuelAmountLoaded > (double) this.EnergyProps.maxFuelAmount)
        this.fuelAmountLoaded = (double) this.EnergyProps.maxFuelAmount;
      fuel.stackCount -= num;
      if (!doNotDestroy && fuel.stackCount <= 0)
        fuel.Destroy();
      return true;
    }

    public int CalculateFuelNeededToRefill(Thing fuel)
    {
      ThingOrderRequest thingOrderRequest = this.EnergyProps.fuels.FirstOrDefault<ThingOrderRequest>((Predicate<ThingOrderRequest>) (req => req.thingDef == fuel.def));
      return thingOrderRequest != null ? Math.Min((int) Math.Ceiling((double) this.MissingFuel / (double) thingOrderRequest.amount), fuel.stackCount) : -1;
    }

    public override void PostExposeData()
    {
      base.PostExposeData();
      Scribe_Values.Look<double>(ref this.fuelAmountLoaded, "fuelAmountLoaded");
      Scribe_Values.Look<bool>(ref this.autoRefuel, "autoRefuel");
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
      if (Find.Selector.NumSelected <= 1)
        yield return (Gizmo) new Gizmo_EnergySourceFueled()
        {
          apparel = (this.parent as Apparel),
          fueledEnergySource = this
        };
      Command_Toggle commandToggle = new Command_Toggle();
      commandToggle.defaultLabel = (string) "AndroidGizmoAutoRefuelLabel".Translate();
      commandToggle.defaultDesc = (string) "AndroidGizmoAutoRefuelDescription".Translate();
      commandToggle.isActive = (Func<bool>) (() => this.autoRefuel);
      commandToggle.Order = -99f;
      commandToggle.toggleAction = (Action) (() => this.autoRefuel = !this.autoRefuel);
      commandToggle.icon = (Texture) ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel");
      yield return (Gizmo) commandToggle;
      Command_Action commandAction = new Command_Action();
      commandAction.defaultLabel = (string) "AndroidGizmoRefuelNowLabel".Translate();
      commandAction.defaultDesc = (string) "AndroidGizmoRefuelNowDescription".Translate();
      commandAction.icon = (Texture) RimWorld.ThingDefOf.Chemfuel.uiIcon;
      commandAction.Order = -99f;
      commandAction.action = (Action) (() =>
      {
        if (!(this.parent is Apparel parent2))
          return;
        Thing suitableFuelForPawn = FuelUtility.FindSuitableFuelForPawn(parent2.Wearer, this);
        if (suitableFuelForPawn != null)
        {
          int fuelNeededToRefill = this.CalculateFuelNeededToRefill(suitableFuelForPawn);
          if (fuelNeededToRefill > 0)
            parent2.Wearer.jobs.TryTakeOrderedJob(new Job(this.EnergyProps.refillJob, (LocalTargetInfo) (Thing) this.parent, (LocalTargetInfo) suitableFuelForPawn)
            {
              count = fuelNeededToRefill
            });
        }
      });
      yield return (Gizmo) commandAction;
    }

    public double FuelUsageModifier()
    {
      double num1 = 1.0;
      double num2 = 1.0;
      CompQuality comp = this.parent.TryGetComp<CompQuality>();
      if (comp != null)
      {
        switch (comp.Quality)
        {
          case QualityCategory.Awful:
            num2 = 2.0;
            break;
          case QualityCategory.Poor:
            num2 = 1.5;
            break;
          case QualityCategory.Normal:
            num2 = 1.0;
            break;
          case QualityCategory.Good:
            num2 = 0.9;
            break;
          case QualityCategory.Excellent:
            num2 = 0.7;
            break;
          case QualityCategory.Masterwork:
            num2 = 0.5;
            break;
          case QualityCategory.Legendary:
            num2 = 0.25;
            break;
        }
      }
      return num1 * num2;
    }

    public override void RechargeEnergyNeed(Pawn targetPawn)
    {
      base.RechargeEnergyNeed(targetPawn);
      if (this.fuelAmountLoaded > 0.0)
      {
        this.fuelAmountLoaded -= this.FuelUsedPerInterval * this.FuelUsageModifier();
        Need_Energy need = targetPawn.needs.TryGetNeed<Need_Energy>();
        if (need != null)
          need.CurLevel += this.EnergyProps.activeEnergyGeneration;
      }
      if (this.fuelAmountLoaded < 0.0)
        this.fuelAmountLoaded = 0.0;
      if (!targetPawn.IsCaravanMember() || (double) this.MissingFuelPercentage <= 0.800000011920929)
        return;
      Thing fuel1 = targetPawn.GetCaravan().Goods.FirstOrDefault<Thing>((Func<Thing, bool>) (fuelThing => this.EnergyProps.fuels.Any<ThingOrderRequest>((Predicate<ThingOrderRequest>) (req => req.thingDef == fuelThing.def))));
      if (fuel1 != null)
      {
        int fuelNeededToRefill = this.CalculateFuelNeededToRefill(fuel1);
        if (fuelNeededToRefill > 0)
        {
          Thing fuel2 = fuel1.SplitOff(fuelNeededToRefill);
          if (fuel2 != null)
            this.LoadFuel(fuel2);
        }
      }
    }

    public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
    {
      StatDrawEntry fuelEfficencyEntry = new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, (string) "AndroidFuelEfficencyStatPartLabel".Translate(), this.FuelUsageModifier().ToString("F2"), (string) "AndroidFuelEfficencyStatPartReport".Translate(), 0);
      yield return fuelEfficencyEntry;
    }
  }
}
