// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_Hibernate
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
  public class JobDriver_Hibernate : JobDriver
  {
    public CompPowerTrader powerTrader;

    public Thing Target => this.TargetA.Thing;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
      if (!this.pawn.CanReserveAndReach((LocalTargetInfo) this.Target, PathEndMode.OnCell, Danger.Deadly))
        return false;
      this.pawn.Reserve((LocalTargetInfo) this.Target, this.job, errorOnFailed: errorOnFailed);
      return true;
    }

    public override RandomSocialMode DesiredSocialMode() => RandomSocialMode.Off;

    protected override IEnumerable<Toil> MakeNewToils()
    {
      this.powerTrader = this.Target.TryGetComp<CompPowerTrader>();
      this.FailOnDestroyedNullOrForbidden<JobDriver_Hibernate>(TargetIndex.A);
      yield return Toils_Reserve.Reserve(TargetIndex.A);
      yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
      yield return new Toil()
      {
        initAction = (Action) (() => this.pawn.pather.StopDead()),
        tickAction = (Action) (() =>
        {
          if (this.pawn.mindState.lastHarmTick - Find.TickManager.TicksGame >= -20)
            this.EndJobWith(JobCondition.InterruptOptional);
          if (Find.TickManager.TicksGame % 200 == 0)
          {
            foreach (IntVec3 c in this.pawn.CellsAdjacent8WayAndInside())
            {
              if (c.InBounds(this.pawn.Map) && c.GetFirstThing(this.pawn.Map, RimWorld.ThingDefOf.Fire) != null)
              {
                this.EndJobWith(JobCondition.InterruptOptional);
                break;
              }
            }
          }
          if (this.powerTrader == null || this.powerTrader.PowerOn)
            return;
          this.EndJobWith(JobCondition.InterruptOptional);
        }),
        defaultCompleteMode = ToilCompleteMode.Never
      };
    }
  }
}
