// Decompiled with JetBrains decompiler
// Type: Androids.CompProperties_EnergySource
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class CompProperties_EnergySource : CompProperties
  {
    public bool isConsumable;
    public float energyWhenConsumed;
    public float passiveEnergyGeneration;
    public List<ThingOrderRequest> fuels = new List<ThingOrderRequest>();
    public float maxFuelAmount = 75f;
    public double fuelAmountUsedPerInterval = 0.001;
    public float activeEnergyGeneration;
    public JobDef refillJob;

    public CompProperties_EnergySource() => this.compClass = typeof (EnergySourceComp);
  }
}
