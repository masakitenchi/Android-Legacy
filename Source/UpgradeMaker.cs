// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeMaker
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
