// Decompiled with JetBrains decompiler
// Type: Androids.Building_AndroidPrinter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

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
    public class Building_AndroidPrinter : 
        Building_AndroidPrinterCasket, 
        IStoreSettingsParent, 
        IPawnCrafter
    {
        public static float requestNutrition = 20f;
        public static int requestPlasteel = 150;
        public static int requestComponents = 20;
        public ThingOwner<Thing> ingredients = new ThingOwner<Thing>();
        public CrafterStatus printerStatus;
        public Pawn pawnToPrint;
        public Pawn clonedPawnToPrint;
        public ThingOrderProcessor orderProcessor;
        public int extraTimeCost = 0;
        public StorageSettings inputSettings;
        private Sustainer soundSustainer;
        private CompPowerTrader powerComp;
        private CompFlickable flickableComp;
        protected PawnCrafterProperties printerProperties;
        public int printingTicksLeft = 0;
        public int nextResourceTick = 0;

        public bool StorageTabVisible => true;

        public PawnCrafterProperties PrinterProperties => this.def.GetModExtension<PawnCrafterProperties>();

        public Pawn PawnInside => (Pawn)this.GetDirectlyHeldThings().Where<Thing>((Func<Thing, bool>)(p => p is Pawn)).FirstOrDefault<Thing>();

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
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
                    this.orderProcessor = new ThingOrderProcessor((ThingOwner)this.ingredients, this.inputSettings);
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
                    this.orderProcessor = new ThingOrderProcessor((ThingOwner)this.ingredients, this.inputSettings);
            }
            this.AdjustPowerNeed();
        }

        public override void PostMake()
        {
            base.PostMake();
            this.inputSettings = new StorageSettings((IStoreSettingsParent)this);
            if (this.def.building.defaultStorageSettings == null)
                return;
            this.inputSettings.CopyFrom(this.def.building.defaultStorageSettings);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner<Thing>>(ref this.ingredients, "ingredients");
            Scribe_Values.Look<CrafterStatus>(ref this.printerStatus, "printerStatus");
            Scribe_Values.Look<int>(ref this.printingTicksLeft, "printingTicksLeft");
            Scribe_Values.Look<int>(ref this.nextResourceTick, "nextResourceTick");
            Scribe_Deep.Look<Pawn>(ref this.pawnToPrint, "androidToPrint");
            Scribe_Deep.Look<Pawn>(ref this.clonedPawnToPrint, "clonedPawnToPrint");
            Scribe_Deep.Look<StorageSettings>(ref this.inputSettings, "inputSettings");
            Scribe_Deep.Look<ThingOrderProcessor>(ref this.orderProcessor, "orderProcessor", (object)this.ingredients, (object)this.inputSettings);
            Scribe_Values.Look<int>(ref this.extraTimeCost, "extraTimeCost");
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (mode != 0)
                this.ingredients.TryDropAll(this.PositionHeld, this.MapHeld, ThingPlaceMode.Near);
            base.Destroy(mode);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            List<Gizmo> gizmos = new List<Gizmo>(base.GetGizmos());
            if (this.pawnToPrint != null)
                gizmos.Insert(0, (Gizmo)new Gizmo_PrinterPawnInfo((IPawnCrafter)this));
            if (this.printerStatus != CrafterStatus.Finished)
                gizmos.Insert(0, (Gizmo)new Gizmo_TogglePrinting((IPawnCrafter)this));
            if (DebugSettings.godMode && this.pawnToPrint != null)
            {
                List<Gizmo> gizmoList = gizmos;
                Command_Action commandAction = new Command_Action();
                commandAction.defaultLabel = "DEBUG: Finish crafting.";
                commandAction.defaultDesc = "Finishes crafting the pawn.";
                commandAction.action = (Action)(() => this.printerStatus = CrafterStatus.Finished);
                gizmoList.Insert(0, (Gizmo)commandAction);
            }
            return (IEnumerable<Gizmo>)gizmos;
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

        public bool ReadyToPrint() => this.printerStatus == CrafterStatus.Filling && this.orderProcessor.PendingRequests() == null;

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
            if (this.pawnToPrint != null && this.clonedPawnToPrint == null)
            {
                this.pawnToPrint.Destroy(DestroyMode.Vanish);
                this.pawnToPrint = (Pawn)null;
            }
            else
                CustomizeAndroidWindow.PawnUpdate(this.pawnToPrint, this.clonedPawnToPrint);
            this.ingredients.TryDropAll(this.InteractionCell, this.Map, ThingPlaceMode.Near);
        }

        public override void Tick()
        {
            base.Tick();
            this.AdjustPowerNeed();
            if (!this.powerComp.PowerOn && this.soundSustainer != null && !this.soundSustainer.Ended)
                this.soundSustainer.End();
            if (this.flickableComp != null && (this.flickableComp == null || !this.flickableComp.SwitchIsOn))
                return;
            switch (this.printerStatus)
            {
                case CrafterStatus.Filling:
                    if (this.powerComp.PowerOn && Current.Game.tickManager.TicksGame % 300 == 0)
                        FleckMaker.ThrowSmoke(this.Position.ToVector3(), this.Map, 1f);
                    IEnumerable<ThingOrderRequest> source = this.orderProcessor.PendingRequests();
                    bool flag = source == null;
                    if (source != null && source.Count<ThingOrderRequest>() == 0)
                        flag = true;
                    if (flag)
                    {
                        this.StartPrinting();
                        break;
                    }
                    break;
                case CrafterStatus.Crafting:
                    if (this.powerComp.PowerOn)
                    {
                        if (Current.Game.tickManager.TicksGame % 100 == 0)
                            FleckMaker.ThrowSmoke(this.Position.ToVector3(), this.Map, 1.33f);
                        if (Current.Game.tickManager.TicksGame % 250 == 0)
                        {
                            for (int index = 0; index < 3; ++index)
                                FleckMaker.ThrowMicroSparks(this.Position.ToVector3() + new Vector3((float)Rand.Range(-1, 1), 0.0f, (float)Rand.Range(-1, 1)), this.Map);
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
                                                    this.ingredients.TryDrop((Thing)t, this.InteractionCell, this.Map, ThingPlaceMode.Near, 1, out resultingThing, (Action<Thing, int>)null, (Predicate<IntVec3>)null);
                                                }
                                                else
                                                {
                                                    this.ingredients.TryDrop((Thing)t, this.InteractionCell, this.Map, ThingPlaceMode.Near, 1, out resultingThing, (Action<Thing, int>)null, (Predicate<IntVec3>)null);
                                                    t.InnerPawn?.equipment?.DropAllEquipment(this.InteractionCell, false);
                                                    t.InnerPawn?.apparel?.DropAll(this.InteractionCell, false);
                                                    thing1.Destroy();
                                                }
                                            }
                                            else
                                                this.ingredients.Take(thing1, count).Destroy();
                                        }
                                    }
                                }
                                else if (this.ingredients.Any<Thing>((Func<Thing, bool>)(thing => thing.def == thingOrderRequest.thingDef)))
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
                            --this.printingTicksLeft;
                        else
                            this.printerStatus = CrafterStatus.Finished;
                        break;
                    }
                    break;
                case CrafterStatus.Finished:
                    if (this.pawnToPrint != null)
                    {
                        this.ingredients.ClearAndDestroyContents();
                        FilthMaker.TryMakeFilth(this.InteractionCell, this.Map, RimWorld.ThingDefOf.Filth_Slime, 5);
                        GenSpawn.Spawn((Thing)this.pawnToPrint, this.InteractionCell, this.Map);
                        this.pawnToPrint.health.AddHediff(RimWorld.HediffDefOf.CryptosleepSickness);
                        this.pawnToPrint.needs.mood.thoughts.memories.TryGainMemory(NeedsDefOf.ChJAndroidSpawned);
                        Find.LetterStack.ReceiveLetter((Letter)LetterMaker.MakeLetter("AndroidPrintedLetterLabel".Translate((NamedArgument)this.pawnToPrint.Name.ToStringShort), "AndroidPrintedLetterDescription".Translate((NamedArgument)this.pawnToPrint.Name.ToStringFull), LetterDefOf.PositiveEvent, (LookTargets)(Thing)this.pawnToPrint));
                        this.pawnToPrint = (Pawn)null;
                        this.printerStatus = CrafterStatus.Idle;
                        this.extraTimeCost = 0;
                        this.orderProcessor.requestedItems.Clear();
                        break;
                    }
                    break;
                default:
                    if (this.soundSustainer != null && !this.soundSustainer.Ended)
                    {
                        this.soundSustainer.End();
                        break;
                    }
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
            float num1 = 0.0f;
            foreach (Thing ingredient in this.ingredients)
            {
                if (ingredient is Corpse corpse)
                {
                    if (!corpse.IsDessicated())
                        num1 += FoodUtility.GetBodyPartNutrition(corpse, corpse.InnerPawn.RaceProps.body.corePart);
                }
                else if (ingredient.def.IsIngestible)
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

        public Pawn PawnBeingCrafted() => this.pawnToPrint;

        public CrafterStatus PawnCrafterStatus() => this.printerStatus;

        public void Notify_SettingsChanged()
        {
        }
    }
}
