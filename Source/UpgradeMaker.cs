// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeMaker
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System;

namespace Androids
{
  public static class UpgradeMaker
  {
    public static UpgradeCommand Make(
      AndroidUpgradeDef def,
      CustomizeAndroidWindow customizationWindow = null)
    {
      UpgradeCommand instance = (UpgradeCommand) Activator.CreateInstance(def.commandType);
      if (instance != null)
      {
        instance.def = def;
        instance.customizationWindow = customizationWindow;
      }
      return instance;
    }
  }
}
