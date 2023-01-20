// Decompiled with JetBrains decompiler
// Type: Androids.HibernationComp
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
  public class HibernationComp : ThingComp
  {
    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
    {
      EnergyTrackerComp energyTracker = selPawn.TryGetComp<EnergyTrackerComp>();
      int num;
      CompProperties_EnergyTracker props = new CompProperties_EnergyTracker();
      if (energyTracker != null)
      {
        props = energyTracker.EnergyProperties;
        if (props != null)
        {
          num = props.canHibernate ? 1 : 0;
          goto label_4;
        }
      }
      num = 0;
label_4:
      if (num != 0)
      {
        if (selPawn.CanReserveAndReach((LocalTargetInfo) (Thing) this.parent, PathEndMode.OnCell, Danger.Deadly))
        {
          CompPowerTrader power = this.parent.TryGetComp<CompPowerTrader>();
          if (power != null)
          {
            if (power.PowerOn)
            {
              FloatMenuOption option = new FloatMenuOption((string) "AndroidMachinelikeHibernate".Translate((NamedArgument) selPawn.Name.ToStringShort), (Action) (() => selPawn.jobs.TryTakeOrderedJob(new Job(props.hibernationJob, (LocalTargetInfo) (Thing) this.parent))));
              yield return option;
              option = (FloatMenuOption) null;
            }
            else
            {
              FloatMenuOption option = new FloatMenuOption((string) "AndroidMachinelikeHibernateFailNoPower".Translate((NamedArgument) selPawn.Name.ToStringShort, (NamedArgument) this.parent.LabelCap), (Action) null);
              option.Disabled = true;
              yield return option;
              option = (FloatMenuOption) null;
            }
          }
          else
          {
            FloatMenuOption option = new FloatMenuOption((string) "AndroidMachinelikeHibernate".Translate((NamedArgument) selPawn.Name.ToStringShort), (Action) (() => selPawn.jobs.TryTakeOrderedJob(new Job(props.hibernationJob, (LocalTargetInfo) (Thing) this.parent))));
            yield return option;
            option = (FloatMenuOption) null;
          }
          power = (CompPowerTrader) null;
        }
        else
        {
          FloatMenuOption option = new FloatMenuOption((string) "AndroidMachinelikeHibernateFailReserveOrReach".Translate((NamedArgument) selPawn.Name.ToStringShort, (NamedArgument) this.parent.LabelCap), (Action) null);
          option.Disabled = true;
          yield return option;
          option = (FloatMenuOption) null;
        }
      }
      else
      {
        FloatMenuOption option = new FloatMenuOption((string) "AndroidMachinelikeHibernateFail".Translate((NamedArgument) selPawn.Name.ToStringShort), (Action) null);
        option.Disabled = true;
        yield return option;
        option = (FloatMenuOption) null;
      }
    }
  }
}
