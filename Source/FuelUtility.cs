// Decompiled with JetBrains decompiler
// Type: Androids.FuelUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace Androids
{
  public static class FuelUtility
  {
    public static readonly float autoRefillThreshhold = 0.8f;

    public static Thing FindSuitableFuelForPawn(Pawn pawn, EnergySource_Fueled fuelEnergySourceComp) => GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.OnCell, TraverseParms.For(pawn), validator: ((Predicate<Thing>) (thing => FuelUtility.ValidFuelSource(fuelEnergySourceComp, thing))));

    public static bool ValidFuelSource(EnergySource_Fueled fuelEnergySourceComp, Thing checkThing) => fuelEnergySourceComp.EnergyProps.fuels.Any<ThingOrderRequest>((Predicate<ThingOrderRequest>) (fuelType => fuelType.thingDef == checkThing.def));

    public static Thing FueledEnergySourceNeedRefilling(Pawn pawn)
    {
      Apparel apparel = new Apparel();
      int num;
      if (pawn.apparel != null)
      {
        apparel = pawn.apparel.WornApparel.FirstOrDefault<Apparel>((Predicate<Apparel>) (ap =>
        {
          EnergySource_Fueled comp = ap.TryGetComp<EnergySource_Fueled>();
          return comp != null && (double) comp.MissingFuelPercentage > (double) FuelUtility.autoRefillThreshhold;
        }));
        num = apparel != null ? 1 : 0;
      }
      else
        num = 0;
      return num != 0 ? (Thing) apparel : (Thing) null;
    }
  }
}
