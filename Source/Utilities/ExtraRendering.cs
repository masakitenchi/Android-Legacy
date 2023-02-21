// Decompiled with JetBrains decompiler
// Type: Androids.ExtraRendering
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
      Quaternion quaternion = graphic.GetQuatFromRot(rot) * Quaternion.AngleAxis(rotY, Vector3.up);
      Material material1 = graphic.MatAt(rot, thing);
      Vector3 position = loc;
      Quaternion rotation = quaternion;
      Material material2 = material1;
      Graphics.DrawMesh(mesh, position, rotation, material2, 0);
      Graphic_Shadow shadowGraphic = graphic.ShadowGraphic;
    }

    public static Quaternion GetQuatFromRot(this Graphic graphic, Rot4 rot) => graphic.data != null && !graphic.data.drawRotated || !graphic.ShouldDrawRotated ? Quaternion.identity : rot.AsQuat;
  }
}
