// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeCommand
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using UnityEngine;
using Verse;

namespace Androids
{
  public abstract class UpgradeCommand
  {
    public AndroidUpgradeDef def;
    public CustomizeAndroidWindow customizationWindow;

    public abstract void Apply(Pawn customTarget = null);

    public abstract void Undo();

    public virtual void Notify_UpgradeAdded()
    {
    }

    public virtual void ExtraOnGUI(Rect inRect)
    {
    }

    public abstract string GetExplanation();
  }
}
