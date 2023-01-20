// Decompiled with JetBrains decompiler
// Type: Androids.IExtraDisplayStats
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System.Collections.Generic;

namespace Androids
{
  public interface IExtraDisplayStats
  {
    IEnumerable<StatDrawEntry> SpecialDisplayStats();
  }
}
