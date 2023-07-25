// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_Hibernate
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
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
            if (!this.pawn.CanReserveAndReach((LocalTargetInfo)this.Target, PathEndMode.OnCell, Danger.Deadly))
                return false;
            this.pawn.Reserve((LocalTargetInfo)this.Target, this.job, errorOnFailed: errorOnFailed);
            return true;
        }

        public override RandomSocialMode DesiredSocialMode() => RandomSocialMode.Off;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            powerTrader = Target.TryGetComp<CompPowerTrader>();
            this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
            Toil toil = new Toil();
            toil.initAction = delegate
            {
                pawn.pather.StopDead();
            };
            toil.tickAction = delegate
            {
                if (pawn.mindState.lastHarmTick - Find.TickManager.TicksGame >= -20)
                {
                    EndJobWith(JobCondition.InterruptOptional);
                }
                if (Find.TickManager.TicksGame % 200 == 0)
                {
                    foreach (IntVec3 item in pawn.CellsAdjacent8WayAndInside())
                    {
                        if (item.InBounds(pawn.Map) && item.GetFirstThing(pawn.Map, RimWorld.ThingDefOf.Fire) != null)
                        {
                            EndJobWith(JobCondition.InterruptOptional);
                            break;
                        }
                    }
                }
                if (powerTrader != null && !powerTrader.PowerOn)
                {
                    EndJobWith(JobCondition.InterruptOptional);
                }
            };
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            yield return toil;
        }
    }
}
