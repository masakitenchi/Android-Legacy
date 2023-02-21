// Decompiled with JetBrains decompiler
// Type: Androids.CompProperties_SpawnPawn
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
