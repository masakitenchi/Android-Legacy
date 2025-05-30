﻿// Decompiled with JetBrains decompiler
// Type: Androids.AndroidLikeHediff
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Verse.AI.Group;

namespace Androids
{
    public class AndroidLikeHediff : HediffWithComps
    {
        public float energyTracked;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.energyTracked, "energyTracked");
        }

        public override void Tick()
        {
            base.Tick();
            Need_Energy need = this.pawn.needs.TryGetNeed<Need_Energy>();
            if (need == null)
                return;
            this.energyTracked = need.CurLevel;
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            if (!this.pawn.health.hediffSet.HasHediff(HediffDefOf.ChjAndroidLike) || ThingDefOf.ChjAndroid.race.DeathActionWorker == null || this.pawn.Corpse == null)
                return;
            ThingDefOf.ChjAndroid.race.DeathActionWorker.PawnDied(this.pawn.Corpse, this.pawn.GetLord());

        }
    }
}
