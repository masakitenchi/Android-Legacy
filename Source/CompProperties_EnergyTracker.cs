// Decompiled with JetBrains decompiler
// Type: Androids.CompProperties_EnergyTracker
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using Verse;

namespace Androids
{
  public class CompProperties_EnergyTracker : CompProperties
  {
    public bool canHibernate = true;
    public float maxEnergy = 1f;
    public float drainRateModifier = 1f;
    public float powerNetDrainRate = 1.32f;
    public int ticksSpentCharging = 300;
    public JobDef hibernationJob;

    public CompProperties_EnergyTracker() => this.compClass = typeof (EnergyTrackerComp);
  }
}
