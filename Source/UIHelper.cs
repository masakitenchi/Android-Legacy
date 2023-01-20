// Decompiled with JetBrains decompiler
// Type: Androids.UIHelper
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using UnityEngine;

namespace Androids
{
  public static class UIHelper
  {
    public static Rect GetRowRect(Rect inRect, float rowHeight, int row)
    {
      float y = inRect.y + rowHeight * (float) row;
      return new Rect(inRect.x, y, inRect.width, rowHeight);
    }
  }
}
