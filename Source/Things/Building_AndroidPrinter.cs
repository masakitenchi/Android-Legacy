// Decompiled with JetBrains decompiler
// Type: Androids.Building_AndroidPrinter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Androids
{
    [StaticConstructorOnStartup]
    public class Building_AndroidPrinter :
        Building_Casket,
        ISuspendableThingHolder,
        IStoreSettingsParent,
        IPawnCrafter
    {
        #region fields
        /// <summary>
        /// Requested nutrition to print one Android.
        /// </summary>
        public static float requestNutrition = 20f;
        /// <summary>
        /// Requested Plasteel to print one Android.
        /// </summary>
        public static int requestPlasteel = 150;
        /// <summary>
        /// Requested Components to print one Android.
        /// </summary>
        public static int requestComponents = 20;

        public static readonly Texture2D EjectPod = ContentFinder<Texture2D>.Get("UI/Commands/PodEject");

        /// <summary>
        /// Stored ingredients for use in producing one pawn.
        /// </summary>
        public ThingOwner ingredients;
        /// <summary>
        /// Printer state.
        /// </summary>
        public CrafterStatus printerStatus;
        /// <summary>
        /// Pawn to print.
        /// </summary>
		public Pawn pawnToPrint;
        public Pawn clonedPawnToPrint;
        /// <summary>
        /// Class used to store the state of the order processor.
        /// </summary>
		public ThingOrderProcessor orderProcessor;
        /// <summary>
        /// Extra time cost set by the upgrades.
        /// </summary>
		public int extraTimeCost;
        /// <summary>
        /// Storage settings for what nutrition sources to use.
        /// </summary>
		public StorageSettings inputSettings;
        /// <summary>
        /// Sustained sound.
        /// </summary>
		private Sustainer soundSustainer;
        /// <summary>
        /// Power component.
        /// </summary>
		private CompPowerTrader powerComp;
        /// <summary>
        /// Flickable component.
        /// </summary>
		private CompFlickable flickableComp;

        /// <summary>
        /// XML properties for the printer.
        /// </summary>
        protected PawnCrafterProperties printerProperties;
        /// <summary>
        /// Ticks left until pawn is finished printing.
        /// </summary>
		public int printingTicksLeft;
        /// <summary>
        /// Next resource drain trick-
        /// </summary>
		public int nextResourceTick;
        public HashSet<AndroidUpgradeDef> upgradesToApply = new HashSet<AndroidUpgradeDef>();
        /*
		public bool needsSaved;
		public float cachedNeedsFood;
		public float cachedNeedsRest;
		public float cachedNeedsJoy;
		public float cachedNeedsMood;
		public float cachedNeedsComfort;
		*/
        #endregion
        #region Properties
        public bool StorageTabVisible => false;
        public bool IsContentsSuspended => this.printerStatus == CrafterStatus.Crafting;
        public Pawn PawnBeingCrafted => this.pawnToPrint;
        public bool IsUpgrade => this.GetDirectlyHeldThings().Any(x => x is Pawn);
        public CrafterStatus PrinterStatus => this.printerStatus;
        public PawnCrafterProperties PrinterProperties => this.def.GetModExtension<PawnCrafterProperties>();
        public Pawn PawnInside => (Pawn)this.GetDirectlyHeldThings().Where<Thing>((Func<Thing, bool>)(p => p is Pawn)).FirstOrDefault<Thing>();
        public bool ReadyToPrint => this.printerStatus == CrafterStatus.Filling && this.orderProcessor.PendingRequests(this.GetDirectlyHeldThings()) == null;
        #endregion
        public void Notify_SettingsChanged()
        { }
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.ingredients = this.GetDirectlyHeldThings();
            this.powerComp = this.GetComp<CompPowerTrader>();
            this.flickableComp = this.GetComp<CompFlickable>();
            if (this.inputSettings == null)
            {
                this.inputSettings = new StorageSettings((IStoreSettingsParent)this);
                if (this.def.building.defaultStorageSettings != null)
                    this.inputSettings.CopyFrom(this.def.building.defaultStorageSettings);
            }
            this.printerProperties = this.def.GetModExtension<PawnCrafterProperties>();
            if (!respawningAfterLoad)
            {
                if (this.printerProperties == null)
                {
                    this.orderProcessor = new ThingOrderProcessor(this.ingredients, this.inputSettings);
                    this.orderProcessor.requestedItems.Add(new ThingOrderRequest()
                    {
                        nutrition = true,
                        amount = Building_AndroidPrinter.requestNutrition
                    });
                    this.orderProcessor.requestedItems.Add(new ThingOrderRequest()
                    {
                        thingDef = RimWorld.ThingDefOf.Plasteel,
                        amount = (float)Building_AndroidPrinter.requestPlasteel
                    });
                    this.orderProcessor.requestedItems.Add(new ThingOrderRequest()
                    {
                        thingDef = RimWorld.ThingDefOf.ComponentIndustrial,
                        amount = (float)Building_AndroidPrinter.requestComponents
                    });
                }
                else
                    this.orderProcessor = new ThingOrderProcessor(this.ingredients, this.inputSettings);
            }
            this.AdjustPowerNeed();
        }

        public override void PostMake()
        {
            base.PostMake();
            this.inputSettings = new StorageSettings(this);
            if (this.def.building.defaultStorageSettings == null)
                return;
            this.inputSettings.CopyFrom(this.def.building.defaultStorageSettings);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<CrafterStatus>(ref this.printerStatus, "printerStatus");
            Scribe_Values.Look<int>(ref this.printingTicksLeft, "printingTicksLeft");
            Scribe_Values.Look<int>(ref this.nextResourceTick, "nextResourceTick");
            //Has Pawn inside => that pawn must be re-entering since de novo crafting doesn't generate pawn until print finishes
            if (this.GetDirectlyHeldThings().Any(x => x is Pawn))
            {
                Scribe_References.Look(ref this.pawnToPrint, "androidToPrint", true);
                Scribe_References.Look(ref this.clonedPawnToPrint, "clonedPawnToPrint", true);
            }
            else
            {
                Scribe_Deep.Look(ref this.pawnToPrint, "androidToPrint");
                Scribe_Deep.Look(ref this.clonedPawnToPrint, "clonedPawnToPrint");
            }
            Scribe_Deep.Look<StorageSettings>(ref this.inputSettings, "inputSettings");
            Scribe_Deep.Look<ThingOrderProcessor>(ref this.orderProcessor, "orderProcessor", (object)this.ingredients, (object)this.inputSettings);
            Scribe_Values.Look<int>(ref this.extraTimeCost, "extraTimeCost");
            Scribe_Collections.Look(ref this.upgradesToApply, "upgradesToApply", LookMode.Def);
            /*
			Scribe_Values.Look<bool>(ref this.needsSaved, "needsSaved");
			Scribe_Values.Look<float>(ref this.cachedNeedsFood, "cachedNeedsFood");
			Scribe_Values.Look<float>(ref this.cachedNeedsRest, "cachedNeedsRest");
			Scribe_Values.Look<float>(ref this.cachedNeedsJoy, "cachedNeedsJoy");
			Scribe_Values.Look<float>(ref this.cachedNeedsMood, "cachedNeedsMood");
			Scribe_Values.Look<float>(ref this.cachedNeedsComfort, "cachedNeedsComfort");
			*/
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (this.pawnToPrint != null)
                yield return new Gizmo_PrinterPawnInfo(this);
            if (this.printerStatus != CrafterStatus.Finished)
                yield return new Gizmo_TogglePrinting(this);
            if (this.Faction.IsPlayer && this.innerContainer.Count > 0 && def.building.isPlayerEjectable && (printerStatus == CrafterStatus.Idle || printerStatus == CrafterStatus.Finished))
                yield return new Command_Action()
                {
                    action = new Action(this.EjectContents),
                    defaultLabel = "AndroidPrinterEject".Translate(),
                    defaultDesc = "AndroidPrinterEjectDesc".Translate(),
                    hotKey = KeyBindingDefOf.Misc1,
                    icon = EjectPod
                };
            foreach (var gizmo in base.GetGizmos())
                yield return gizmo;
            if (DebugSettings.godMode && this.upgradesToApply.Count > 0 && this.pawnToPrint != null)
            {
                yield return new Command_Action()
                {
                    defaultLabel = "DEBUG: Finish crafting.",
                    defaultDesc = "Finishes crafting the pawn.",
                    action = delegate ()
                    {
                        this.printerStatus = CrafterStatus.Finished;
                    }
                };
            }
        }

        public override string GetInspectString()
        {
            if (this.ParentHolder != null && !(this.ParentHolder is Map))
                return base.GetInspectString();
            StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
            stringBuilder.AppendLine();
            stringBuilder.AppendLine((string)this.printerProperties.crafterStatusText.Translate((NamedArgument)(this.printerProperties.crafterStatusEnumText + ((int)this.printerStatus).ToString()).Translate()));
            if (this.printerStatus == CrafterStatus.Crafting)
                stringBuilder.AppendLine((string)this.printerProperties.crafterProgressText.Translate((NamedArgument)(((float)(this.printerProperties.ticksToCraft + this.extraTimeCost) - (float)this.printingTicksLeft) / (float)(this.printerProperties.ticksToCraft + this.extraTimeCost)).ToStringPercent()));
            if (this.printerStatus == CrafterStatus.Filling)
            {
                bool needsFulfilled = true;
                stringBuilder.Append(this.FormatIngredientCosts(out needsFulfilled, (IEnumerable<ThingOrderRequest>)this.orderProcessor.requestedItems));
                if (!needsFulfilled)
                    stringBuilder.AppendLine();
            }
            this.ingredients = this.GetDirectlyHeldThings();
            if (this.ingredients.Count > 0)
                stringBuilder.Append((string)(this.printerProperties.crafterMaterialsText.Translate() + " "));
            foreach (Thing thing in this.ingredients.Where<Thing>((Func<Thing, bool>)(i => !(i is Pawn))))
                stringBuilder.Append(thing.LabelCap + "; ");
            return stringBuilder.ToString().TrimEndNewlines();
        }

        public string FormatIngredientCosts(
          out bool needsFulfilled,
          IEnumerable<ThingOrderRequest> requestedItems,
          bool deductCosts = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            needsFulfilled = true;
            this.ingredients = this.GetDirectlyHeldThings();
            foreach (ThingOrderRequest requestedItem in requestedItems)
            {
                if (requestedItem.nutrition)
                {
                    float num1 = this.CountNutrition();
                    if (deductCosts)
                    {
                        float num2 = requestedItem.amount - num1;
                        if ((double)num2 > 0.0)
                        {
                            stringBuilder.Append((string)(this.printerProperties.crafterMaterialNeedText.Translate((NamedArgument)num2, (NamedArgument)this.printerProperties.crafterNutritionText.Translate()) + " "));
                            needsFulfilled = false;
                        }
                    }
                    else
                        stringBuilder.Append((string)(this.printerProperties.crafterMaterialNeedText.Translate((NamedArgument)requestedItem.amount, (NamedArgument)this.printerProperties.crafterNutritionText.Translate()) + " "));
                }
                else
                {
                    int num = this.ingredients.TotalStackCountOfDef(requestedItem.thingDef);
                    if (deductCosts)
                    {
                        if ((double)num < (double)requestedItem.amount)
                        {
                            stringBuilder.Append((string)(this.printerProperties.crafterMaterialNeedText.Translate((NamedArgument)(requestedItem.amount - (float)num), (NamedArgument)requestedItem.thingDef.LabelCap) + " "));
                            needsFulfilled = false;
                        }
                    }
                    else
                        stringBuilder.Append((string)(this.printerProperties.crafterMaterialNeedText.Translate((NamedArgument)requestedItem.amount, (NamedArgument)requestedItem.thingDef.LabelCap) + " "));
                }
            }
            return stringBuilder.ToString();
        }


        public void InitiatePawnCrafting() => Find.WindowStack.Add((Window)new CustomizeAndroidWindow(this));

        public void StartPrinting()
        {
            if (this.printerProperties == null)
            {
                this.printingTicksLeft = 60000;
                this.nextResourceTick = 2500;
            }
            else
            {
                this.printingTicksLeft = this.printerProperties.ticksToCraft + this.extraTimeCost;
                this.nextResourceTick = this.printerProperties.resourceTick;
            }
            this.printerStatus = CrafterStatus.Crafting;
        }

        public void StopPawnCrafting()
        {
            this.printerStatus = CrafterStatus.Idle;
            if (this.pawnToPrint != null && this.clonedPawnToPrint == null) //De novo printing
            {
                this.pawnToPrint.Destroy(DestroyMode.Vanish);
                this.pawnToPrint = (Pawn)null;
            }
            else
                CustomizeAndroidWindow.PawnUpdate(this.pawnToPrint, this.clonedPawnToPrint);
            this.ingredients = this.GetDirectlyHeldThings();
            this.ingredients.TryDropAll(this.InteractionCell, this.Map, ThingPlaceMode.Near);
            //this.needsSaved = false;
        }

        public override void Tick()
        {
            base.Tick();
            this.AdjustPowerNeed();
            this.ingredients = this.GetDirectlyHeldThings();
            if (!this.powerComp.PowerOn && this.soundSustainer != null && !this.soundSustainer.Ended)
                this.soundSustainer.End();
            if (this.flickableComp != null && (this.flickableComp == null || !this.flickableComp.SwitchIsOn))
                return;
            switch (this.printerStatus)
            {
                case CrafterStatus.Filling:
                    if (this.powerComp.PowerOn && Current.Game.tickManager.TicksGame % 300 == 0)
                        FleckMaker.ThrowSmoke(this.Position.ToVector3(), this.Map, 1f);
                    IEnumerable<ThingOrderRequest> pendingRequests = this.orderProcessor.PendingRequests(this.GetDirectlyHeldThings());
                    /*bool startPrinting = pendingRequests == null;
                    if (pendingRequests != null && pendingRequests.Count<ThingOrderRequest>() == 0)
                        startPrinting = true;
                    if (startPrinting)
                        this.StartPrinting();*/
                    if (pendingRequests == null || pendingRequests.Count<ThingOrderRequest>() == 0)
                    {
                        this.StartPrinting();
                    }
                    break;
                case CrafterStatus.Crafting:
                    if (!this.powerComp.PowerOn)
                        break;
                    IntVec3 position;
                    if (Current.Game.tickManager.TicksGame % 100 == 0)
                    {
                        position = this.Position;
                        FleckMaker.ThrowSmoke(position.ToVector3(), this.Map, 1.33f);
                    }
                    if (Current.Game.tickManager.TicksGame % 250 == 0)
                    {
                        for (int index = 0; index < 3; ++index)
                        {
                            position = this.Position;
                            FleckMaker.ThrowMicroSparks(position.ToVector3() + new Vector3((float)Rand.Range(-1, 1), 0.0f, (float)Rand.Range(-1, 1)), this.Map);
                        }
                    }
                    if (this.soundSustainer == null || this.soundSustainer.Ended)
                    {
                        SoundDef craftingSound = this.printerProperties.craftingSound;
                        if (craftingSound != null && craftingSound.sustain)
                        {
                            SoundInfo info = SoundInfo.InMap((TargetInfo)(Thing)this, MaintenanceType.PerTick);
                            this.soundSustainer = craftingSound.TrySpawnSustainer(info);
                        }
                    }
                    if (this.soundSustainer != null && !this.soundSustainer.Ended)
                        this.soundSustainer.Maintain();
                    --this.nextResourceTick;
                    if (this.nextResourceTick <= 0)
                    {
                        this.nextResourceTick = this.printerProperties.resourceTick;
                        foreach (ThingOrderRequest requestedItem in this.orderProcessor.requestedItems)
                        {
                            ThingOrderRequest thingOrderRequest = requestedItem;
                            if (thingOrderRequest.nutrition)
                            {
                                if ((double)this.CountNutrition() > 0.0)
                                {
                                    Thing thing1 = this.ingredients.First<Thing>((Func<Thing, bool>)(thing => thing.def.IsIngestible));
                                    if (thing1 != null)
                                    {
                                        int count = Math.Min((int)Math.Ceiling((double)thingOrderRequest.amount / ((double)(this.printerProperties.ticksToCraft + this.extraTimeCost) / (double)this.printerProperties.resourceTick)), thing1.stackCount);
                                        Thing resultingThing = (Thing)null;
                                        if (thing1 is Corpse t)
                                        {
                                            if (t.IsDessicated())
                                            {
                                                this.ingredients.TryDrop((Thing)t, this.InteractionCell, this.Map, ThingPlaceMode.Near, 1, out resultingThing);
                                            }
                                            else
                                            {
                                                this.ingredients.TryDrop((Thing)t, this.InteractionCell, this.Map, ThingPlaceMode.Near, 1, out resultingThing);
                                                t.InnerPawn?.equipment?.DropAllEquipment(this.InteractionCell, false);
                                                t.InnerPawn?.apparel?.DropAll(this.InteractionCell, false, true);
                                                thing1.Destroy();
                                            }
                                        }
                                        else
                                            this.ingredients.Take(thing1, count).Destroy();
                                    }
                                }
                            }
                            else if (this.ingredients.Any<Thing>((Func<Thing, bool>)(thing => !(thing is Pawn) && thing.def == thingOrderRequest.thingDef)))
                            {
                                Thing thing2 = this.ingredients.First<Thing>((Func<Thing, bool>)(thing => thing.def == thingOrderRequest.thingDef));
                                if (thing2 != null)
                                {
                                    int count = Math.Min((int)Math.Ceiling((double)thingOrderRequest.amount / ((double)(this.printerProperties.ticksToCraft + this.extraTimeCost) / (double)this.printerProperties.resourceTick)), thing2.stackCount);
                                    this.ingredients.Take(thing2, count).Destroy();
                                }
                            }
                        }
                    }
                    if (this.printingTicksLeft > 0)
                    {
                        --this.printingTicksLeft;
                        break;
                    }
                    this.printerStatus = CrafterStatus.Finished;
                    break;
                case CrafterStatus.Finished:
                    //Upgrade
                    if (IsUpgrade)
                    {
                        ThingOwner directlyHeldThings = this.GetDirectlyHeldThings();
                        Pawn pawn = (Pawn)directlyHeldThings.FirstOrDefault(p => p is Pawn);
                        ApplyUpgrades(pawn);
                        //this.needsSaved = false;
                        foreach (Thing thing in (IEnumerable<Thing>)directlyHeldThings)
                        {
                            if (thing is not Pawn)
                                thing.Destroy();
                        }
                        pawn.health.AddHediff(RimWorld.HediffDefOf.CryptosleepSickness);
                        pawn.needs.mood.thoughts.memories.TryGainMemory(NeedsDefOf.ChJAndroidSpawned);
                        Find.LetterStack.ReceiveLetter((Letter)LetterMaker.MakeLetter("AndroidUpgradedLetterLabel".Translate((NamedArgument)this.pawnToPrint.Name.ToStringShort), "AndroidUpgradedLetterDescription".Translate((NamedArgument)this.pawnToPrint.Name.ToStringFull), LetterDefOf.PositiveEvent, (LookTargets)(Thing)this.pawnToPrint));
                        this.pawnToPrint = null;
                        this.clonedPawnToPrint = null;
                        this.printerStatus = CrafterStatus.Idle;
                        this.extraTimeCost = 0;
                        this.orderProcessor.requestedItems.Clear();
                        break;
                    }
                    if (this.pawnToPrint == null)
                        break;
                    /*foreach (Thing directlyHeldThing in this.GetDirectlyHeldThings())
                    {
                        if (directlyHeldThing is not Pawn)
                            directlyHeldThing.Destroy();
                    }*/
                    this.innerContainer.ClearAndDestroyContents();
                    FilthMaker.TryMakeFilth(this.InteractionCell, this.Map, RimWorld.ThingDefOf.Filth_Slime, 5, FilthSourceFlags.None);
                    GenSpawn.Spawn((Thing)this.pawnToPrint, this.InteractionCell, this.Map);
                    this.pawnToPrint.health.AddHediff(RimWorld.HediffDefOf.CryptosleepSickness);
                    this.pawnToPrint.needs.mood.thoughts.memories.TryGainMemory(NeedsDefOf.ChJAndroidSpawned);
                    Find.LetterStack.ReceiveLetter((Letter)LetterMaker.MakeLetter("AndroidPrintedLetterLabel".Translate((NamedArgument)this.pawnToPrint.Name.ToStringShort), "AndroidPrintedLetterDescription".Translate((NamedArgument)this.pawnToPrint.Name.ToStringFull), LetterDefOf.PositiveEvent, (LookTargets)(Thing)this.pawnToPrint));
                    this.pawnToPrint = null;
                    this.printerStatus = CrafterStatus.Idle;
                    this.extraTimeCost = 0;
                    this.orderProcessor.requestedItems.Clear();
                    break;
                default:
                    if (this.soundSustainer == null || this.soundSustainer.Ended)
                        break;
                    this.soundSustainer.End();
                    break;
            }
        }



        public void AdjustPowerNeed()
        {
            if (this.flickableComp == null || this.flickableComp != null && this.flickableComp.SwitchIsOn)
            {
                if (this.printerStatus == CrafterStatus.Crafting)
                    this.powerComp.PowerOutput = -this.powerComp.Props.PowerConsumption;
                else
                    this.powerComp.PowerOutput = (float)(-(double)this.powerComp.Props.PowerConsumption * 0.10000000149011612);
            }
            else
                this.powerComp.PowerOutput = 0.0f;
        }

        public float CountNutrition()
        {
            this.ingredients = this.GetDirectlyHeldThings();
            float num1 = 0.0f;
            foreach (Thing ingredient in (IEnumerable<Thing>)this.ingredients)
            {
                if (ingredient is Corpse corpse)
                {
                    if (!corpse.IsDessicated())
                        num1 += FoodUtility.GetBodyPartNutrition(corpse, corpse.InnerPawn.RaceProps.body.corePart);
                }
                else if (ingredient.def.IsIngestible && !(ingredient is Pawn))
                {
                    double num2 = (double)num1;
                    ThingDef def = ingredient.def;
                    double num3 = (def != null ? (double)def.ingestible.CachedNutrition : 0.05000000074505806) * (double)ingredient.stackCount;
                    num1 = (float)(num2 + num3);
                }
            }
            return num1;
        }

        public StorageSettings GetStoreSettings() => this.inputSettings;

        public StorageSettings GetParentStoreSettings() => this.def.building.fixedStorageSettings;

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (var option in base.GetFloatMenuOptions(selPawn))
                yield return option;
            if (this.innerContainer.Count == 0 && selPawn.IsAndroid())
            {
                if (!selPawn.CanReach(this, Verse.AI.PathEndMode.InteractionCell, Danger.Deadly))
                {
                    yield return new FloatMenuOption("CannotUseNoPath".Translate(), null);
                }
                else
                {
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterAndroidPrinter".Translate(), () => selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterAndroidPrinterCasket, this))), selPawn, this, "ReservedBy");
                }
            }
        }

        public override void Open()
        {
            if (printerStatus != CrafterStatus.Idle && printerStatus != CrafterStatus.Finished) return;
            base.Open();

        }

        public override void EjectContents()
        {
            Find.WindowStack.TryRemove(typeof(CustomizeAndroidWindow), false);
            foreach (var thing in this.innerContainer)
            {
                if (thing is Pawn pawn)
                    PawnComponentsUtility.AddComponentsForSpawn(pawn);
            }
            if (!this.Destroyed)
                SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.Position, this.Map)));
            this.printerStatus = CrafterStatus.Idle;
            this.pawnToPrint = null;
            this.clonedPawnToPrint = null;
            base.EjectContents();
        }
        public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (!base.TryAcceptThing(thing, allowSpecialEffects))
                return false;
            if (allowSpecialEffects)
                SoundDefOf.CryptosleepCasket_Accept.PlayOneShot(new TargetInfo(this.Position, this.Map));
            return true;
        }

        private void ApplyUpgrades(Pawn target)
        {
            foreach (var upgrade in upgradesToApply)
            {
                UpgradeMaker.Make(upgrade).Apply(target);
            }
            upgradesToApply.Clear();
        }
    }
}
