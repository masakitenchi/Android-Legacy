// Decompiled with JetBrains decompiler
// Type: Androids.SpawnerProjectileProperties
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;
using Verse.AI.Group;

namespace Androids
{
  public class SpawnerProjectileProperties : DefModExtension
  {
    public PawnKindDef pawnKind;
    public ThingDef pawnThingDef;
    public int amount = 1;
    public FactionDef forcedFaction;
    public bool usePlayerFaction = true;
    public bool forceAgeToZero = false;
    public MentalStateDef mentalStateUponSpawn;
    public bool joinLordOnSpawn;
    public System.Type lordJob = typeof (LordJob_DefendPoint);
    public float lordJoinRadius = 99999f;
    public bool joinSameLordFromProjectile = true;

    public LordJob CreateJobForLord(IntVec3 point)
    {
      LordJob instance;
      if (((IEnumerable<ConstructorInfo>) this.lordJob.GetConstructors()).Any<ConstructorInfo>((Func<ConstructorInfo, bool>) (constructor =>
      {
        ParameterInfo[] parameters = constructor.GetParameters();
        return parameters != null && ((IEnumerable<ParameterInfo>) parameters).Count<ParameterInfo>() > 0 && parameters[0].ParameterType == typeof (IntVec3);
      })))
        instance = (LordJob) Activator.CreateInstance(this.lordJob, (object) point);
      else
        instance = (LordJob) Activator.CreateInstance(this.lordJob);
      return instance;
    }

    public Faction GetFaction(Thing launcher)
    {
      if (this.usePlayerFaction)
        return Faction.OfPlayer;
      return this.forcedFaction == null ? launcher.Faction : FactionUtility.DefaultFactionFrom(this.forcedFaction);
    }
  }
}
