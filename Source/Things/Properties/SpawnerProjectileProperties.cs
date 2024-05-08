// Decompiled with JetBrains decompiler
// Type: Androids.SpawnerProjectileProperties
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
        public bool forceAgeToZero;
        public MentalStateDef mentalStateUponSpawn;
        public bool joinLordOnSpawn;
        public System.Type lordJob = typeof(LordJob_DefendPoint);
        public float lordJoinRadius = 99999f;
        public bool joinSameLordFromProjectile = true;

        public LordJob CreateJobForLord(IntVec3 point)
        {
            LordJob instance;
            if (this.lordJob.GetConstructors().Any<ConstructorInfo>(constructor =>
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                return parameters != null && parameters.Count<ParameterInfo>() > 0 && parameters[0].ParameterType == typeof(IntVec3);
            }))
                instance = (LordJob)Activator.CreateInstance(this.lordJob, point);
            else
                instance = (LordJob)Activator.CreateInstance(this.lordJob);
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
