// Decompiled with JetBrains decompiler
// Type: Androids.EnergySourceComp
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Androids
{
    public class EnergySourceComp : ThingComp
    {
        public CompProperties_EnergySource EnergyProps => this.props as CompProperties_EnergySource;

        public virtual void RechargeEnergyNeed(Pawn targetPawn)
        {
            Need_Energy need = targetPawn.needs.TryGetNeed<Need_Energy>();
            if (need == null)
                return;
            if (this.EnergyProps.isConsumable)
            {
                float num = parent.stackCount * this.EnergyProps.energyWhenConsumed;
                need.CurLevel += num;
            }
            else
                need.CurLevel += this.EnergyProps.passiveEnergyGeneration;
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            EnergySourceComp energySourceComp = this;
            Need_Energy need = selPawn.needs.TryGetNeed<Need_Energy>();
            if (energySourceComp.EnergyProps.isConsumable && need != null)
            {
                int thingCount = (int)Math.Ceiling(((double)need.MaxLevel - (double)need.CurLevel) / energySourceComp.EnergyProps.energyWhenConsumed);
                if (thingCount > 0)
                    yield return new FloatMenuOption((string)"AndroidConsumeEnergySource".Translate((NamedArgument)energySourceComp.parent.LabelCap), () =>
                    {
                        Pawn_JobTracker jobs = selPawn.jobs;
                        Job job = new Job(JobDefOf.ChJAndroidRechargeEnergyComp, new LocalTargetInfo(parent));
                        job.count = thingCount;
                        JobTag? tag = new JobTag?(JobTag.Misc);
                        jobs.TryTakeOrderedJob(job, tag);
                    }, revalidateClickTarget: energySourceComp.parent);
            }
        }
    }
}
