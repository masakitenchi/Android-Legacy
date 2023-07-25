// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeCommand
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
