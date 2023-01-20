// Decompiled with JetBrains decompiler
// Type: Androids.Building_CustomDroidCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

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
