// Decompiled with JetBrains decompiler
// Type: Androids.PostDefFixer
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using AlienRace;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Androids
{
    [StaticConstructorOnStartup]
    public static class PostDefFixer
    {
        static PostDefFixer()
        {
            int totalnum = default;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Androids: Fixing surgery recipes for Droids.");
            foreach (RecipeDef allDef in DefDatabase<RecipeDef>.AllDefs)
            {
                if (allDef.defName.StartsWith("Administer_"))
                {
                    totalnum += allDef.recipeUsers.RemoveAll((Predicate<ThingDef>)(thingDef => thingDef.HasModExtension<MechanicalPawnProperties>()));
                }
            }
            if (totalnum > 0)
                sb.AppendLine("Androids: Removed '" + totalnum.ToString() + "' recipes for Droids.");
            sb.AppendLine("Androids: Fixing belts whitelist for AlienRace.ThingDef_AlienRace with defName='ChjBattleDroid'.");
            List<ThingDef> whiteApparelList = ((ThingDef_AlienRace)ThingDef.Named("ChjBattleDroid")).alienRace.raceRestriction.whiteApparelList;
            foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs.Where
                (x=>x.IsApparel && 
                x.apparel.bodyPartGroups != null && 
                x.apparel.bodyPartGroups.Count ==1 && 
                x.apparel.bodyPartGroups.First<BodyPartGroupDef>().defName== "Waist"&& 
                x.apparel.layers != null && 
                x.apparel.layers.Count == 1 && 
                x.apparel.layers.First<ApparelLayerDef>().defName == "Belt" && 
                !whiteApparelList.Any<ThingDef>(i => i.defName == x.defName)))
            {
                sb.AppendLine($"Androids: Belt found and added: {allDef.defName}");
                whiteApparelList.Add(allDef);
            }
            Log.Message(sb.ToString());
        }
    }
}
