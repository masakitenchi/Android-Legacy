// Decompiled with JetBrains decompiler
// Type: Androids.AndroidUpgradeDef
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Androids
{
  public class AndroidUpgradeDef : Def
  {
    public int orderID = 0;
    public AndroidUpgradeGroupDef upgradeGroup;
    public System.Type commandType = typeof (UpgradeCommand_Hediff);
    public string iconTexturePath;
    public List<ThingOrderRequest> costList = new List<ThingOrderRequest>();
    public List<ThingDef> costsNotAffectedByBodySize = new List<ThingDef>();
    public int extraPrintingTime = 0;
    public List<HediffApplication> hediffs = new List<HediffApplication>();
    public HediffDef hediffToApply;
    public float hediffSeverity = 1f;
    public List<BodyPartGroupDef> partsToApplyTo;
    public BodyPartDepth partsDepth = BodyPartDepth.Undefined;
    public List<string> exclusivityGroups = new List<string>();
    public BodyTypeDef newBodyType;
    public bool changeSkinColor = false;
    public Color newSkinColor = new Color(1f, 1f, 1f);
    public ResearchProjectDef requiredResearch;
    public List<string> spawnInBackstories = new List<string>();
    public bool invasive = true;
  }
}
