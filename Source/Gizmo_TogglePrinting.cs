// Decompiled with JetBrains decompiler
// Type: Androids.Gizmo_TogglePrinting
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using UnityEngine;
using Verse;

namespace Androids
{
  [StaticConstructorOnStartup]
  public class Gizmo_TogglePrinting : Command
  {
    public static Texture2D startIcon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject");
    public static Texture2D stopIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");
    public IPawnCrafter printer;
    public string labelStart = "AndroidGizmoTogglePrintingStartLabel";
    public string descriptionStart = "AndroidGizmoTogglePrintingStartDescription";
    public string labelStop = "AndroidGizmoTogglePrintingStopLabel";
    public string descriptionStop = "AndroidGizmoTogglePrintingStopDescription";

    public Gizmo_TogglePrinting(IPawnCrafter printer)
    {
      this.printer = printer;
      if (printer.PawnCrafterStatus() == CrafterStatus.Idle)
      {
        this.defaultLabel = (string) this.labelStart.Translate();
        this.defaultDesc = (string) this.descriptionStart.Translate();
        this.icon = (Texture) Gizmo_TogglePrinting.startIcon;
      }
      else
      {
        if (printer.PawnCrafterStatus() != CrafterStatus.Crafting && printer.PawnCrafterStatus() != CrafterStatus.Filling)
          return;
        this.defaultLabel = (string) this.labelStop.Translate();
        this.defaultDesc = (string) this.descriptionStop.Translate();
        this.icon = (Texture) Gizmo_TogglePrinting.stopIcon;
      }
    }

    public override void ProcessInput(Event ev)
    {
      base.ProcessInput(ev);
      if (this.printer.PawnCrafterStatus() == CrafterStatus.Idle)
        this.printer.InitiatePawnCrafting();
      else
        this.printer.StopPawnCrafting();
    }
  }
}
