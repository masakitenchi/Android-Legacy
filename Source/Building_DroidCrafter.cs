// Decompiled with JetBrains decompiler
// Type: Androids.Building_DroidCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Androids
{
  public class Building_DroidCrafter : Building_PawnCrafter
  {
    private Sustainer soundSustainer;
    public DroidCraftingDef lastDef;
    public bool repeatLastPawn = false;

    public override void InitiatePawnCrafting()
    {
      List<FloatMenuOption> options = new List<FloatMenuOption>();
      foreach (DroidCraftingDef droidCraftingDef in (IEnumerable<DroidCraftingDef>) DefDatabase<DroidCraftingDef>.AllDefs.OrderBy<DroidCraftingDef, int>((Func<DroidCraftingDef, int>) (def => def.orderID)))
      {
        DroidCraftingDef def = droidCraftingDef;
        bool disabled = false;
        if (def.requiredResearch != null && !def.requiredResearch.IsFinished)
          disabled = true;
        options.Add(new FloatMenuOption(!disabled ? (string) "AndroidDroidCrafterPawnMake".Translate((NamedArgument) def.label) : (string) "AndroidDroidCrafterPawnNeedResearch".Translate((NamedArgument) def.label, (NamedArgument) def.requiredResearch.LabelCap), (Action) (() =>
        {
          if (disabled)
            return;
          this.lastDef = def;
          this.MakePawnAndInitCrafting(def);
        }))
        {
          Disabled = disabled
        });
      }
      if (options.Count <= 0)
        return;
      Find.WindowStack.Add((Window) new FloatMenu(options));
    }

    public void MakePawnAndInitCrafting(DroidCraftingDef def)
    {
      this.orderProcessor.requestedItems.Clear();
      foreach (ThingOrderRequest cost in def.costList)
        this.orderProcessor.requestedItems.Add(new ThingOrderRequest()
        {
          nutrition = cost.nutrition,
          thingDef = cost.thingDef,
          amount = cost.amount
        });
      this.craftingTime = def.timeCost;
      if (def.useDroidCreator)
        this.pawnBeingCrafted = DroidUtility.MakeDroidTemplate(def.pawnKind, this.Faction, this.Map.Tile);
      else
        this.pawnBeingCrafted = PawnGenerator.GeneratePawn(def.pawnKind, this.Faction);
      this.crafterStatus = CrafterStatus.Filling;
    }

    public override void ExtraCrafterTickAction()
    {
      if (!this.powerComp.PowerOn && this.soundSustainer != null && !this.soundSustainer.Ended)
        this.soundSustainer.End();
      switch (this.crafterStatus)
      {
        case CrafterStatus.Filling:
          if (!this.powerComp.PowerOn || Current.Game.tickManager.TicksGame % 300 != 0)
            break;
          FleckMaker.ThrowSmoke(this.Position.ToVector3(), this.Map, 1f);
          break;
        case CrafterStatus.Crafting:
          if (this.powerComp.PowerOn && Current.Game.tickManager.TicksGame % 100 == 0)
          {
            for (int index = 0; index < 5; ++index)
              FleckMaker.ThrowMicroSparks(this.Position.ToVector3() + new Vector3((float) Rand.Range(-1, 1), 0.0f, (float) Rand.Range(-1, 1)), this.Map);
            for (int index = 0; index < 3; ++index)
              FleckMaker.ThrowSmoke(this.Position.ToVector3() + new Vector3(Rand.Range(-1f, 1f), 0.0f, Rand.Range(-1f, 1f)), this.Map, Rand.Range(0.5f, 0.75f));
            FleckMaker.ThrowHeatGlow(this.Position, this.Map, 1f);
            if (this.soundSustainer == null || this.soundSustainer.Ended)
            {
              SoundDef craftingSound = this.printerProperties.craftingSound;
              if (craftingSound != null && craftingSound.sustain)
              {
                SoundInfo info = SoundInfo.InMap((TargetInfo) (Thing) this, MaintenanceType.PerTick);
                this.soundSustainer = craftingSound.TrySpawnSustainer(info);
              }
            }
          }
          if (this.soundSustainer == null || this.soundSustainer.Ended)
            break;
          this.soundSustainer.Maintain();
          break;
        default:
          if (this.soundSustainer == null || this.soundSustainer.Ended)
            break;
          this.soundSustainer.End();
          break;
      }
    }

    public override void FinishAction()
    {
      this.orderProcessor.requestedItems.Clear();
      if (!this.repeatLastPawn || this.lastDef == null)
        return;
      this.MakePawnAndInitCrafting(this.lastDef);
    }

    public override void ExposeData()
    {
      base.ExposeData();
      Scribe_Deep.Look<ThingOrderProcessor>(ref this.orderProcessor, "orderProcessor", (object) this.ingredients, (object) this.inputSettings);
      Scribe_Defs.Look<DroidCraftingDef>(ref this.lastDef, "lastDef");
      Scribe_Values.Look<bool>(ref this.repeatLastPawn, "repeatLastPawn");
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
      base.SpawnSetup(map, respawningAfterLoad);
      if (respawningAfterLoad)
        return;
      this.orderProcessor = new ThingOrderProcessor((ThingOwner) this.ingredients, this.inputSettings);
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
      foreach (Gizmo gizmo in base.GetGizmos())
        yield return gizmo;
      Command_Toggle commandToggle = new Command_Toggle();
      commandToggle.defaultLabel = (string) "AndroidGizmoRepeatPawnCraftingLabel".Translate();
      commandToggle.defaultDesc = (string) "AndroidGizmoRepeatPawnCraftingDescription".Translate();
      commandToggle.icon = (Texture) ContentFinder<Texture2D>.Get("ui/designators/PlanOn");
      commandToggle.isActive = (Func<bool>) (() => this.repeatLastPawn);
      commandToggle.toggleAction = (Action) (() => this.repeatLastPawn = !this.repeatLastPawn);
      yield return (Gizmo) commandToggle;
    }
  }
}
