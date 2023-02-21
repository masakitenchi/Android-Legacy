// Decompiled with JetBrains decompiler
// Type: Androids.DroidCraftingDef
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class DroidCraftingDef : Def
  {
    public List<ThingOrderRequest> costList = new List<ThingOrderRequest>();
    public int timeCost;
    public PawnKindDef pawnKind;
    public bool useDroidCreator = true;
    public int orderID;
    public ResearchProjectDef requiredResearch;
  }
}
