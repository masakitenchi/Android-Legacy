﻿// Decompiled with JetBrains decompiler
// Type: Androids.HibernationComp
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
      HibernationComp hibernationComp = this;
      EnergyTrackerComp comp1 = selPawn.TryGetComp<EnergyTrackerComp>();
      if (comp1 != null)
      {
        CompProperties_EnergyTracker props = comp1.EnergyProperties;
        if (props != null && props.canHibernate)
        {
          if (selPawn.CanReserveAndReach((LocalTargetInfo) (Thing) hibernationComp.parent, PathEndMode.OnCell, Danger.Deadly))
          {
            CompPowerTrader comp2 = hibernationComp.parent.TryGetComp<CompPowerTrader>();
            if (comp2 != null)
            {
              if (comp2.PowerOn)
              {
                yield return new FloatMenuOption((string) "AndroidMachinelikeHibernate".Translate((NamedArgument) selPawn.Name.ToStringShort), (Action) (() => selPawn.jobs.TryTakeOrderedJob(new Job(props.hibernationJob, (LocalTargetInfo) (Thing) this.parent))));
                yield break;
              }
              else
              {
                yield return new FloatMenuOption((string) "AndroidMachinelikeHibernateFailNoPower".Translate((NamedArgument) selPawn.Name.ToStringShort, (NamedArgument) hibernationComp.parent.LabelCap), (Action) null)
                {
                  Disabled = true
                };
                yield break;
              }
            }
            else
            {
              yield return new FloatMenuOption((string) "AndroidMachinelikeHibernate".Translate((NamedArgument) selPawn.Name.ToStringShort), (Action) (() => selPawn.jobs.TryTakeOrderedJob(new Job(props.hibernationJob, (LocalTargetInfo) (Thing) this.parent))));
              yield break;
            }
          }
          else
          {
            yield return new FloatMenuOption((string) "AndroidMachinelikeHibernateFailReserveOrReach".Translate((NamedArgument) selPawn.Name.ToStringShort, (NamedArgument) hibernationComp.parent.LabelCap), (Action) null)
            {
              Disabled = true
            };
            yield break;
          }
        }
      }
      yield return new FloatMenuOption((string) "AndroidMachinelikeHibernateFail".Translate((NamedArgument) selPawn.Name.ToStringShort), (Action) null)
      {
        Disabled = true
      };
    }
  }
}
