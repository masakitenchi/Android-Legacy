// Decompiled with JetBrains decompiler
// Type: Androids.ThingOrderProcessor
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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

        public IEnumerable<ThingOrderRequest> PendingRequests(ThingOwner things)
        {
            this.thingHolder = things;
            foreach (ThingOrderRequest requestedItem in this.requestedItems)
            {
                if (requestedItem.nutrition)
                {
                    float num = this.CountNutrition(things);
                    if ((double)num < requestedItem.amount)
                        yield return new ThingOrderRequest()
                        {
                            nutrition = true,
                            amount = requestedItem.amount - num,
                            thingFilter = this.storageSettings.filter
                        };
                }
                else
                {
                    float num = this.thingHolder.TotalStackCountOfDef(requestedItem.thingDef);
                    if ((double)num < requestedItem.amount)
                        yield return new ThingOrderRequest()
                        {
                            thingDef = requestedItem.thingDef,
                            amount = requestedItem.amount - num
                        };
                }
            }
        }

        public float CountNutrition(ThingOwner things)
        {
            this.thingHolder = things;
            float num1 = 0.0f;
            foreach (Thing thing in thingHolder)
            {
                if (thing is Corpse corpse)
                    num1 += FoodUtility.GetBodyPartNutrition(corpse, corpse.InnerPawn.RaceProps.body.corePart);
                else if (thing.def.IsIngestible)
                {
                    double num2 = (double)num1;
                    ThingDef def = thing.def;
                    double num3 = (def != null ? (double)def.ingestible.CachedNutrition : 0.05000000074505806) * thing.stackCount;
                    num1 = (float)(num2 + num3);
                }
            }
            return num1;
        }

        public void ExposeData() => Scribe_Collections.Look(ref this.requestedItems, "requestedItems", LookMode.Deep);
    }
}
