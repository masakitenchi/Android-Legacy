// Decompiled with JetBrains decompiler
// Type: Androids.ThoughtWorker_DroidAlways
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    public class ThoughtWorker_DroidAlways : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p) => p.def.HasModExtension<MechanicalPawnProperties>() || p.health.hediffSet.HasHediff(HediffDefOf.ChjAndroidUpgrade_DroneCore) ? ThoughtState.ActiveAtStage(0) : ThoughtState.Inactive;
    }
}
