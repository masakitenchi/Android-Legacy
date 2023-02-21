// Decompiled with JetBrains decompiler
// Type: Androids.WorkGiver_GiveEnergySourceConsumableToPatient
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace Androids
{
  public class WorkGiver_GiveEnergySourceConsumableToPatient : WorkGiver_Scanner
  {
    public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);

    public override PathEndMode PathEndMode => PathEndMode.Touch;

    public override bool HasJobOnThing(Pawn pawn, Thing thing, bool forced = false)
    {
      if (pawn.Downed || (thing.IsForbidden(pawn) || !thing.Position.InAllowedArea(pawn)) && !pawn.CanReach(new LocalTargetInfo(thing), PathEndMode.ClosestTouch, Danger.Deadly) || HealthAIUtility.ShouldSeekMedicalRest(pawn) || !(thing is Pawn pawn1) || !pawn.CanReserve(new LocalTargetInfo((Thing) pawn1)))
        return false;
      bool? nullable;
      if (pawn1 == null)
      {
        nullable = new bool?();
      }
      else
      {
        Faction faction = pawn1.Faction;
        nullable = faction != null ? new bool?(!faction.IsPlayer) : new bool?();
      }
      if ((nullable ?? true) != false || !pawn1.Downed || !HealthAIUtility.ShouldSeekMedicalRest(pawn1))
        return false;
      Need_Energy need = pawn1.needs.TryGetNeed<Need_Energy>();
      if (need == null || !forced && (double) need.CurLevelPercentage > 0.5)
        return false;
      Thing bestEnergySource = this.TryFindBestEnergySource(pawn);
      return bestEnergySource != null && (!bestEnergySource.Spawned || pawn.CanReserve(new LocalTargetInfo(bestEnergySource)));
    }

    public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
    {
      Pawn pawn1 = thing as Pawn;
      Thing bestEnergySource = this.TryFindBestEnergySource(pawn);
      if (bestEnergySource != null)
      {
        Need_Energy need = pawn1.needs.TryGetNeed<Need_Energy>();
        EnergySourceComp comp = bestEnergySource.TryGetComp<EnergySourceComp>();
        int num = Math.Min((int) Math.Ceiling(((double) need.MaxLevel - (double) need.CurLevel) / (double) comp.EnergyProps.energyWhenConsumed), bestEnergySource.stackCount);
        if (num > 0)
          return new Job(JobDefOf.ChJAndroidRechargeEnergyComp, new LocalTargetInfo(bestEnergySource), new LocalTargetInfo((Thing) pawn1))
          {
            count = num
          };
      }
      return (Job) null;
    }

    public Thing TryFindBestEnergySource(Pawn pawn)
    {
      Pawn_CarryTracker carryTracker = pawn.carryTracker;
      if (carryTracker != null)
      {
        Thing carriedThing = carryTracker.CarriedThing;
        if (carriedThing != null)
        {
          EnergySourceComp comp = carriedThing.TryGetComp<EnergySourceComp>();
          if (comp != null && comp.EnergyProps.isConsumable && carriedThing.stackCount > 0)
            return carryTracker.CarriedThing;
        }
      }
      Pawn_InventoryTracker inventory = pawn.inventory;
      if (inventory != null)
      {
        Thing bestEnergySource = inventory.innerContainer.FirstOrDefault<Thing>((Func<Thing, bool>) (thing =>
        {
          EnergySourceComp comp = thing.TryGetComp<EnergySourceComp>();
          return comp != null && comp.EnergyProps.isConsumable;
        }));
        if (bestEnergySource != null)
          return bestEnergySource;
      }
      return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: ((Predicate<Thing>) (searchThing => searchThing.TryGetComp<EnergySourceComp>() != null && !searchThing.IsForbidden(pawn) && pawn.CanReserve((LocalTargetInfo) searchThing) && searchThing.Position.InAllowedArea(pawn) && pawn.CanReach(new LocalTargetInfo(searchThing), PathEndMode.OnCell, Danger.Deadly))));
    }
  }
}
