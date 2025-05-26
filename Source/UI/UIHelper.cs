// Decompiled with JetBrains decompiler
// Type: Androids.UIHelper
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    public static class UIHelper
    {
        public static Rect GetRowRect(Rect inRect, float rowHeight, int row)
        {
            float y = inRect.y + rowHeight * row;
            return new Rect(inRect.x, y, inRect.width, rowHeight);
        }
    }
}
