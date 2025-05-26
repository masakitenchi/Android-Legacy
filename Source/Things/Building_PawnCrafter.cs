// Decompiled with JetBrains decompiler
// Type: Androids.Building_PawnCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Text;

namespace Androids
{
    public class Building_PawnCrafter : Building, IThingHolder, IStoreSettingsParent, IPawnCrafter
    {
        public ThingOwner<Thing> ingredients = new ThingOwner<Thing>();
        public CrafterStatus crafterStatus;
        public Pawn pawnBeingCrafted;
        public StorageSettings inputSettings;
        protected CompPowerTrader powerComp;
        protected CompFlickable flickableComp;
        protected PawnCrafterProperties printerProperties;
        public ThingOrderProcessor orderProcessor;
        public int craftingTicksLeft;
        public int nextResourceTick;
        public int craftingTime;

        public void Notify_SettingsChanged()
        {
        }
        public float CraftingFinishedPercentage => this.printerProperties.customCraftingTime ? (craftingTime - (float)this.craftingTicksLeft) / craftingTime : (printerProperties.ticksToCraft - (float)this.craftingTicksLeft) / printerProperties.ticksToCraft;

        public int CraftingTicks => this.printerProperties.customCraftingTime ? this.craftingTime : this.printerProperties.ticksToCraft;

        public bool StorageTabVisible => true;

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
        }

        public ThingOwner GetDirectlyHeldThings() => ingredients;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.powerComp = this.GetComp<CompPowerTrader>();
            this.flickableComp = this.GetComp<CompFlickable>();
            if (this.inputSettings == null)
            {
                this.inputSettings = new StorageSettings(this);
                if (this.def.building.defaultStorageSettings != null)
                    this.inputSettings.CopyFrom(this.def.building.defaultStorageSettings);
            }
            this.printerProperties = this.def.GetModExtension<PawnCrafterProperties>();
            if (!this.printerProperties.customOrderProcessor)
            {
                this.orderProcessor = new ThingOrderProcessor(ingredients, this.inputSettings);
                if (this.printerProperties != null)
                    this.orderProcessor.requestedItems.AddRange(printerProperties.costList);
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
            Scribe_Deep.Look(ref this.ingredients, "ingredients");
            Scribe_Values.Look(ref this.crafterStatus, "crafterStatus");
            Scribe_Values.Look(ref this.craftingTicksLeft, "craftingTicksLeft");
            Scribe_Values.Look(ref this.nextResourceTick, "nextResourceTick");
            Scribe_Deep.Look(ref this.pawnBeingCrafted, "pawnBeingCrafted");
            Scribe_Deep.Look(ref this.inputSettings, "inputSettings");
            Scribe_Values.Look(ref this.craftingTime, "craftingTime");
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (mode != DestroyMode.Vanish)
                this.ingredients.TryDropAll(this.PositionHeld, this.MapHeld, ThingPlaceMode.Near);
            base.Destroy(mode);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            List<Gizmo> gizmos = new List<Gizmo>(base.GetGizmos());
            if (this.pawnBeingCrafted != null)
                gizmos.Insert(0, new Gizmo_PrinterPawnInfo(this));
            if (this.crafterStatus != CrafterStatus.Finished)
                gizmos.Insert(0, new Gizmo_TogglePrinting(this));
            if (DebugSettings.godMode && this.pawnBeingCrafted != null)
            {
                List<Gizmo> gizmoList = gizmos;
                Command_Action commandAction = new Command_Action();
                commandAction.defaultLabel = "DEBUG: Finish crafting.";
                commandAction.defaultDesc = "Finishes crafting the pawn.";
                commandAction.action = () => this.crafterStatus = CrafterStatus.Finished;
                gizmoList.Insert(0, commandAction);
            }
            return gizmos;
        }

        public virtual bool ReadyToCraft()
        {
            IEnumerable<ThingOrderRequest> source = this.orderProcessor.PendingRequests(this.GetDirectlyHeldThings());
            bool flag = source == null;
            if (source != null && source.Count() == 0)
                flag = true;
            return this.crafterStatus == CrafterStatus.Filling & flag;
        }

