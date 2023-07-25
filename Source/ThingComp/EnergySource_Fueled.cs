// Decompiled with JetBrains decompiler
// Type: Androids.EnergySource_Fueled
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
        public double fuelAmountLoaded;
        public bool autoRefuel = true;

        public double FuelUsedPerInterval => this.EnergyProps.fuelAmountUsedPerInterval;

        public float MissingFuel => this.EnergyProps.maxFuelAmount - (float)this.fuelAmountLoaded;

        public float MissingFuelPercentage => this.MissingFuel / this.EnergyProps.maxFuelAmount;

        public bool LoadFuel(Thing fuel, bool doNotDestroy = false)
        {
            if (fuel.stackCount <= 0)
                return false;
            ThingOrderRequest thingOrderRequest = this.EnergyProps.fuels.FirstOrDefault<ThingOrderRequest>((Func<ThingOrderRequest, bool>)(req => req.thingDef == fuel.def));
            if (thingOrderRequest == null)
                return false;
            int num = Math.Min((int)Math.Ceiling((double)this.MissingFuel / (double)thingOrderRequest.amount), fuel.stackCount);
            if (num <= 0)
                return false;
            this.fuelAmountLoaded += (double)(int)Math.Ceiling((double)num * (double)thingOrderRequest.amount);
            if (this.fuelAmountLoaded > (double)this.EnergyProps.maxFuelAmount)
                this.fuelAmountLoaded = (double)this.EnergyProps.maxFuelAmount;
            fuel.stackCount -= num;
            if (!doNotDestroy && fuel.stackCount <= 0)
                fuel.Destroy();
            return true;
        }

        public int CalculateFuelNeededToRefill(Thing fuel)
        {
            ThingOrderRequest thingOrderRequest = this.EnergyProps.fuels.FirstOrDefault<ThingOrderRequest>((Func<ThingOrderRequest, bool>)(req => req.thingDef == fuel.def));
            return thingOrderRequest != null ? Math.Min((int)Math.Ceiling((double)this.MissingFuel / (double)thingOrderRequest.amount), fuel.stackCount) : -1;
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
            {
                yield return new Gizmo_EnergySourceFueled
                {
                    apparel = (parent as Apparel),
                    fueledEnergySource = this
                };
            }
            yield return new Command_Toggle
            {
                defaultLabel = "AndroidGizmoAutoRefuelLabel".Translate(),
                defaultDesc = "AndroidGizmoAutoRefuelDescription".Translate(),
                isActive = () => autoRefuel,
                Order = -99f,
                toggleAction = delegate
                {
                    autoRefuel = !autoRefuel;
                },
                icon = ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel")
            };
            yield return new Command_Action
            {
                defaultLabel = "AndroidGizmoRefuelNowLabel".Translate(),
                defaultDesc = "AndroidGizmoRefuelNowDescription".Translate(),
                icon = RimWorld.ThingDefOf.Chemfuel.uiIcon,
                Order = -99f,
                action = delegate
                {
                    if (parent is Apparel apparel)
                    {
                        Thing thing = FuelUtility.FindSuitableFuelForPawn(apparel.Wearer, this);
                        if (thing != null)
                        {
                            int num = CalculateFuelNeededToRefill(thing);
                            if (num > 0)
                            {
                                Job job = new Job(base.EnergyProps.refillJob, parent, thing)
                                {
                                    count = num
                                };
                                apparel.Wearer.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                            }
                        }
                    }
                }
            };
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
            if (!targetPawn.IsCaravanMember() || (double)this.MissingFuelPercentage <= 0.800000011920929)
                return;
            Thing fuel1 = targetPawn.GetCaravan().Goods.FirstOrDefault<Thing>((Func<Thing, bool>)(fuelThing => this.EnergyProps.fuels.Any<ThingOrderRequest>((Predicate<ThingOrderRequest>)(req => req.thingDef == fuelThing.def))));
            if (fuel1 == null)
                return;
            int fuelNeededToRefill = this.CalculateFuelNeededToRefill(fuel1);
            if (fuelNeededToRefill <= 0)
                return;
            Thing fuel2 = fuel1.SplitOff(fuelNeededToRefill);
            if (fuel2 == null)
                return;
            this.LoadFuel(fuel2);
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            yield return new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, (string)"AndroidFuelEfficencyStatPartLabel".Translate(), this.FuelUsageModifier().ToString("F2"), (string)"AndroidFuelEfficencyStatPartReport".Translate(), 0);
        }
    }
}
