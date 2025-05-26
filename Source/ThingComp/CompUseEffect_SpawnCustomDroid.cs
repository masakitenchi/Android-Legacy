// Decompiled with JetBrains decompiler
// Type: Androids.CompUseEffect_SpawnCustomDroid
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    public class CompUseEffect_SpawnCustomDroid : CompUseEffect_SpawnPawn
    {
        public override void DoSpawn(Pawn usedBy)
        {
            this.GetFaction();
            Pawn pawn = DroidUtility.MakeCustomDroid(this.SpawnerProps.pawnKind, usedBy.Faction);
            if (pawn == null)
                return;
            GenPlace.TryPlaceThing(pawn, this.parent.Position, this.parent.Map, ThingPlaceMode.Near);
            if (!this.SpawnerProps.sendMessage)
                return;
            Messages.Message((string)this.SpawnerProps.pawnSpawnedStringKey.Translate((NamedArgument)pawn.Name.ToStringFull, (NamedArgument)usedBy.Name.ToStringFull), (LookTargets)new GlobalTargetInfo(pawn), MessageTypeDefOf.NeutralEvent);
        }
    }
}
