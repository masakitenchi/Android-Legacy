// Decompiled with JetBrains decompiler
// Type: Androids.ButcherUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System;
using Verse;

namespace Androids
{
    public static class ButcherUtility
    {
        public static void SpawnDrops(Pawn pawn, IntVec3 position, Map map)
        {
            float missingNaturalParts = pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(pawn.RaceProps.body.corePart);
            foreach (ThingDefCountClass butcherProduct in pawn.def.butcherProducts)
            {
                int val1 = (int)Math.Ceiling(butcherProduct.count * (double)missingNaturalParts);
                if (val1 > 0)
                {
                    do
                    {
                        Thing thing = ThingMaker.MakeThing(butcherProduct.thingDef);
                        thing.stackCount = Math.Min(val1, butcherProduct.thingDef.stackLimit);
                        val1 -= thing.stackCount;
                        GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Near);
                    }
                    while (val1 > 0);
                }
            }
        }
    }
}
