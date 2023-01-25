// Decompiled with JetBrains decompiler
// Type: Androids.CompUseEffect_SpawnDroid
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Androids
{
  public class CompUseEffect_SpawnDroid : CompUseEffect_SpawnPawn
  {
    public override void DoSpawn(Pawn usedBy)
    {
      Pawn pawn = DroidUtility.MakeDroidTemplate(this.SpawnerProps.pawnKind, this.GetFaction(), this.parent.Map.Tile);
      if (pawn == null)
        return;
      GenPlace.TryPlaceThing((Thing) pawn, this.parent.Position, this.parent.Map, ThingPlaceMode.Near);
      if (!this.SpawnerProps.sendMessage)
        return;
      Messages.Message((string) "AndroidSpawnedDroidMessageText".Translate((NamedArgument) pawn.Name.ToStringFull, (NamedArgument) usedBy.Name.ToStringFull), (LookTargets) new GlobalTargetInfo((Thing) pawn), MessageTypeDefOf.NeutralEvent);
    }
  }
}
