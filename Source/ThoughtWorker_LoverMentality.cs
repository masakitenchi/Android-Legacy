// Decompiled with JetBrains decompiler
// Type: Androids.ThoughtWorker_LoverMentality
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using Verse;

namespace Androids
{
  public class ThoughtWorker_LoverMentality : ThoughtWorker
  {
    protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn) => p.health.hediffSet.HasHediff(this.def.hediff) && p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff) is Hediff_LoverMentality firstHediffOfDef && firstHediffOfDef.loverToChase == otherPawn ? (ThoughtState) true : (ThoughtState) false;
  }
}
