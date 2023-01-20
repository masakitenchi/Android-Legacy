// Decompiled with JetBrains decompiler
// Type: Androids.DeathActionWorker_Android
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using Androids.Integration;
using System;
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
        float radius = firstHediffOfDef.Severity * AndroidsModSettings.Instance.androidExplosionRadius * comp.energy;
        if (firstHediffOfDef != null && (double) radius >= 1.0)
          GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, RimWorld.DamageDefOf.Bomb, (Thing) corpse.InnerPawn);
      }
      else
      {
        Hediff firstHediffOfDef1 = innerPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjAndroidLike);
        AndroidLikeHediff androidLikeHediff = new AndroidLikeHediff();
        int num;
        if (firstHediffOfDef1 != null)
        {
          androidLikeHediff = firstHediffOfDef1 as AndroidLikeHediff;
          num = androidLikeHediff != null ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          bool flag = innerPawn.health.hediffSet.hediffs.Any<Hediff>((Predicate<Hediff>) (hediff => hediff.CauseDeathNow()));
          Hediff firstHediffOfDef2 = innerPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjOverheating);
          if (firstHediffOfDef2 == null || firstHediffOfDef2 == null && flag)
            return;
          float radius = firstHediffOfDef2.Severity * AndroidsModSettings.Instance.androidExplosionRadius * androidLikeHediff.energyTracked;
          if (firstHediffOfDef2 != null && (double) radius >= 1.0)
            GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, RimWorld.DamageDefOf.Bomb, (Thing) corpse.InnerPawn);
        }
        else
          Log.Warning("Androids.DeathActionWorker_Android: EnergyTrackerComp is null at or is not Android Like either: " + corpse.ThingID);
      }
    }
  }
}
