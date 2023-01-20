// Decompiled with JetBrains decompiler
// Type: Androids.DeathActionWorker_Droid
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using Androids.Integration;
using System;
using Verse;

namespace Androids
{
  public class DeathActionWorker_Droid : DeathActionWorker
  {
    public override void PawnDied(Corpse corpse)
    {
      if (!AndroidsModSettings.Instance.androidExplodesOnDeath)
        return;
      Pawn innerPawn = corpse.InnerPawn;
      EnergyTrackerComp comp = innerPawn.TryGetComp<EnergyTrackerComp>();
      bool flag1 = innerPawn.health.hediffSet.hediffs.Any<Hediff>((Predicate<Hediff>) (hediff => hediff.CauseDeathNow()));
      Hediff firstHediffOfDef = innerPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjOverheating);
      bool flag2 = firstHediffOfDef != null && (double) firstHediffOfDef.Severity >= 1.0;
      if (firstHediffOfDef != null || !flag1)
      {
        float radius = AndroidsModSettings.Instance.androidExplosionRadius * comp.energy;
        if (flag2)
          radius *= 2f;
        if ((double) radius >= 1.0)
          GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, RimWorld.DamageDefOf.Bomb, (Thing) corpse.InnerPawn);
      }
      if (corpse.Destroyed)
        return;
      ButcherUtility.SpawnDrops(corpse.InnerPawn, corpse.Position, corpse.Map);
      if (corpse.InnerPawn.apparel != null)
        corpse.InnerPawn.apparel.DropAll(corpse.PositionHeld);
      corpse.Destroy(DestroyMode.Vanish);
    }
  }
}
