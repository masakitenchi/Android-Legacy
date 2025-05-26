// Decompiled with JetBrains decompiler
// Type: Androids.ThoughtWorker_Expectations_CurrentStateInternal_Patch
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    [HarmonyPatch(typeof(ThoughtWorker_Expectations), "CurrentStateInternal")]
    public class ThoughtWorker_Expectations_CurrentStateInternal_Patch
    {
        [HarmonyPostfix]
        public static void Listener(Pawn p, ref ThoughtState __result)
        {
            if (p == null || !p.def.HasModExtension<MechanicalPawnProperties>())
                return;
            __result = ThoughtState.ActiveAtStage(5);
        }
    }
}
