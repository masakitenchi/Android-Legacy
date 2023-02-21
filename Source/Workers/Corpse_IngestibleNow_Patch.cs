// Decompiled with JetBrains decompiler
// Type: Androids.Corpse_IngestibleNow_Patch
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using HarmonyLib;
using Verse;

namespace Androids
{
  [HarmonyPatch(typeof (Corpse), "get_IngestibleNow")]
  public class Corpse_IngestibleNow_Patch
  {
    [HarmonyPostfix]
    public static void Listener(Corpse __instance, ref bool __result)
    {
      if (!__instance.InnerPawn.IsAndroid() && !__instance.InnerPawn.def.HasModExtension<MechanicalPawnProperties>())
        return;
      __result = false;
    }
  }
}
