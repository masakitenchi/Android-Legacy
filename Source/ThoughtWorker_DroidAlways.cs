// Decompiled with JetBrains decompiler
// Type: Androids.ThoughtWorker_DroidAlways
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using Verse;

namespace Androids
{
  public class ThoughtWorker_DroidAlways : ThoughtWorker
  {
    protected override ThoughtState CurrentStateInternal(Pawn p) => p.def.HasModExtension<MechanicalPawnProperties>() || p.health.hediffSet.HasHediff(HediffDefOf.ChjAndroidUpgrade_DroneCore) ? ThoughtState.ActiveAtStage(0) : ThoughtState.Inactive;
  }
}
