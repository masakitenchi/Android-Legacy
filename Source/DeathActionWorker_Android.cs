// Decompiled with JetBrains decompiler
// Type: Androids.DeathActionWorker_Android
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Androids.Integration;
using System;
using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class DeathActionWorker_Android : DeathActionWorker
  {
    public override void PawnDied(Corpse corpse)
    {
      if (!AndroidsModSettings.Instance.androidExplodesOnDeath)
        return;
      Pawn innerPawn = corpse.InnerPawn;
      EnergyTrackerComp comp = innerPawn.TryGetComp<EnergyTrackerComp>();
      if (comp != null)
      {
        bool flag = innerPawn.health.hediffSet.hediffs.Any<Hediff>((Predicate<Hediff>) (hediff => hediff.CauseDeathNow()));
        Hediff firstHediffOfDef = innerPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjOverheating);
        if (firstHediffOfDef == null || firstHediffOfDef == null && flag)
          return;
        float num = firstHediffOfDef.Severity * AndroidsModSettings.Instance.androidExplosionRadius * comp.energy;
        if (firstHediffOfDef == null || (double) num < 1.0)
          return;
        GenExplosion.DoExplosion(corpse.Position, corpse.Map, num, RimWorld.DamageDefOf.Bomb, (Thing) corpse.InnerPawn, -1, -1f, (SoundDef) null, (ThingDef) null, (ThingDef) null, (Thing) null, (ThingDef) null, 0.0f, 1, null, false, (ThingDef) null, 0.0f, 1, 0.0f, false, new float?(), (List<Thing>) null);
      }
      else
      {
        Hediff firstHediffOfDef1 = innerPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjAndroidLike);
        if (firstHediffOfDef1 != null && firstHediffOfDef1 is AndroidLikeHediff androidLikeHediff)
        {
          bool flag = innerPawn.health.hediffSet.hediffs.Any<Hediff>((Predicate<Hediff>) (hediff => hediff.CauseDeathNow()));
          Hediff firstHediffOfDef2 = innerPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjOverheating);
          if (firstHediffOfDef2 == null || firstHediffOfDef2 == null && flag)
            return;
          float num = firstHediffOfDef2.Severity * AndroidsModSettings.Instance.androidExplosionRadius * androidLikeHediff.energyTracked;
          if (firstHediffOfDef2 == null || (double) num < 1.0)
            return;
          GenExplosion.DoExplosion(corpse.Position, corpse.Map, num, RimWorld.DamageDefOf.Bomb, (Thing) corpse.InnerPawn, -1, -1f, (SoundDef) null, (ThingDef) null, (ThingDef) null, (Thing) null, (ThingDef) null, 0.0f, 1, null, false, (ThingDef) null, 0.0f, 1, 0.0f, false, new float?(), (List<Thing>) null);
        }
        else
          Log.Warning("Androids.DeathActionWorker_Android: EnergyTrackerComp is null at or is not Android Like either: " + corpse.ThingID);
      }
    }
  }
}
