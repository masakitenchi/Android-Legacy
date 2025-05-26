// Decompiled with JetBrains decompiler
// Type: Androids.WorkGiver_Tend_HasJobOnThing_IgnoreMedPods
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    [HarmonyPatch(typeof(WorkGiver_Tend), "HasJobOnThing")]
    internal static class WorkGiver_Tend_HasJobOnThing_IgnoreMedPods
    {
        private static void Postfix(ref bool __result, Pawn pawn, Thing t, bool forced = false)
        {
            Pawn patient = t as Pawn;
            if (!patient.def.HasModExtension<MechanicalPawnProperties>() || HealthAIUtility.FindBestMedicine(pawn, patient) != null)
                return;
            __result = false;
        }
    }
}
