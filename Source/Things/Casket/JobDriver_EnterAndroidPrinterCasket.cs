﻿// Decompiled with JetBrains decompiler
// Type: Androids.JobDriver_EnterAndroidPrinterCasket
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    public class JobDriver_EnterAndroidPrinterCasket : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed) => this.pawn.Reserve(this.job.targetA, this.job, errorOnFailed: errorOnFailed);
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            Toil toil = Toils_General.Wait(500);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
            toil.WithProgressBarToilDelay(TargetIndex.A);
            yield return toil;
            Toil enter = new Toil();
            enter.initAction = () =>
            {
                Pawn actor = enter.actor;
                Building_AndroidPrinter pod = (Building_AndroidPrinter)actor.CurJob.targetA.Thing;
                Action confirmedAct = () =>
                {
                    actor.DeSpawnOrDeselect(DestroyMode.Vanish);
                    pod.TryAcceptThing(actor, true);
                };
                if (pod.def.building.isPlayerEjectable)
                    confirmedAct();
                else if (this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate(actor.Named("PAWN")).AdjustedFor(actor), confirmedAct));
                else
                    confirmedAct();
            };
            enter.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return enter;
        }
    }
}
