// Decompiled with JetBrains decompiler
// Type: Androids.RaceUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using AlienRace;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Androids
{
    public static class RaceUtility
    {
        private static List<PawnKindDef> alienRaceKindsint = new List<PawnKindDef>();
        private static bool alienRaceKindSearchDoneint = false;
        private static bool alienRacesFoundint = false;

        public static bool AlienRacesExist => RaceUtility.alienRacesFoundint;

        public static IEnumerable<PawnKindDef> AlienRaceKinds
        {
            get
            {
                if (!RaceUtility.alienRaceKindSearchDoneint)
                {
                    foreach (ThingDef_AlienRace allDef in DefDatabase<ThingDef_AlienRace>.AllDefs)
                    {
                        ThingDef_AlienRace alienDef = allDef;
                        PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefs.FirstOrDefault<PawnKindDef>((Func<PawnKindDef, bool>)(def => def.race == alienDef));
                        if (pawnKindDef != null)
                            RaceUtility.alienRaceKindsint.Add(pawnKindDef);
                    }
                    RaceUtility.alienRaceKindsint.RemoveAll((Predicate<PawnKindDef>)(def => def.race.defName == "Human"));
                    RaceUtility.alienRaceKindsint.RemoveAll((Predicate<PawnKindDef>)(def => def.race.HasModExtension<MechanicalPawnProperties>()));
                    foreach (Def allDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        PawnCrafterProperties modExtension = allDef.GetModExtension<PawnCrafterProperties>();
                        if (modExtension != null)
                        {
                            foreach (ThingDef disabledRace in modExtension.disabledRaces)
                            {
                                ThingDef raceDef = disabledRace;
                                RaceUtility.alienRaceKindsint.RemoveAll((Predicate<PawnKindDef>)(def => def.race == raceDef));
                            }
                        }
                    }
                    if (RaceUtility.alienRaceKindsint.Count > 1)
                        RaceUtility.alienRacesFoundint = true;
                    RaceUtility.alienRaceKindSearchDoneint = true;
                }
                return (IEnumerable<PawnKindDef>)RaceUtility.alienRaceKindsint;
            }
        }

        public static bool IsAndroid(this Pawn pawn) => pawn.def == ThingDefOf.ChjAndroid || pawn.health.hediffSet.HasHediff(HediffDefOf.ChjAndroidLike);
    }
}
