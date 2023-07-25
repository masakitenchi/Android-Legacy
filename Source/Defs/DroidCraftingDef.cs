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
        /// <summary>
        /// The cost to manufacture one Droid.
        /// </summary>
        public List<ThingOrderRequest> costList = new List<ThingOrderRequest>();

        /// <summary>
        /// The time it takes to manufacture one Droid.
        /// </summary>
        public int timeCost = 0;

        /// <summary>
        /// The Droid kind to spawn upon construction.
        /// </summary>
        public PawnKindDef pawnKind;

        /// <summary>
        /// Whether to use the Utility way of creating the Droid or not.
        /// </summary>
        public bool useDroidCreator = true;

        /// <summary>
        /// In what order to display this in menus.
        /// </summary>
        public int orderID = 0;

        /// <summary>
        /// Research required in order for it to be craftable.
        /// </summary>
        public ResearchProjectDef requiredResearch;
    }
}
