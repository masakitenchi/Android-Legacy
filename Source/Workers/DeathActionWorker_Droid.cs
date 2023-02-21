// Decompiled with JetBrains decompiler
// Type: Androids.DeathActionWorker_Droid
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Androids.Integration;
using System;
using System.Collections.Generic;
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
        float num = AndroidsModSettings.Instance.androidExplosionRadius * comp.energy;
        if (flag2)
          num *= 2f;
        if ((double) num >= 1.0)
          GenExplosion.DoExplosion(corpse.Position, corpse.Map, num, RimWorld.DamageDefOf.Bomb, (Thing) corpse.InnerPawn, -1, -1f, (SoundDef) null, (ThingDef) null, (ThingDef) null, (Thing) null, (ThingDef) null, 0.0f, 1, null, false, (ThingDef) null, 0.0f, 1, 0.0f, false, new float?(), (List<Thing>) null);
      }
      if (corpse.Destroyed)
        return;
      ButcherUtility.SpawnDrops(corpse.InnerPawn, corpse.Position, corpse.Map);
      if (corpse.InnerPawn.apparel != null)
        corpse.InnerPawn.apparel.DropAll(corpse.PositionHeld, true, true);
      corpse.Destroy(DestroyMode.Vanish);
    }
  }
}
