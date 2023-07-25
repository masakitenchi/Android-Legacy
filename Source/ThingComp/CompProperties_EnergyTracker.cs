// Decompiled with JetBrains decompiler
// Type: Androids.CompProperties_EnergyTracker
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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

        public CompProperties_EnergyTracker() => this.compClass = typeof(EnergyTrackerComp);
    }
}