        public virtual void InitiatePawnCrafting()
        {
            this.pawnBeingCrafted = PawnGenerator.GeneratePawn(this.printerProperties.pawnKind, this.Faction);
            this.crafterStatus = CrafterStatus.Filling;
        }

        public virtual void StartPrinting()
        {
            this.craftingTicksLeft = this.CraftingTicks;
            this.nextResourceTick = this.printerProperties.resourceTick;
            this.crafterStatus = CrafterStatus.Crafting;
        }

        public virtual void StopPawnCrafting()
        {
            this.crafterStatus = CrafterStatus.Idle;
            if (this.pawnBeingCrafted != null)
                this.pawnBeingCrafted.Destroy(DestroyMode.Vanish);
            this.pawnBeingCrafted = null;
            this.ingredients.TryDropAll(this.InteractionCell, this.Map, ThingPlaceMode.Near);
        }

        public virtual void ExtraCrafterTickAction()
        {
            switch (this.crafterStatus)
            {
                case CrafterStatus.Filling:
                    if (!this.powerComp.PowerOn || Current.Game.tickManager.TicksGame % 300 != 0)
                        break;
                    FleckMaker.ThrowSmoke(this.Position.ToVector3(), this.Map, 1f);
                    break;
                case CrafterStatus.Crafting:
                    if (!this.powerComp.PowerOn || Current.Game.tickManager.TicksGame % 100 != 0)
                        break;
                    FleckMaker.ThrowSmoke(this.Position.ToVector3(), this.Map, 1.33f);
                    break;
            }
        }

        public virtual void FinishAction() => FilthMaker.TryMakeFilth(this.InteractionCell, this.Map, RimWorld.ThingDefOf.Filth_Slime, 5, FilthSourceFlags.None);

        public override string GetInspectString()
        {
            if (this.ParentHolder != null && !(this.ParentHolder is Map))
                return base.GetInspectString();
            StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
            stringBuilder.AppendLine();
            stringBuilder.AppendLine((string)this.printerProperties.crafterStatusText.Translate((NamedArgument)(this.printerProperties.crafterStatusEnumText + ((int)this.crafterStatus).ToString()).Translate()));
            if (this.crafterStatus == CrafterStatus.Crafting)
                stringBuilder.AppendLine((string)this.printerProperties.crafterProgressText.Translate((NamedArgument)this.CraftingFinishedPercentage.ToStringPercent()));
            if (this.crafterStatus == CrafterStatus.Filling)
            {
                bool flag = true;
                foreach (ThingOrderRequest requestedItem in this.orderProcessor.requestedItems)
                {
                    if (requestedItem.nutrition)
                    {
                        float num = this.CountNutrition();
                        if ((double)num > 0.0)
                        {
                            stringBuilder.Append((string)(this.printerProperties.crafterMaterialNeedText.Translate((NamedArgument)(requestedItem.amount - num), (NamedArgument)this.printerProperties.crafterNutritionText.Translate()) + " "));
                            flag = false;
                        }
                    }
                    else
                    {
                        int num = this.ingredients.TotalStackCountOfDef(requestedItem.thingDef);
                        if (num < (double)requestedItem.amount)
                        {
                            stringBuilder.Append((string)(this.printerProperties.crafterMaterialNeedText.Translate((NamedArgument)(requestedItem.amount - num), (NamedArgument)requestedItem.thingDef.LabelCap) + " "));
                            flag = false;
                        }
                    }
                }
                if (!flag)
                    stringBuilder.AppendLine();
            }
            if (this.ingredients.Count > 0)
                stringBuilder.Append((string)(this.printerProperties.crafterMaterialsText.Translate() + " "));
            foreach (Thing ingredient in this.ingredients)
                stringBuilder.Append(ingredient.LabelCap + "; ");
            return stringBuilder.ToString().TrimEndNewlines();
        }

