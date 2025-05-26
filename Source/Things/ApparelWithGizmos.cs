// Decompiled with JetBrains decompiler
// Type: Androids.ApparelWithGizmos
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    public class ApparelWithGizmos : Apparel
    {
        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            foreach (ThingComp allComp in this.AllComps)
            {
                foreach (Gizmo gizmo in allComp.CompGetGizmosExtra())
                    yield return gizmo;
            }
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            ApparelWithGizmos apparelWithGizmos = this;
            foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
                yield return statDrawEntry;
            foreach (ThingComp allComp in apparelWithGizmos.AllComps)
            {
                if (allComp is IExtraDisplayStats extraDisplayStats)
                {
                    foreach (StatDrawEntry specialDisplayStat in extraDisplayStats.SpecialDisplayStats())
                        yield return specialDisplayStat;
                }
            }
        }
    }
}
