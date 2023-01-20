// Decompiled with JetBrains decompiler
// Type: Androids.Projectile_Spawner
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace Androids
{
  public class Projectile_Spawner : Projectile
  {
    private Lord lord = (Lord) null;

    public SpawnerProjectileProperties SpawnerProps => this.def.GetModExtension<SpawnerProjectileProperties>();

    public virtual void DoSpawn(Thing hitThing)
    {
      Pawn pawn = (Pawn) null;
      if (this.SpawnerProps.pawnKind != null)
        pawn = PawnGenerator.GeneratePawn(this.SpawnerProps.pawnKind);
      if (this.SpawnerProps.pawnThingDef != null)
        pawn = (Pawn) ThingMaker.MakeThing(this.SpawnerProps.pawnThingDef);
      if (pawn == null)
        return;
      pawn.SetFaction(this.SpawnerProps.GetFaction(this.launcher), (Pawn) null);
      if (this.SpawnerProps.forceAgeToZero)
      {
        pawn.ageTracker.AgeBiologicalTicks = 0L;
        pawn.ageTracker.AgeChronologicalTicks = 0L;
      }
      GenPlace.TryPlaceThing((Thing) pawn, this.Position, this.Map, ThingPlaceMode.Near);
      if (this.SpawnerProps.mentalStateUponSpawn != null)
        pawn.mindState.mentalStateHandler.TryStartMentalState(this.SpawnerProps.mentalStateUponSpawn, forceWake: true);
      if (this.SpawnerProps.joinLordOnSpawn)
      {
        if (this.lord == null && !this.SpawnerProps.joinSameLordFromProjectile)
          this.lord = this.GetLord(pawn);
        this.lord.AddPawn(pawn);
      }
      FleckMaker.ThrowSmoke(pawn.Position.ToVector3(), this.Map, Rand.Range(0.5f, 1.5f));
      FleckMaker.ThrowSmoke(pawn.Position.ToVector3(), this.Map, Rand.Range(1f, 3f));
      FleckMaker.ThrowAirPuffUp(pawn.Position.ToVector3(), this.Map);
    }

    public Lord GetLord(Pawn forPawn)
    {
      Lord lord = (Lord) null;
      Faction faction = forPawn.Faction;
      if (forPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any<Pawn>((Predicate<Pawn>) (p => p != forPawn)))
        lord = ((Pawn) GenClosest.ClosestThing_Global(forPawn.Position, (IEnumerable) forPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), this.SpawnerProps.lordJoinRadius, (Predicate<Thing>) (p => p != forPawn && ((Pawn) p).GetLord() != null))).GetLord();
      if (lord == null)
      {
        LordJob jobForLord = this.SpawnerProps.CreateJobForLord(forPawn.Position);
        lord = LordMaker.MakeNewLord(faction, jobForLord, this.Map);
      }
      return lord;
    }

    protected override void Impact(Thing hitThing, bool blockedByShield = false)
    {
      SoundDef soundExplode = this.def.projectile.soundExplode;
      if (soundExplode != null)
        soundExplode.PlayOneShot((SoundInfo) new TargetInfo(this.Position, this.Map));
      if (this.SpawnerProps.joinSameLordFromProjectile)
        this.lord = LordMaker.MakeNewLord(this.Faction, this.SpawnerProps.CreateJobForLord(this.Position), this.Map);
      for (int index = 0; index < this.SpawnerProps.amount; ++index)
        this.DoSpawn(hitThing);
      this.Destroy(DestroyMode.Vanish);
    }
  }
}
