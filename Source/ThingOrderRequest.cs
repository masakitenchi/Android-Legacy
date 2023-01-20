// Decompiled with JetBrains decompiler
// Type: Androids.ThingOrderRequest
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Xml;
using Verse;

namespace Androids
{
  public class ThingOrderRequest : IExposable
  {
    public ThingDef thingDef;
    public bool nutrition = false;
    public ThingFilter thingFilter = (ThingFilter) null;
    public float amount;

    public ThingRequest Request()
    {
      if (this.thingDef != null)
        return ThingRequest.ForDef(this.thingDef);
      return this.nutrition ? ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree) : ThingRequest.ForUndefined();
    }

    public Predicate<Thing> ExtraPredicate()
    {
      if (!this.nutrition)
        return (Predicate<Thing>) (thing => true);
      return this.thingFilter == null ? (Predicate<Thing>) (thing =>
      {
        ThingDef def = thing.def;
        return (def != null ? (!def.ingestible.IsMeal ? 1 : 0) : 0) != 0 && thing.def.IsNutritionGivingIngestible;
      }) : (Predicate<Thing>) (thing => this.thingFilter.Allows(thing) && thing.def.IsNutritionGivingIngestible && (!(thing is Corpse t) || !t.IsDessicated()));
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
          DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef((object) this, "thingDef", xmlRoot.Name);
        this.amount = (float) ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof (float));
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