        public override void Tick()
        {
            base.Tick();
            this.AdjustPowerNeed();
            if (this.flickableComp != null && (this.flickableComp == null || !this.flickableComp.SwitchIsOn))
                return;
            switch (this.crafterStatus)
            {
                case CrafterStatus.Filling:
                    this.ExtraCrafterTickAction();
                    IEnumerable<ThingOrderRequest> source = this.orderProcessor.PendingRequests(this.GetDirectlyHeldThings());
                    bool flag = source == null;
                    if (source != null && source.Count() == 0)
                        flag = true;
                    if (!flag)
                        break;
                    this.StartPrinting();
                    break;
                case CrafterStatus.Crafting:
                    this.ExtraCrafterTickAction();
                    if (!this.powerComp.PowerOn)
                        break;
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
                                    Thing thing1 = this.ingredients.First(thing => thing.def.IsIngestible);
                                    if (thing1 != null)
                                    {
                                        int count = Math.Min((int)Math.Ceiling(thingOrderRequest.amount / (CraftingTicks / (double)this.printerProperties.resourceTick)), thing1.stackCount);
                                        Thing resultingThing = null;
                                        if (thing1 is Corpse t)
                                        {
                                            if (t.IsDessicated())
                                            {
                                                this.ingredients.TryDrop(t, this.InteractionCell, this.Map, ThingPlaceMode.Near, 1, out resultingThing, null, null);
                                            }
                                            else
                                            {
                                                this.ingredients.TryDrop(t, this.InteractionCell, this.Map, ThingPlaceMode.Near, 1, out resultingThing, null, null);
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
                            else if (this.ingredients.Any(thing => thing.def == thingOrderRequest.thingDef))
                            {
                                Thing thing2 = this.ingredients.First(thing => thing.def == thingOrderRequest.thingDef);
                                if (thing2 != null)
                                {
                                    int count = Math.Min((int)Math.Ceiling(thingOrderRequest.amount / (CraftingTicks / (double)this.printerProperties.resourceTick)), thing2.stackCount);
                                    this.ingredients.Take(thing2, count).Destroy();
                                }
                            }
                        }
                    }
                    if (this.craftingTicksLeft > 0)
                    {
                        --this.craftingTicksLeft;
                        break;
                    }
                    this.crafterStatus = CrafterStatus.Finished;
                    break;
                case CrafterStatus.Finished:
                    if (this.pawnBeingCrafted == null)
                        break;
                    this.ExtraCrafterTickAction();
                    this.ingredients.ClearAndDestroyContents();
                    GenSpawn.Spawn(pawnBeingCrafted, this.InteractionCell, this.Map);
                    if (this.printerProperties.hediffOnPawnCrafted != null && this.pawnBeingCrafted.health != null)
                        this.pawnBeingCrafted.health.AddHediff(this.printerProperties.hediffOnPawnCrafted);
                    if (this.printerProperties.thoughtOnPawnCrafted != null && this.pawnBeingCrafted?.needs?.mood != null)
                        this.pawnBeingCrafted.needs.mood.thoughts.memories.TryGainMemory(this.printerProperties.thoughtOnPawnCrafted);
                    Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter(this.printerProperties.pawnCraftedLetterLabel.Translate((NamedArgument)this.pawnBeingCrafted.Name.ToStringShort), this.printerProperties.pawnCraftedLetterText.Translate((NamedArgument)this.pawnBeingCrafted.Name.ToStringFull), LetterDefOf.PositiveEvent, (LookTargets)pawnBeingCrafted));
                    this.pawnBeingCrafted = null;
                    this.crafterStatus = CrafterStatus.Idle;
                    this.FinishAction();
                    break;
            }
        }

        public void AdjustPowerNeed()
        {
            if (this.flickableComp == null || this.flickableComp != null && this.flickableComp.SwitchIsOn)
            {
                if (this.crafterStatus == CrafterStatus.Crafting)
                    this.powerComp.PowerOutput = -this.powerComp.Props.PowerConsumption;
                else
                    this.powerComp.PowerOutput = -this.powerComp.Props.PowerConsumption * this.printerProperties.powerConsumptionFactorIdle;
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
                    double num3 = (def != null ? (double)def.ingestible.CachedNutrition : 0.05000000074505806) * ingredient.stackCount;
                    num1 = (float)(num2 + num3);
                }
            }
            return num1;
        }

        public StorageSettings GetStoreSettings() => this.inputSettings;

        public StorageSettings GetParentStoreSettings() => this.def.building.fixedStorageSettings;

        public Pawn PawnBeingCrafted => this.pawnBeingCrafted;

        public CrafterStatus PrinterStatus => this.crafterStatus;
    }
}
