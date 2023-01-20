// Decompiled with JetBrains decompiler
// Type: Androids.PawnCrafterProperties
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class PawnCrafterProperties : DefModExtension
  {
    public PawnKindDef pawnKind;
    public List<ThingOrderRequest> costList = new List<ThingOrderRequest>();
    public List<ThingDef> disabledRaces = new List<ThingDef>();
    public string pawnCraftedLetterLabel = "AndroidPrintedLetterLabel";
    public string pawnCraftedLetterText = "AndroidPrintedLetterDescription";
    public string crafterStatusText = "AndroidPrinterStatus";
    public string crafterStatusEnumText = "AndroidPrinterStatusEnum";
    public string crafterProgressText = "AndroidPrinterProgress";
    public string crafterMaterialsText = "AndroidPrinterMaterials";
    public string crafterMaterialNeedText = "AndroidPrinterNeed";
    public string crafterNutritionText = "AndroidNutrition";
    public int ticksToCraft = 60000;
    public int resourceTick = 2500;
    public HediffDef hediffOnPawnCrafted;
    public ThoughtDef thoughtOnPawnCrafted;
    public float powerConsumptionFactorIdle = 0.1f;
    public List<SkillRequirement> skills = new List<SkillRequirement>();
    public int defaultSkillLevel = 6;
    public SoundDef craftingSound;
    public bool customOrderProcessor = false;
    public bool customCraftingTime = false;

    public float ResourceTicks() => (float) Math.Ceiling((double) this.ticksToCraft / (double) this.resourceTick);
  }
}
