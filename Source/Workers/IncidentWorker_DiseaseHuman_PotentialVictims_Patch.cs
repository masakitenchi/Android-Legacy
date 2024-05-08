// Decompiled with JetBrains decompiler
// Type: Androids.IncidentWorker_DiseaseHuman_PotentialVictims_Patch
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Androids
{
    [HarmonyPatch(typeof(IncidentWorker_DiseaseHuman), "PotentialVictimCandidates")]
    public class IncidentWorker_DiseaseHuman_PotentialVictims_Patch
    {
        [HarmonyPostfix]
        public static void Listener(IIncidentTarget target, ref IEnumerable<Pawn> __result)
        {
            if (__result == null)
                return;
            List<Pawn> pawnList = new List<Pawn>();
            foreach (Pawn pawn in __result)
            {
                if (!pawn.def.HasModExtension<MechanicalPawnProperties>() || !pawn.IsAndroid())
                    pawnList.Add(pawn);
            }
            __result = pawnList;
        }
    }
}
