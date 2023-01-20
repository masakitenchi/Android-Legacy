// Decompiled with JetBrains decompiler
// Type: Androids.ExtraRendering
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using UnityEngine;
using Verse;

namespace Androids
{
  public static class ExtraRendering
  {
    public static void DrawAdvanced(
      this Graphic graphic,
      Vector3 loc,
      Rot4 rot,
      float rotY,
      ThingDef thingDef,
      Thing thing)
    {
      Mesh mesh = graphic.MeshAt(rot);
      Quaternion rotation = graphic.GetQuatFromRot(rot) * Quaternion.AngleAxis(rotY, Vector3.up);
      Material material = graphic.MatAt(rot, thing);
      Graphics.DrawMesh(mesh, loc, rotation, material, 0);
      if (graphic.ShadowGraphic == null)
        ;
    }

    public static Quaternion GetQuatFromRot(this Graphic graphic, Rot4 rot) => graphic.data != null && !graphic.data.drawRotated || !graphic.ShouldDrawRotated ? Quaternion.identity : rot.AsQuat;
  }
}
