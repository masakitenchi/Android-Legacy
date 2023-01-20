// Decompiled with JetBrains decompiler
// Type: Androids.ApparelWithGizmos
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class ApparelWithGizmos : Apparel
  {
    public override IEnumerable<Gizmo> GetWornGizmos()
    {
      foreach (ThingComp comp in this.AllComps)
      {
        foreach (Gizmo gizmo in comp.CompGetGizmosExtra())
          yield return gizmo;
      }
    }

    public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
    {
      foreach (StatDrawEntry entry in base.SpecialDisplayStats())
        yield return entry;
      foreach (ThingComp comp in this.AllComps)
      {
        if (comp is IExtraDisplayStats displayStat)
        {
          foreach (StatDrawEntry entry in displayStat.SpecialDisplayStats())
            yield return entry;
        }
        displayStat = (IExtraDisplayStats) null;
      }
    }
  }
}
