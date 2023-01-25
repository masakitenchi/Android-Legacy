// Decompiled with JetBrains decompiler
// Type: Androids.HediffGiver_Machine
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Verse;

namespace Androids
{
  public class HediffGiver_Machine : HediffGiver
  {
    public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
    {
      if (!hediff.def.makesSickThought || hediff.Bleeding)
        return true;
      pawn.health.RemoveHediff(hediff);
      return false;
    }
  }
}
