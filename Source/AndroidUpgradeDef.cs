// Decompiled with JetBrains decompiler
// Type: Androids.AndroidUpgradeDef
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Androids
{
  public class AndroidUpgradeDef : Def
  {
    public int orderID;
    public AndroidUpgradeGroupDef upgradeGroup;
    public System.Type commandType = typeof (UpgradeCommand_Hediff);
    public string iconTexturePath;
    public List<ThingOrderRequest> costList = new List<ThingOrderRequest>();
    public List<ThingDef> costsNotAffectedByBodySize = new List<ThingDef>();
    public int extraPrintingTime;
    public List<HediffApplication> hediffs = new List<HediffApplication>();
    public HediffDef hediffToApply;
    public float hediffSeverity = 1f;
    public List<BodyPartGroupDef> partsToApplyTo;
    public BodyPartDepth partsDepth;
    public List<string> exclusivityGroups = new List<string>();
    public BodyTypeDef newBodyType;
    public bool changeSkinColor;
    public Color newSkinColor = new Color(1f, 1f, 1f);
    public ResearchProjectDef requiredResearch;
    public List<string> spawnInBackstories = new List<string>();
    public bool invasive = true;
  }
}
