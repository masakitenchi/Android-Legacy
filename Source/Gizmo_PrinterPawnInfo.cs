// Decompiled with JetBrains decompiler
// Type: Androids.Gizmo_PrinterPawnInfo
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
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
      Vector2 size = new Vector2(width - 20f, width);
      GUI.DrawTexture(new Rect(rect.x, rect.y, size.x, size.y), (Texture) PortraitsCache.Get(this.printer.PawnBeingCrafted(), size, Rot4.South));
      return gizmoResult;
    }

    public override void ProcessInput(Event ev)
    {
      base.ProcessInput(ev);
      Find.WindowStack.Add((Window) new Dialog_InfoCard((Thing) this.printer.PawnBeingCrafted()));
    }
  }
}
