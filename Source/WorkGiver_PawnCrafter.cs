// Decompiled with JetBrains decompiler
// Type: Androids.WorkGiver_PawnCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Androids
{
  public class WorkGiver_PawnCrafter : WorkGiver_Scanner
  {
    private PawnCrafterWorkgiverProperties intWorkGiverProperties = (PawnCrafterWorkgiverProperties) null;

    public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(this.WorkGiverProperties.defToScan);

    public override PathEndMode PathEndMode => PathEndMode.Touch;

    public PawnCrafterWorkgiverProperties WorkGiverProperties
    {
      get
      {
        if (this.intWorkGiverProperties == null)
          this.intWorkGiverProperties = this.def.GetModExtension<PawnCrafterWorkgiverProperties>();
        return this.intWorkGiverProperties;
      }
    }

    public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
      if (!(t is Building_PawnCrafter androidPrinter) || androidPrinter.crafterStatus != CrafterStatus.Filling || t.IsForbidden(pawn) || !pawn.CanReserveAndReach((LocalTargetInfo) t, PathEndMode.Touch, pawn.NormalMaxDanger(), ignoreOtherReservations: forced) || pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
        return false;
      IEnumerable<ThingOrderRequest> thingOrderRequests = androidPrinter.orderProcessor.PendingRequests();
      bool flag = false;
      if (thingOrderRequests != null)
      {
        foreach (ThingOrderRequest request in thingOrderRequests)
        {
          if (this.FindIngredient(pawn, androidPrinter, request) != null)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    public override Job JobOnThing(Pawn pawn, Thing crafterThing, bool forced = false)
    {
      Building_PawnCrafter androidPrinter = crafterThing as Building_PawnCrafter;
      IEnumerable<ThingOrderRequest> thingOrderRequests = androidPrinter.orderProcessor.PendingRequests();
      if (thingOrderRequests != null)
      {
        foreach (ThingOrderRequest request in thingOrderRequests)
        {
          Thing ingredient = this.FindIngredient(pawn, androidPrinter, request);
          if (ingredient != null)
          {
            if (request.nutrition)
            {
              int num = (int) Math.Ceiling((double) request.amount / (double) ingredient.def.ingestible.CachedNutrition);
              if (num > 0)
                return new Job(this.WorkGiverProperties.fillJob, (LocalTargetInfo) ingredient, (LocalTargetInfo) crafterThing)
                {
                  count = num
                };
            }
            else
              return new Job(this.WorkGiverProperties.fillJob, (LocalTargetInfo) ingredient, (LocalTargetInfo) crafterThing)
              {
                count = (int) request.amount
              };
          }
        }
      }
      return (Job) null;
    }

    private Thing FindIngredient(
      Pawn pawn,
      Building_PawnCrafter androidPrinter,
      ThingOrderRequest request)
    {
      if (request == null)
        return (Thing) null;
      Predicate<Thing> extraPredicate = request.ExtraPredicate();
      Predicate<Thing> validator = (Predicate<Thing>) (x => !x.IsForbidden(pawn) && pawn.CanReserve((LocalTargetInfo) x) && extraPredicate(x));
      return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, request.Request(), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: validator);
    }
  }
}
