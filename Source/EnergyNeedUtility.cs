// Decompiled with JetBrains decompiler
// Type: Androids.EnergyNeedUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Androids
{
  public static class EnergyNeedUtility
  {
    public static Thing ClosestPowerSource(Pawn pawn) => GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: ((Predicate<Thing>) (thing => EnergyNeedUtility.BestClosestPowerSource(pawn, thing))));

    public static bool BestClosestPowerSource(Pawn pawn, Thing thing)
    {
      int num;
      if (thing.Faction == pawn.Faction)
      {
        CompPower comp = thing.TryGetComp<CompPower>();
        if (comp != null && comp.PowerNet != null && (double) comp.PowerNet.CurrentStoredEnergy() > 50.0 && !thing.IsForbidden(pawn) && pawn.CanReserve(new LocalTargetInfo(thing)) && thing.Position.InAllowedArea(pawn))
        {
          num = pawn.CanReach(new LocalTargetInfo(thing), PathEndMode.OnCell, Danger.Deadly) ? 1 : 0;
          goto label_4;
        }
      }
      num = 0;
label_4:
      if (num == 0)
        return false;
      Building t = thing as Building;
      IntVec3 position = thing.Position;
      if (position.Walkable(pawn.Map) && position.InAllowedArea(pawn) && pawn.CanReserve(new LocalTargetInfo(position)) && pawn.CanReach((LocalTargetInfo) position, PathEndMode.OnCell, Danger.Deadly))
        return true;
      foreach (IntVec3 intVec3 in (IEnumerable<IntVec3>) GenAdj.CellsAdjacentCardinal((Thing) t).OrderByDescending<IntVec3, float>((Func<IntVec3, float>) (selector => selector.DistanceTo(pawn.Position))))
      {
        if (intVec3.Walkable(pawn.Map) && intVec3.InAllowedArea(pawn) && pawn.CanReserve(new LocalTargetInfo(intVec3)) && pawn.CanReach((LocalTargetInfo) intVec3, PathEndMode.ClosestTouch, Danger.Deadly))
          return true;
      }
      return false;
    }
  }
}
