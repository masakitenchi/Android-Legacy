// Decompiled with JetBrains decompiler
// Type: Androids.Need_Energy
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Androids.Integration;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Androids
{
    public class Need_Energy : Need
    {
        public static float rechargePercentage = 0.505f;

        public EnergyTrackerComp EnergyTracker => this.pawn.TryGetComp<EnergyTrackerComp>();

        public Need_Energy(Pawn pawn) => this.pawn = pawn;

        public override float MaxLevel
        {
            get
            {
                EnergyTrackerComp comp = this.pawn.TryGetComp<EnergyTrackerComp>();
                return comp == null ? 1f : comp.EnergyProperties.maxEnergy;
            }
        }

        public override void SetInitialLevel() => this.CurLevel = this.MaxLevel;

        public virtual void DrawOnGUI(
          Rect rect,
          int maxThresholdMarkers = 2147483647,
          float customMargin = -1f,
          bool drawArrows = true,
          bool doTooltip = true,
          Rect? rectForTooltip = null)
        {
            if (this.threshPercents == null)
                this.threshPercents = new List<float>();
            this.threshPercents.Clear();
            if ((double)this.MaxLevel > 1.0)
            {
                float num = 1f / this.MaxLevel;
                this.threshPercents.Add(num * 0.5f);
                this.threshPercents.Add(num * 0.2f);
                for (int index = 0; index < (int)Math.Floor((double)this.MaxLevel); ++index)
                    this.threshPercents.Add(num + num * (float)index);
            }
            else
            {
                this.threshPercents.Add(0.5f);
                this.threshPercents.Add(0.2f);
            }
            base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, new Rect?());
        }

        public override void NeedInterval()
        {
            if (AndroidsModSettings.Instance.droidCompatibilityMode && this.pawn.def.HasModExtension<MechanicalPawnProperties>())
            {
                foreach (Need allNeed in this.pawn.needs.AllNeeds)
                {
                    if (allNeed.def != NeedsDefOf.ChJEnergy)
                        allNeed.CurLevelPercentage = 1f;
                }
            }
            float num = 1f;
            EnergyTrackerComp comp1 = this.pawn.TryGetComp<EnergyTrackerComp>();
            if (comp1 != null)
            {
                if (!this.pawn.IsCaravanMember() && comp1.EnergyProperties.canHibernate && this.pawn.CurJobDef == comp1.EnergyProperties.hibernationJob)
                    num = -0.1f;
                else
                    num *= comp1.EnergyProperties.drainRateModifier;
            }
            if (this.pawn.needs == null || this.IsFrozen)
                return;
            this.CurLevel -= num * 0.000833333354f;
            if (this.pawn.needs.food != null && (double)this.pawn.needs.food.CurLevelPercentage > 0.0)
                this.CurLevel += 0.0133333337f;
            if (this.pawn.apparel != null)
            {
                foreach (Thing thing in this.pawn.apparel.WornApparel)
                {
                    EnergySourceComp comp2 = thing.TryGetComp<EnergySourceComp>();
                    if (comp2 != null && !comp2.EnergyProps.isConsumable)
                        comp2.RechargeEnergyNeed(this.pawn);
                }
            }
            if (this.pawn.IsCaravanMember() && this.pawn.IsHashIntervalTick(250) && (double)this.CurLevelPercentage < (double)Need_Energy.rechargePercentage)
            {
                Thing thing1 = this.pawn.GetCaravan().Goods.FirstOrDefault<Thing>((Func<Thing, bool>)(thing =>
                {
                    EnergySourceComp comp3 = thing.TryGetComp<EnergySourceComp>();
                    return comp3 != null && comp3.EnergyProps.isConsumable;
                }));
                if (thing1 != null)
                {
                    int count = Math.Min((int)Math.Ceiling(((double)this.MaxLevel - (double)this.CurLevel) / (double)thing1.TryGetComp<EnergySourceComp>().EnergyProps.energyWhenConsumed), thing1.stackCount);
                    Thing thing2 = thing1.SplitOff(count);
                    thing2.TryGetComp<EnergySourceComp>().RechargeEnergyNeed(this.pawn);
                    thing2.Destroy();
                }
            }
            if ((double)this.CurLevel < 0.20000000298023224)
            {
                if (!this.pawn.health.hediffSet.HasHediff(HediffDefOf.ChjPowerShortage))
                    this.pawn.health.AddHediff(HediffDefOf.ChjPowerShortage);
            }
            else if (this.pawn.health.hediffSet.HasHediff(HediffDefOf.ChjPowerShortage))
                this.pawn.health.RemoveHediff(this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjPowerShortage));
            if ((double)this.CurLevel > 0.0)
                return;
            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.ChjPowerFailure, this.pawn);
            this.pawn.health.AddHediff(hediff);
            this.pawn.Kill(null, exactCulprit: hediff);
        }
    }
}
