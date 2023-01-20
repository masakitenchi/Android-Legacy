// Decompiled with JetBrains decompiler
// Type: Androids.ThingOrderProcessor
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class ThingOrderProcessor : IExposable
  {
    public ThingOwner thingHolder;
    public StorageSettings storageSettings;
    public List<ThingOrderRequest> requestedItems = new List<ThingOrderRequest>();

    public ThingOrderProcessor()
    {
    }

    public ThingOrderProcessor(ThingOwner thingHolder, StorageSettings storageSettings)
    {
      this.thingHolder = thingHolder;
      this.storageSettings = storageSettings;
    }

    public IEnumerable<ThingOrderRequest> PendingRequests()
    {
      foreach (ThingOrderRequest idealRequest in this.requestedItems)
      {
        if (idealRequest.nutrition)
        {
          float totalNutrition = this.CountNutrition();
          if ((double) totalNutrition < (double) idealRequest.amount)
          {
            ThingOrderRequest request = new ThingOrderRequest();
            request.nutrition = true;
            request.amount = idealRequest.amount - totalNutrition;
            request.thingFilter = this.storageSettings.filter;
            yield return request;
            request = (ThingOrderRequest) null;
          }
        }
        else
        {
          float totalItemCount = (float) this.thingHolder.TotalStackCountOfDef(idealRequest.thingDef);
          if ((double) totalItemCount < (double) idealRequest.amount)
          {
            ThingOrderRequest request = new ThingOrderRequest();
            request.thingDef = idealRequest.thingDef;
            request.amount = idealRequest.amount - totalItemCount;
            yield return request;
            request = (ThingOrderRequest) null;
          }
        }
      }
    }

    public float CountNutrition()
    {
      float num1 = 0.0f;
      foreach (Thing thing in (IEnumerable<Thing>) this.thingHolder)
      {
        if (thing is Corpse corpse)
          num1 += FoodUtility.GetBodyPartNutrition(corpse, corpse.InnerPawn.RaceProps.body.corePart);
        else if (thing.def.IsIngestible)
        {
          double num2 = (double) num1;
          ThingDef def = thing.def;
          double num3 = (def != null ? (double) def.ingestible.CachedNutrition : 0.05000000074505806) * (double) thing.stackCount;
          num1 = (float) (num2 + num3);
        }
      }
      return num1;
    }

    public void ExposeData() => Scribe_Collections.Look<ThingOrderRequest>(ref this.requestedItems, "requestedItems", LookMode.Deep);
  }
}
