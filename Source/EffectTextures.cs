// Decompiled with JetBrains decompiler
// Type: Androids.EffectTextures
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Androids
{
  [StaticConstructorOnStartup]
  public static class EffectTextures
  {
    public static string Eyeglow_Front_Path = "Effects/Eyeglow_front";
    public static string Eyeglow_Side_Path = "Effects/Eyeglow_side";
    public static Dictionary<Pair<bool, Color>, Graphic> eyeCache = new Dictionary<Pair<bool, Color>, Graphic>();

    public static Graphic GetEyeGraphic(bool isFront, Color color)
    {
      Pair<bool, Color> key = new Pair<bool, Color>(isFront, color);
      if (EffectTextures.eyeCache.ContainsKey(key))
        return EffectTextures.eyeCache[key];
      EffectTextures.eyeCache[key] = !isFront ? GraphicDatabase.Get<Graphic_Single>(EffectTextures.Eyeglow_Side_Path, ShaderDatabase.MoteGlow, Vector2.one, color) : GraphicDatabase.Get<Graphic_Single>(EffectTextures.Eyeglow_Front_Path, ShaderDatabase.MoteGlow, Vector2.one, color);
      return EffectTextures.eyeCache[key];
    }
  }
}
