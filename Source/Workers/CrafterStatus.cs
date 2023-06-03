// Decompiled with JetBrains decompiler
// Type: Androids.CrafterStatus
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System;

namespace Androids
{
    [Flags]
    public enum CrafterStatus : byte
    {
        Idle = 0,
        Filling = 1,
        Crafting = 2,
        Finished = 4,
    }
}
