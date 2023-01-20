// Decompiled with JetBrains decompiler
// Type: Androids.PostDefFixer
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using AlienRace;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Androids
{
  [StaticConstructorOnStartup]
  public static class PostDefFixer
  {
    static PostDefFixer()
    {
      Log.Message("Androids: Fixing surgery recipes for Droids.");
      foreach (RecipeDef allDef in DefDatabase<RecipeDef>.AllDefs)
      {
        if (allDef.defName.StartsWith("Administer_"))
        {
          int num = allDef.recipeUsers.RemoveAll((Predicate<ThingDef>) (thingDef => thingDef.HasModExtension<MechanicalPawnProperties>()));
          if (Prefs.LogVerbose && num > 0)
            Log.Message("Androids: Removed '" + num.ToString() + "' recipes for Droids.");
        }
      }
      Log.Message("Androids: Fixing belts whitelist for AlienRace.ThingDef_AlienRace with defName='ChjBattleDroid'.");
      List<ThingDef> whiteApparelList = ((ThingDef_AlienRace) ThingDef.Named("ChjBattleDroid")).alienRace.raceRestriction.whiteApparelList;
      foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
      {
        ThingDef thingDef = allDef;
        if (thingDef.IsApparel && thingDef.apparel.bodyPartGroups != null && thingDef.apparel.bodyPartGroups.Count == 1 && thingDef.apparel.bodyPartGroups.First<BodyPartGroupDef>().defName == "Waist" && thingDef.apparel.layers != null && thingDef.apparel.layers.Count == 1 && thingDef.apparel.layers.First<ApparelLayerDef>().defName == "Belt" && !whiteApparelList.Any<ThingDef>((Predicate<ThingDef>) (item => item.defName == thingDef.defName)))
        {
          if (Prefs.LogVerbose)
            Log.Message("Androids: Belt found and added: " + thingDef.defName);
          whiteApparelList.Add(thingDef);
        }
      }
    }
  }
}
