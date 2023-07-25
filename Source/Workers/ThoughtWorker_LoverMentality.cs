// Decompiled with JetBrains decompiler
// Type: Androids.ThoughtWorker_LoverMentality
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using Verse;

namespace Androids
{
    public class ThoughtWorker_LoverMentality : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn) => p.health.hediffSet.HasHediff(this.def.hediff) && p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff) is Hediff_LoverMentality firstHediffOfDef && firstHediffOfDef.loverToChase == otherPawn ? (ThoughtState)true : (ThoughtState)false;
    }
}
