// Decompiled with JetBrains decompiler
// Type: Androids.DroidCraftingDef
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class DroidCraftingDef : Def
  {
    public List<ThingOrderRequest> costList = new List<ThingOrderRequest>();
    public int timeCost = 0;
    public PawnKindDef pawnKind;
    public bool useDroidCreator = true;
    public int orderID = 0;
    public ResearchProjectDef requiredResearch;
  }
}
