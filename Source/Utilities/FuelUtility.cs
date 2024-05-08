// Decompiled with JetBrains decompiler
// Type: Androids.FuelUtility
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
    public static class FuelUtility
    {
        public static readonly float autoRefillThreshhold = 0.8f;

        public static Thing FindSuitableFuelForPawn(Pawn pawn, EnergySource_Fueled fuelEnergySourceComp) => GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.OnCell, TraverseParms.For(pawn), validator: thing => ValidFuelSource(fuelEnergySourceComp, thing));

        public static bool ValidFuelSource(EnergySource_Fueled fuelEnergySourceComp, Thing checkThing) => fuelEnergySourceComp.EnergyProps.fuels.Any<ThingOrderRequest>(fuelType => fuelType.thingDef == checkThing.def);

        public static Thing FueledEnergySourceNeedRefilling(Pawn pawn)
        {
            if (pawn.apparel != null)
            {
                Apparel apparel = pawn.apparel.WornApparel.FirstOrDefault<Apparel>((Func<Apparel, bool>)(ap =>
                {
                    EnergySource_Fueled comp = ap.TryGetComp<EnergySource_Fueled>();
                    return comp != null && (double)comp.MissingFuelPercentage > autoRefillThreshhold;
                }));
                if (apparel != null)
                    return apparel;
            }
            return null;
        }
    }
}
