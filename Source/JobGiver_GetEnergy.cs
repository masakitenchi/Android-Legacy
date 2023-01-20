// Decompiled with JetBrains decompiler
// Type: Androids.JobGiver_GetEnergy
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
  public class JobGiver_GetEnergy : ThinkNode_JobGiver
  {
    public override ThinkNode DeepCopy(bool resolve = true) => base.DeepCopy(resolve);

    public override float GetPriority(Pawn pawn)
    {
      Need_Energy need = pawn.needs.TryGetNeed<Need_Energy>();
      return need == null || (double) need.CurLevelPercentage >= (double) Need_Energy.rechargePercentage ? 0.0f : 11.5f;
    }

    protected override Job TryGiveJob(Pawn pawn)
    {
      if (pawn.Downed)
        return (Job) null;
      Need_Energy need = pawn.needs.TryGetNeed<Need_Energy>();
      if (need == null || (double) need.CurLevelPercentage >= (double) Need_Energy.rechargePercentage || Find.TickManager.TicksGame < this.GetLastTryTick(pawn) + 2500)
        return (Job) null;
      this.SetLastTryTick(pawn, Find.TickManager.TicksGame);
      Thing targetA = EnergyNeedUtility.ClosestPowerSource(pawn);
      if (targetA != null)
      {
        Building t = targetA as Building;
        if (targetA != null && t != null && t.PowerComp != null && (double) t.PowerComp.PowerNet.CurrentStoredEnergy() > 50.0)
        {
          IntVec3 position = targetA.Position;
          if (position.Walkable(pawn.Map) && position.InAllowedArea(pawn) && pawn.CanReserve(new LocalTargetInfo(position)) && pawn.CanReach((LocalTargetInfo) position, PathEndMode.OnCell, Danger.Deadly))
            return new Job(JobDefOf.ChJAndroidRecharge, (LocalTargetInfo) targetA);
          foreach (IntVec3 intVec3 in (IEnumerable<IntVec3>) GenAdj.CellsAdjacentCardinal((Thing) t).OrderByDescending<IntVec3, float>((Func<IntVec3, float>) (selector => selector.DistanceTo(pawn.Position))))
          {
            if (intVec3.Walkable(pawn.Map) && intVec3.InAllowedArea(pawn) && pawn.CanReserve(new LocalTargetInfo(intVec3)) && pawn.CanReach((LocalTargetInfo) intVec3, PathEndMode.OnCell, Danger.Deadly))
              return new Job(JobDefOf.ChJAndroidRecharge, (LocalTargetInfo) targetA, (LocalTargetInfo) intVec3);
          }
        }
      }
      Pawn_CarryTracker carryTracker = pawn.carryTracker;
      Thing carriedThing = new Thing();
      int num1;
      if (carryTracker != null)
      {
        carriedThing = carryTracker.CarriedThing;
        if (carriedThing != null)
        {
          EnergySourceComp comp = carriedThing.TryGetComp<EnergySourceComp>();
          if (comp != null)
          {
            num1 = comp.EnergyProps.isConsumable ? 1 : 0;
            goto label_22;
          }
        }
      }
      num1 = 0;
label_22:
      if (num1 != 0 && carriedThing.stackCount > 0)
        return new Job(JobDefOf.ChJAndroidRechargeEnergyComp, new LocalTargetInfo(carriedThing))
        {
          count = carriedThing.stackCount
        };
      Pawn_InventoryTracker inventory = pawn.inventory;
      if (inventory != null && inventory.innerContainer.Any<Thing>((Func<Thing, bool>) (thing =>
      {
        EnergySourceComp comp = thing.TryGetComp<EnergySourceComp>();
        return comp != null && comp.EnergyProps.isConsumable;
      })))
      {
        Thing thing1 = inventory.innerContainer.FirstOrDefault<Thing>((Func<Thing, bool>) (thing =>
        {
          EnergySourceComp comp = thing.TryGetComp<EnergySourceComp>();
          return comp != null && comp.EnergyProps.isConsumable;
        }));
        if (thing1 != null)
        {
          EnergySourceComp comp = thing1.TryGetComp<EnergySourceComp>();
          int num2 = Math.Min((int) Math.Ceiling(((double) need.MaxLevel - (double) need.CurLevel) / (double) comp.EnergyProps.energyWhenConsumed), thing1.stackCount);
          if (num2 > 0)
            return new Job(JobDefOf.ChJAndroidRechargeEnergyComp, new LocalTargetInfo(thing1))
            {
              count = num2
            };
        }
      }
      Thing thing2 = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.OnCell, TraverseParms.For(pawn), validator: ((Predicate<Thing>) (thing => thing.TryGetComp<EnergySourceComp>() != null && !thing.IsForbidden(pawn) && pawn.CanReserve(new LocalTargetInfo(thing)) && thing.Position.InAllowedArea(pawn) && pawn.CanReach(new LocalTargetInfo(thing), PathEndMode.OnCell, Danger.Deadly))));
      if (thing2 != null)
      {
        EnergySourceComp comp = thing2.TryGetComp<EnergySourceComp>();
        if (comp != null)
        {
          int num3 = (int) Math.Ceiling(((double) need.MaxLevel - (double) need.CurLevel) / (double) comp.EnergyProps.energyWhenConsumed);
          if (num3 > 0)
            return new Job(JobDefOf.ChJAndroidRechargeEnergyComp, new LocalTargetInfo(thing2))
            {
              count = num3
            };
        }
      }
      return (Job) null;
    }

    private int GetLastTryTick(Pawn pawn)
    {
      int num;
      return pawn.mindState.thinkData.TryGetValue(this.UniqueSaveKey, out num) ? num : -99999;
    }

    private void SetLastTryTick(Pawn pawn, int val) => pawn.mindState.thinkData[this.UniqueSaveKey] = val;
  }
}
