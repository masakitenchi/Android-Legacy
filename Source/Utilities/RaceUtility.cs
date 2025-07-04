// Decompiled with JetBrains decompiler
// Type: Androids.RaceUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using AlienRace;

namespace Androids
{
    public static class RaceUtility
    {
        private static bool cached = false;
        private static readonly HashSet<PawnKindDef> alienRaceKindsint = new ();
        public static bool AlienRacesExist => alienRaceKindsint.Count > 0;
        public static bool IsAndroid(this Pawn pawn) => pawn.def == ThingDefOf.ChjAndroid || pawn.health.hediffSet.HasHediff(HediffDefOf.ChjAndroidLike);

        public static IEnumerable<PawnKindDef> AvailableRacesForPrinter
        {
            get
            {
                if (!cached)
                {
                    foreach (ThingDef_AlienRace allDef in DefDatabase<ThingDef_AlienRace>.AllDefs)
                    {
                        PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefs.FirstOrDefault(def => def.race == allDef);
                        if (pawnKindDef != null)
                            alienRaceKindsint.Add(pawnKindDef);
                    }
                    alienRaceKindsint.RemoveWhere(def => def.race.defName == "Human");
                    alienRaceKindsint.RemoveWhere(def => def.race.HasModExtension<MechanicalPawnProperties>());
                    foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs.Where(x => x.HasModExtension<PawnCrafterProperties>()))
                    {
                        PawnCrafterProperties properties = allDef.GetModExtension<PawnCrafterProperties>();
                        if (properties != null && properties.pawnKind != null)
                        {
                            PawnKindDef pawnKindDef = properties.pawnKind;
                            if (pawnKindDef != null && !alienRaceKindsint.Contains(pawnKindDef))
                            {
                                alienRaceKindsint.Add(pawnKindDef);
                            }
                        }
                        foreach (ThingDef disabledRace in properties.disabledRaces)
                        {
                            ThingDef raceDef = disabledRace;
                            alienRaceKindsint.RemoveWhere((def => def.race == raceDef));
                        }
                    }
                    cached = true;
                }
                return alienRaceKindsint;
            }
        }

    }
}
