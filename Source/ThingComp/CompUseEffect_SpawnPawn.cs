// Decompiled with JetBrains decompiler
// Type: Androids.CompUseEffect_SpawnPawn
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Androids
{
    public class CompUseEffect_SpawnPawn : CompUseEffect
    {
        public override float OrderPriority => 1000f;

        public CompProperties_SpawnPawn SpawnerProps => this.props as CompProperties_SpawnPawn;

        public virtual void DoSpawn(Pawn usedBy)
        {
            Pawn pawn = PawnGenerator.GeneratePawn(this.SpawnerProps.pawnKind);
            if (pawn == null)
                return;
            pawn.SetFaction(this.GetFaction(), (Pawn)null);
            GenPlace.TryPlaceThing((Thing)pawn, this.parent.Position, this.parent.Map, ThingPlaceMode.Near);
            if (!this.SpawnerProps.sendMessage)
                return;
            Messages.Message((string)"AndroidSpawnedPawnMessageText".Translate((NamedArgument)pawn.Name.ToStringFull), (LookTargets)new GlobalTargetInfo((Thing)pawn), MessageTypeDefOf.NeutralEvent);
        }

        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            for (int index = 0; index < this.SpawnerProps.amount; ++index)
                this.DoSpawn(usedBy);
        }

        public Faction GetFaction()
        {
            if (this.SpawnerProps.usePlayerFaction)
                return Faction.OfPlayer;
            return this.SpawnerProps.forcedFaction == null ? this.parent.Faction : FactionUtility.DefaultFactionFrom(this.SpawnerProps.forcedFaction);
        }
    }
}
