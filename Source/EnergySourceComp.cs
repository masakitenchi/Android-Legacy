// Decompiled with JetBrains decompiler
// Type: Androids.EnergySourceComp
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

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
        float num = (float) this.parent.stackCount * this.EnergyProps.energyWhenConsumed;
        need.CurLevel += num;
      }
      else
        need.CurLevel += this.EnergyProps.passiveEnergyGeneration;
    }

    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
    {
      Need_Energy energyNeed = selPawn.needs.TryGetNeed<Need_Energy>();
      if (this.EnergyProps.isConsumable && energyNeed != null)
      {
        int thingCount = (int) Math.Ceiling(((double) energyNeed.MaxLevel - (double) energyNeed.CurLevel) / (double) this.EnergyProps.energyWhenConsumed);
        if (thingCount > 0)
        {
          FloatMenuOption floatMenuOption = new FloatMenuOption((string) "AndroidConsumeEnergySource".Translate((NamedArgument) this.parent.LabelCap), (Action) (() =>
          {
            Pawn_JobTracker jobs = selPawn.jobs;
            Job job = new Job(JobDefOf.ChJAndroidRechargeEnergyComp, new LocalTargetInfo((Thing) this.parent));
            job.count = thingCount;
            JobTag? tag = new JobTag?(JobTag.Misc);
            jobs.TryTakeOrderedJob(job, tag);
          }), revalidateClickTarget: ((Thing) this.parent));
          yield return floatMenuOption;
          floatMenuOption = (FloatMenuOption) null;
        }
      }
    }
  }
}
