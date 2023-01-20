// Decompiled with JetBrains decompiler
// Type: Androids.CompProperties_SpawnPawn
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using Verse;

namespace Androids
{
  public class CompProperties_SpawnPawn : CompProperties_UseEffect
  {
    public PawnKindDef pawnKind;
    public int amount = 1;
    public FactionDef forcedFaction;
    public bool usePlayerFaction = true;
    public string pawnSpawnedStringKey = "AndroidSpawnedDroidMessageText";
    public bool sendMessage = true;

    public CompProperties_SpawnPawn() => this.compClass = typeof (CompUseEffect_SpawnPawn);
  }
}
