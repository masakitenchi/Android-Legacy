// Decompiled with JetBrains decompiler
// Type: Androids.Gizmo_PrinterPawnInfo
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Androids
{
  [StaticConstructorOnStartup]
  public class Gizmo_PrinterPawnInfo : Command
  {
    public IPawnCrafter printer;
    public static Texture2D emptyIcon = ContentFinder<Texture2D>.Get("UI/Overlays/ThingLine");
    public string description = "AndroidGizmoPrinterAndroidInfoDescription";

    public Gizmo_PrinterPawnInfo(IPawnCrafter printer)
    {
      this.printer = printer;
      this.defaultLabel = printer.PawnBeingCrafted().Name.ToStringFull;
      this.defaultDesc = (string) this.description.Translate();
      this.icon = (Texture) Gizmo_PrinterPawnInfo.emptyIcon;
    }

    public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
    {
      GizmoResult gizmoResult = base.GizmoOnGUI(topLeft, maxWidth, parms);
      float width = this.GetWidth(maxWidth);
      Rect rect = new Rect(topLeft.x + 10f, topLeft.y, width - 40f, width - 20f);
      Vector2 vector2 = new Vector2(width - 20f, width);
      RenderTexture image = PortraitsCache.Get(this.printer.PawnBeingCrafted(), vector2, Rot4.South, new Vector3(), 1f, true, true, true, true, (Dictionary<Apparel, Color>) null, new Color?(), false);
      if (!((Object) image != (Object) null))
        return gizmoResult;
      GUI.DrawTexture(new Rect(rect.x, rect.y, vector2.x, vector2.y), (Texture) image);
      return gizmoResult;
    }

    public override void ProcessInput(Event ev)
    {
      base.ProcessInput(ev);
      Find.WindowStack.Add((Window) new Dialog_InfoCard((Thing) this.printer.PawnBeingCrafted()));
    }
  }
}
