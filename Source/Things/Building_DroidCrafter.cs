﻿// Decompiled with JetBrains decompiler
// Type: Androids.Building_DroidCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Verse.Sound;

namespace Androids
{
    public class Building_DroidCrafter : Building_PawnCrafter
    {
        private Sustainer soundSustainer;
        public DroidCraftingDef lastDef;
        public bool repeatLastPawn;

        public override void InitiatePawnCrafting()
        {
            List<FloatMenuOption> options = new List<FloatMenuOption>();
            foreach (DroidCraftingDef def in (IEnumerable<DroidCraftingDef>)DefDatabase<DroidCraftingDef>.AllDefs.OrderBy(def => def.orderID))
            {
                bool disabled = false;
                if (def.requiredResearch != null && !def.requiredResearch.IsFinished)
                    disabled = true;
                options.Add(new FloatMenuOption(!disabled ? (string)"AndroidDroidCrafterPawnMake".Translate((NamedArgument)def.label) : (string)"AndroidDroidCrafterPawnNeedResearch".Translate((NamedArgument)def.label, (NamedArgument)def.requiredResearch.LabelCap), () =>
                {
                    if (disabled)
                        return;
                    this.lastDef = def;
                    this.MakePawnAndInitCrafting(def);
                })
                {
                    Disabled = disabled
                });
            }
            if (options.Count <= 0)
                return;
            Find.WindowStack.Add(new FloatMenu(options));
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
                        IntVec3 position;
                        for (int index = 0; index < 5; ++index)
                        {
                            position = this.Position;
                            FleckMaker.ThrowMicroSparks(position.ToVector3() + new Vector3(Rand.Range(-1, 1), 0.0f, Rand.Range(-1, 1)), this.Map);
                        }
                        for (int index = 0; index < 3; ++index)
                        {
                            position = this.Position;
                            FleckMaker.ThrowSmoke(position.ToVector3() + new Vector3(Rand.Range(-1f, 1f), 0.0f, Rand.Range(-1f, 1f)), this.Map, Rand.Range(0.5f, 0.75f));
                        }
                        FleckMaker.ThrowHeatGlow(this.Position, this.Map, 1f);
                        if (this.soundSustainer == null || this.soundSustainer.Ended)
                        {
                            SoundDef craftingSound = this.printerProperties.craftingSound;
                            if (craftingSound != null && craftingSound.sustain)
                            {
                                SoundInfo info = SoundInfo.InMap((TargetInfo)this, MaintenanceType.PerTick);
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
            Scribe_Deep.Look(ref this.orderProcessor, "orderProcessor", ingredients, inputSettings);
            Scribe_Defs.Look(ref this.lastDef, "lastDef");
            Scribe_Values.Look(ref this.repeatLastPawn, "repeatLastPawn");
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (respawningAfterLoad)
                return;
            this.orderProcessor = new ThingOrderProcessor(ingredients, this.inputSettings);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
                yield return gizmo;
            yield return new Command_Toggle()
            {
                defaultLabel = "AndroidGizmoRepeatPawnCraftingLabel".Translate(),
                defaultDesc = "AndroidGizmoRepeatPawnCraftingDescription".Translate(),
                icon = ContentFinder<Texture2D>.Get("ui/designators/PlanOn", true),
                isActive = () => repeatLastPawn,
                toggleAction = delegate ()
                {
                    repeatLastPawn = !repeatLastPawn;
                }
            };
        }
    }
}
