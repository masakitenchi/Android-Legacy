// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeCommand_Skin
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using AlienRace;
using RimWorld;
using UnityEngine;
using Verse;

namespace Androids
{
  public class UpgradeCommand_Skin : UpgradeCommand_Hediff
  {
    public Color originalSkinColor;
    public Color originalSkinColorTwo;

    public override void Apply(Pawn customTarget = null)
    {
      base.Apply(customTarget);
      Pawn pawn = customTarget == null ? this.customizationWindow.newAndroid : customTarget;
      AlienPartGenerator.AlienComp comp = pawn.TryGetComp<AlienPartGenerator.AlienComp>();
      if (comp != null)
      {
        this.originalSkinColor = comp.ColorChannels["skin"].first;
        this.originalSkinColorTwo = comp.ColorChannels["skin"].second;
        comp.ColorChannels["skin"].first = this.def.newSkinColor;
        comp.ColorChannels["skin"].second = this.def.newSkinColor;
        if (this.customizationWindow != null)
        {
          this.customizationWindow.refreshAndroidPortrait = true;
        }
        else
        {
          PortraitsCache.SetDirty(pawn);
          PortraitsCache.PortraitsCacheUpdate();
        }
      }
      else
        Log.Error("alienComp is null! Impossible to alter skin color without it.");
    }

    public override void Undo()
    {
      base.Undo();
      AlienPartGenerator.AlienComp comp = this.customizationWindow.newAndroid.TryGetComp<AlienPartGenerator.AlienComp>();
      if (comp == null)
        return;
      comp.ColorChannels["skin"].first = this.originalSkinColor;
      comp.ColorChannels["skin"].second = this.originalSkinColorTwo;
      this.customizationWindow.refreshAndroidPortrait = true;
    }
  }
}
