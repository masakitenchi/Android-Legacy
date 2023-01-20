// Decompiled with JetBrains decompiler
// Type: Androids.AndroidUpgradeGroupDef
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Androids
{
  public class AndroidUpgradeGroupDef : Def
  {
    public int orderID = 0;
    [Unsaved(false)]
    private List<AndroidUpgradeDef> intCachedUpgrades;

    public IEnumerable<AndroidUpgradeDef> Upgrades
    {
      get
      {
        if (this.intCachedUpgrades == null)
        {
          this.intCachedUpgrades = new List<AndroidUpgradeDef>();
          foreach (AndroidUpgradeDef allDef in DefDatabase<AndroidUpgradeDef>.AllDefs)
          {
            if (allDef.upgradeGroup == this)
              this.intCachedUpgrades.Add(allDef);
          }
        }
        return (IEnumerable<AndroidUpgradeDef>) this.intCachedUpgrades;
      }
    }

    public float calculateNeededHeight(Rect upgradeSize, float rowWidth)
    {
      int num = (int) Math.Floor((double) rowWidth / (double) upgradeSize.width);
      return upgradeSize.height * (float) Math.Ceiling((double) this.Upgrades.Count<AndroidUpgradeDef>() / (double) num);
    }
  }
}
