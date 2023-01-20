// Decompiled with JetBrains decompiler
// Type: Androids.CompUseEffect_SpawnDroid
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

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
      if (this.SpawnerProps.sendMessage)
        Messages.Message((string) "AndroidSpawnedDroidMessageText".Translate((NamedArgument) pawn.Name.ToStringFull, (NamedArgument) usedBy.Name.ToStringFull), (LookTargets) new GlobalTargetInfo((Thing) pawn), MessageTypeDefOf.NeutralEvent);
    }
  }
}
