// Decompiled with JetBrains decompiler
// Type: Androids.Building_PawnCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

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
    public int craftingTicksLeft = 0;
    public int nextResourceTick = 0;
    public int craftingTime = 0;

    public void Notify_SettingsChanged()
    {
    }

    public float CraftingFinishedPercentage => this.printerProperties.customCraftingTime ? ((float) this.craftingTime - (float) this.craftingTicksLeft) / (float) this.craftingTime : ((float) this.printerProperties.ticksToCraft - (float) this.craftingTicksLeft) / (float) this.printerProperties.ticksToCraft;

    public int CraftingTicks => this.printerProperties.customCraftingTime ? this.craftingTime : this.printerProperties.ticksToCraft;

    public bool StorageTabVisible => true;

    public void GetChildHolders(List<IThingHolder> outChildren)
    {
    }

    public ThingOwner GetDirectlyHeldThings() => (ThingOwner) this.ingredients;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
      base.SpawnSetup(map, respawningAfterLoad);
      this.powerComp = this.GetComp<CompPowerTrader>();
      this.flickableComp = this.GetComp<CompFlickable>();
      if (this.inputSettings == null)
      {
        this.inputSettings = new StorageSettings((IStoreSettingsParent) this);
        if (this.def.building.defaultStorageSettings != null)
          this.inputSettings.CopyFrom(this.def.building.defaultStorageSettings);
      }
      this.printerProperties = this.def.GetModExtension<PawnCrafterProperties>();
      if (!this.printerProperties.customOrderProcessor)
      {
        this.orderProcessor = new ThingOrderProcessor((ThingOwner) this.ingredients, this.inputSettings);
        if (this.printerProperties != null)
          this.orderProcessor.requestedItems.AddRange((IEnumerable<ThingOrderRequest>) this.printerProperties.costList);
      }
      this.AdjustPowerNeed();
    }

    public override void PostMake()
    {
      base.PostMake();
      this.inputSettings = new StorageSettings((IStoreSettingsParent) this);
      if (this.def.building.defaultStorageSettings == null)
        return;
      this.inputSettings.CopyFrom(this.def.building.defaultStorageSettings);
    }

    public override void ExposeData()
    {
      base.ExposeData();
      Scribe_Deep.Look<ThingOwner<Thing>>(ref this.ingredients, "ingredients");
      Scribe_Values.Look<CrafterStatus>(ref this.crafterStatus, "crafterStatus");
      Scribe_Values.Look<int>(ref this.craftingTicksLeft, "craftingTicksLeft");
      Scribe_Values.Look<int>(ref this.nextResourceTick, "nextResourceTick");
      Scribe_Deep.Look<Pawn>(ref this.pawnBeingCrafted, "pawnBeingCrafted");
      Scribe_Deep.Look<StorageSettings>(ref this.inputSettings, "inputSettings");
      Scribe_Values.Look<int>(ref this.craftingTime, "craftingTime");
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
      if (this.pawnBeingCrafted != null)
        gizmos.Insert(0, (Gizmo) new Gizmo_PrinterPawnInfo((IPawnCrafter) this));
      if (this.crafterStatus != CrafterStatus.Finished)
        gizmos.Insert(0, (Gizmo) new Gizmo_TogglePrinting((IPawnCrafter) this));
      if (DebugSettings.godMode && this.pawnBeingCrafted != null)
      {
        List<Gizmo> gizmoList = gizmos;
        Command_Action commandAction = new Command_Action();
        commandAction.defaultLabel = "DEBUG: Finish crafting.";
        commandAction.defaultDesc = "Finishes crafting the pawn.";
        commandAction.action = (Action) (() => this.crafterStatus = CrafterStatus.Finished);
        gizmoList.Insert(0, (Gizmo) commandAction);
      }
      return (IEnumerable<Gizmo>) gizmos;
    }

    public virtual bool ReadyToCraft()
    {
      IEnumerable<ThingOrderRequest> source = this.orderProcessor.PendingRequests();
      bool flag = source == null;
      if (source != null && source.Count<ThingOrderRequest>() == 0)
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
      this.pawnBeingCrafted = (Pawn) null;
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

    public virtual void FinishAction() => FilthMaker.TryMakeFilth(this.InteractionCell, this.Map, RimWorld.ThingDefOf.Filth_Slime, 5);

    public override string GetInspectString()
    {
      if (this.ParentHolder != null && !(this.ParentHolder is Map))
        return base.GetInspectString();
      StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
      stringBuilder.AppendLine();
      stringBuilder.AppendLine((string) this.printerProperties.crafterStatusText.Translate((NamedArgument) (this.printerProperties.crafterStatusEnumText + ((int) this.crafterStatus).ToString()).Translate()));
      if (this.crafterStatus == CrafterStatus.Crafting)
        stringBuilder.AppendLine((string) this.printerProperties.crafterProgressText.Translate((NamedArgument) this.CraftingFinishedPercentage.ToStringPercent()));
      if (this.crafterStatus == CrafterStatus.Filling)
      {
        bool flag = true;
        foreach (ThingOrderRequest requestedItem in this.orderProcessor.requestedItems)
        {
          if (requestedItem.nutrition)
          {
            float num = this.CountNutrition();
            if ((double) num > 0.0)
            {
              stringBuilder.Append((string) (this.printerProperties.crafterMaterialNeedText.Translate((NamedArgument) (requestedItem.amount - num), (NamedArgument) this.printerProperties.crafterNutritionText.Translate()) + " "));
              flag = false;
            }
          }
          else
          {
            int num = this.ingredients.TotalStackCountOfDef(requestedItem.thingDef);
            if ((double) num < (double) requestedItem.amount)
            {
              stringBuilder.Append((string) (this.printerProperties.crafterMaterialNeedText.Translate((NamedArgument) (requestedItem.amount - (float) num), (NamedArgument) requestedItem.thingDef.LabelCap) + " "));
              flag = false;
            }
          }
        }
        if (!flag)
          stringBuilder.AppendLine();
      }
      if (this.ingredients.Count > 0)
        stringBuilder.Append((string) (this.printerProperties.crafterMaterialsText.Translate() + " "));
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
          this.ExtraCrafterTickAction();
          if (this.powerComp.PowerOn)
          {
            --this.nextResourceTick;
            if (this.nextResourceTick <= 0)
            {
              this.nextResourceTick = this.printerProperties.resourceTick;
              foreach (ThingOrderRequest requestedItem in this.orderProcessor.requestedItems)
              {
                ThingOrderRequest thingOrderRequest = requestedItem;
                if (thingOrderRequest.nutrition)
                {
                  if ((double) this.CountNutrition() > 0.0)
                  {
                    Thing thing1 = this.ingredients.First<Thing>((Func<Thing, bool>) (thing => thing.def.IsIngestible));
                    if (thing1 != null)
                    {
                      int count = Math.Min((int) Math.Ceiling((double) thingOrderRequest.amount / ((double) this.CraftingTicks / (double) this.printerProperties.resourceTick)), thing1.stackCount);
                      Thing resultingThing = (Thing) null;
                      if (thing1 is Corpse t)
                      {
                        if (t.IsDessicated())
                        {
                          this.ingredients.TryDrop((Thing) t, this.InteractionCell, this.Map, ThingPlaceMode.Near, 1, out resultingThing, (Action<Thing, int>) null, (Predicate<IntVec3>) null);
                        }
                        else
                        {
                          this.ingredients.TryDrop((Thing) t, this.InteractionCell, this.Map, ThingPlaceMode.Near, 1, out resultingThing, (Action<Thing, int>) null, (Predicate<IntVec3>) null);
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
                else if (this.ingredients.Any<Thing>((Func<Thing, bool>) (thing => thing.def == thingOrderRequest.thingDef)))
                {
                  Thing thing2 = this.ingredients.First<Thing>((Func<Thing, bool>) (thing => thing.def == thingOrderRequest.thingDef));
                  if (thing2 != null)
                  {
                    int count = Math.Min((int) Math.Ceiling((double) thingOrderRequest.amount / ((double) this.CraftingTicks / (double) this.printerProperties.resourceTick)), thing2.stackCount);
                    this.ingredients.Take(thing2, count).Destroy();
                  }
                }
              }
            }
            if (this.craftingTicksLeft > 0)
              --this.craftingTicksLeft;
            else
              this.crafterStatus = CrafterStatus.Finished;
            break;
          }
          break;
        case CrafterStatus.Finished:
          if (this.pawnBeingCrafted != null)
          {
            this.ExtraCrafterTickAction();
            this.ingredients.ClearAndDestroyContents();
            GenSpawn.Spawn((Thing) this.pawnBeingCrafted, this.InteractionCell, this.Map);
            if (this.printerProperties.hediffOnPawnCrafted != null)
              this.pawnBeingCrafted.health.AddHediff(this.printerProperties.hediffOnPawnCrafted);
            if (this.printerProperties.thoughtOnPawnCrafted != null)
              this.pawnBeingCrafted.needs.mood.thoughts.memories.TryGainMemory(this.printerProperties.thoughtOnPawnCrafted);
            Find.LetterStack.ReceiveLetter((Letter) LetterMaker.MakeLetter(this.printerProperties.pawnCraftedLetterLabel.Translate((NamedArgument) this.pawnBeingCrafted.Name.ToStringShort), this.printerProperties.pawnCraftedLetterText.Translate((NamedArgument) this.pawnBeingCrafted.Name.ToStringFull), LetterDefOf.PositiveEvent, (LookTargets) (Thing) this.pawnBeingCrafted));
            this.pawnBeingCrafted = (Pawn) null;
            this.crafterStatus = CrafterStatus.Idle;
            this.FinishAction();
            break;
          }
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
          double num2 = (double) num1;
          ThingDef def = ingredient.def;
          double num3 = (def != null ? (double) def.ingestible.CachedNutrition : 0.05000000074505806) * (double) ingredient.stackCount;
          num1 = (float) (num2 + num3);
        }
      }
      return num1;
    }

    public StorageSettings GetStoreSettings() => this.inputSettings;

    public StorageSettings GetParentStoreSettings() => this.def.building.fixedStorageSettings;

    public Pawn PawnBeingCrafted() => this.pawnBeingCrafted;

    public CrafterStatus PawnCrafterStatus() => this.crafterStatus;
  }
}
