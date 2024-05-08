// Decompiled with JetBrains decompiler
// Type: Androids.EnergyTrackerComp
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
            EnergyTrackerComp energyTrackerComp = this;
            if (Prefs.DevMode && DebugSettings.godMode)
            {
                Command_Action commandAction1 = new Command_Action();
                commandAction1.defaultLabel = "DEBUG: Set Energy to 100%";
                commandAction1.action = () => this.energyNeed.CurLevelPercentage = 1f;
                yield return commandAction1;
                Command_Action commandAction2 = new Command_Action();
                commandAction2.defaultLabel = "DEBUG: Set Energy to 50%";
                commandAction2.action = () => this.energyNeed.CurLevelPercentage = 0.5f;
                yield return commandAction2;
                Command_Action commandAction3 = new Command_Action();
                commandAction3.defaultLabel = "DEBUG: Set Energy to 20%";
                commandAction3.action = () => this.energyNeed.CurLevelPercentage = 0.2f;
                yield return commandAction3;
            }
            Pawn pawn = energyTrackerComp.parent as Pawn;
            if (AndroidsModSettings.Instance.androidExplodesOnDeath && pawn != null && pawn.IsColonistPlayerControlled && pawn.def.HasModExtension<MechanicalPawnProperties>())
            {
                Command_Action commandAction = new Command_Action();
                commandAction.defaultLabel = (string)"AndroidGizmoSelfDetonationLabel".Translate();
                commandAction.defaultDesc = (string)"AndroidGizmoSelfDetonationDescription".Translate();
                commandAction.icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate");
                commandAction.action = () =>
                {
                    if (AndroidsModSettings.Instance.droidDetonationConfirmation)
                        Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("AndroidSelfDetonationConfirmationDialogText".Translate((NamedArgument)pawn.Name.ToStringFull), () => HealthUtility.AdjustSeverity(pawn, HediffDefOf.ChjOverheating, 1.1f), true, (string)"AndroidGizmoSelfDetonationLabel".Translate()));
                    else
                        HealthUtility.AdjustSeverity(pawn, HediffDefOf.ChjOverheating, 1.1f);
                };
                yield return commandAction;
            }
        }
    }
}
