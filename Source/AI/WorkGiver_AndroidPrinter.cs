// Decompiled with JetBrains decompiler
// Type: Androids.WorkGiver_AndroidPrinter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Androids
{
    public class WorkGiver_AndroidPrinter : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(ThingDefOf.ChJAndroidPrinter);

        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!(t is Building_AndroidPrinter androidPrinter) || androidPrinter.printerStatus != CrafterStatus.Filling || t.IsForbidden(pawn) || !pawn.CanReserveAndReach((LocalTargetInfo)t, PathEndMode.Touch, pawn.NormalMaxDanger(), ignoreOtherReservations: forced) || pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
                return false;
            IEnumerable<ThingOrderRequest> thingOrderRequests = androidPrinter.orderProcessor.PendingRequests(androidPrinter.GetDirectlyHeldThings());
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

        public override Job JobOnThing(Pawn pawn, Thing printerThing, bool forced = false)
        {
            Building_AndroidPrinter androidPrinter = printerThing as Building_AndroidPrinter;
            IEnumerable<ThingOrderRequest> thingOrderRequests = androidPrinter.orderProcessor.PendingRequests(androidPrinter.GetDirectlyHeldThings());
            if (thingOrderRequests != null)
            {
                foreach (ThingOrderRequest request in thingOrderRequests)
                {
                    Thing ingredient = this.FindIngredient(pawn, androidPrinter, request);
                    if (ingredient != null)
                    {
                        if (request.nutrition)
                        {
                            int num = (int)Math.Ceiling(request.amount / (double)ingredient.def.ingestible.CachedNutrition);
                            if (num > 0)
                                return new Job(JobDefOf.ChJFillAndroidPrinter, (LocalTargetInfo)ingredient, (LocalTargetInfo)printerThing)
                                {
                                    count = num
                                };
                        }
                        else
                            return new Job(JobDefOf.ChJFillAndroidPrinter, (LocalTargetInfo)ingredient, (LocalTargetInfo)printerThing)
                            {
                                count = (int)request.amount
                            };
                    }
                }
            }
            return null;
        }

        private Thing FindIngredient(
          Pawn pawn,
          Building_AndroidPrinter androidPrinter,
          ThingOrderRequest request)
        {
            if (request == null)
                return null;
            Predicate<Thing> extraPredicate = request.ExtraPredicate();
            Predicate<Thing> validator = x => !x.IsForbidden(pawn) && pawn.CanReserve((LocalTargetInfo)x) && extraPredicate(x);
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, request.Request(), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: validator);
        }
    }
}
