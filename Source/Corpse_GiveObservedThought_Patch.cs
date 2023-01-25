// Decompiled with JetBrains decompiler
// Type: Androids.Corpse_GiveObservedThought_Patch
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using HarmonyLib;
using RimWorld;
using Verse;

namespace Androids
{
  [HarmonyPatch(typeof (Corpse), "GiveObservedThought")]
  public class Corpse_GiveObservedThought_Patch
  {
    [HarmonyPostfix]
    public static void Listener(Corpse __instance, ref Thought_Memory __result)
    {
      if (__instance.InnerPawn == null || !__instance.InnerPawn.def.HasModExtension<MechanicalPawnProperties>())
        return;
      __result = (Thought_Memory) null;
    }
  }
}
