// Decompiled with JetBrains decompiler
// Type: Androids.EnergyTrackerComp
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using Androids.Integration;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Androids
{
  public class EnergyTrackerComp : ThingComp
  {
    public float energy;
    private Pawn pawn;
    private Need_Energy energyNeed;

    public CompProperties_EnergyTracker EnergyProperties => this.props as CompProperties_EnergyTracker;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
      this.pawn = this.parent as Pawn;
      if (this.pawn == null)
        return;
      this.energyNeed = this.pawn.needs.TryGetNeed<Need_Energy>();
    }

    public override void CompTick()
    {
      if (this.energyNeed == null)
        return;
      this.energy = this.energyNeed.CurLevel;
    }

    public override void PostExposeData() => Scribe_Values.Look<float>(ref this.energy, "energy");

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
      if (Prefs.DevMode && DebugSettings.godMode)
      {
        Command_Action gizmo1 = new Command_Action();
        gizmo1.defaultLabel = "DEBUG: Set Energy to 100%";
        gizmo1.action = (Action) (() => this.energyNeed.CurLevelPercentage = 1f);
        yield return (Gizmo) gizmo1;
        gizmo1 = (Command_Action) null;
        Command_Action gizmo2 = new Command_Action();
        gizmo2.defaultLabel = "DEBUG: Set Energy to 50%";
        gizmo2.action = (Action) (() => this.energyNeed.CurLevelPercentage = 0.5f);
        yield return (Gizmo) gizmo2;
        gizmo2 = (Command_Action) null;
        Command_Action gizmo3 = new Command_Action();
        gizmo3.defaultLabel = "DEBUG: Set Energy to 20%";
        gizmo3.action = (Action) (() => this.energyNeed.CurLevelPercentage = 0.2f);
        yield return (Gizmo) gizmo3;
        gizmo3 = (Command_Action) null;
      }
      Pawn pawn = this.parent as Pawn;
      if (AndroidsModSettings.Instance.androidExplodesOnDeath && pawn != null && pawn.IsColonistPlayerControlled && pawn.def.HasModExtension<MechanicalPawnProperties>())
      {
        Command_Action gizmo = new Command_Action();
        gizmo.defaultLabel = (string) "AndroidGizmoSelfDetonationLabel".Translate();
        gizmo.defaultDesc = (string) "AndroidGizmoSelfDetonationDescription".Translate();
        gizmo.icon = (Texture) ContentFinder<Texture2D>.Get("UI/Commands/Detonate");
        gizmo.action = (Action) (() =>
        {
          if (AndroidsModSettings.Instance.droidDetonationConfirmation)
            Find.WindowStack.Add((Window) Dialog_MessageBox.CreateConfirmation("AndroidSelfDetonationConfirmationDialogText".Translate((NamedArgument) pawn.Name.ToStringFull), (Action) (() => HealthUtility.AdjustSeverity(pawn, HediffDefOf.ChjOverheating, 1.1f)), true, (string) "AndroidGizmoSelfDetonationLabel".Translate()));
          else
            HealthUtility.AdjustSeverity(pawn, HediffDefOf.ChjOverheating, 1.1f);
        });
        yield return (Gizmo) gizmo;
        gizmo = (Command_Action) null;
      }
    }
  }
}
