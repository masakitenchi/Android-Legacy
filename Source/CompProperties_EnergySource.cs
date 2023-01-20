// Decompiled with JetBrains decompiler
// Type: Androids.CompProperties_EnergySource
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class CompProperties_EnergySource : CompProperties
  {
    public bool isConsumable = false;
    public float energyWhenConsumed = 0.0f;
    public float passiveEnergyGeneration = 0.0f;
    public List<ThingOrderRequest> fuels = new List<ThingOrderRequest>();
    public float maxFuelAmount = 75f;
    public double fuelAmountUsedPerInterval = 0.001;
    public float activeEnergyGeneration = 0.0f;
    public JobDef refillJob;

    public CompProperties_EnergySource() => this.compClass = typeof (EnergySourceComp);
  }
}
