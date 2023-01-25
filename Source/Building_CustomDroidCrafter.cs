// Decompiled with JetBrains decompiler
// Type: Androids.Building_CustomDroidCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
  public class Building_CustomDroidCrafter : Building_DroidCrafter
  {
    public override void InitiatePawnCrafting()
    {
      this.pawnBeingCrafted = DroidUtility.MakeCustomDroid(this.printerProperties.pawnKind, this.Faction);
      this.crafterStatus = CrafterStatus.Filling;
    }
  }
}
