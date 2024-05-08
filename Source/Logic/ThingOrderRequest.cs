// Decompiled with JetBrains decompiler
// Type: Androids.ThingOrderRequest
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System;
using System.Xml;
using Verse;

namespace Androids
{
    public class ThingOrderRequest : IExposable
    {
        public ThingDef thingDef;
        public bool nutrition;
        public ThingFilter thingFilter;
        public float amount;

        public ThingRequest Request()
        {
            if (this.thingDef != null)
                return ThingRequest.ForDef(this.thingDef);
            return this.nutrition ? ThingRequest.ForGroup(ThingRequestGroup.MechCharger) : ThingRequest.ForUndefined();
        }

        public Predicate<Thing> ExtraPredicate()
        {
            if (!this.nutrition)
                return thing => true;
            return this.thingFilter == null ? (thing =>
            {
                ThingDef def = thing.def;
                return (def != null ? (!def.ingestible.IsMeal ? 1 : 0) : 0) != 0 && thing.def.IsNutritionGivingIngestible;
            }) : (thing => this.thingFilter.Allows(thing) && thing.def.IsNutritionGivingIngestible && (!(thing is Corpse t) || !t.IsDessicated()));
        }

        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            if (xmlRoot.ChildNodes.Count != 1)
            {
                Log.Error("Misconfigured ThingOrderRequest: " + xmlRoot.OuterXml);
            }
            else
            {
                if (xmlRoot.Name.ToLower() == "nutrition")
                    this.nutrition = true;
                else
                    DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name);
                this.amount = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
            }
        }

        public void ExposeData()
        {
            Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
            Scribe_Values.Look<bool>(ref this.nutrition, "nutrition");
            Scribe_Values.Look<float>(ref this.amount, "amount");
        }
    }
}
