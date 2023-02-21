// Decompiled with JetBrains decompiler
// Type: Androids.EnergySource_SolarComp
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Androids
{
  public class EnergySource_SolarComp : EnergySourceComp
  {
    public override void RechargeEnergyNeed(Pawn targetPawn)
    {
      if (((double) GenLocalDate.DayPercent((Thing) targetPawn) < 0.20000000298023224 ? 1 : ((double) GenLocalDate.DayPercent((Thing) targetPawn) > 0.699999988079071 ? 1 : 0)) != 0 || targetPawn.InContainerEnclosed || !targetPawn.IsCaravanMember() && targetPawn.Position.Roofed(targetPawn.Map))
        return;
      Need_Energy need = targetPawn.needs.TryGetNeed<Need_Energy>();
      if (need == null)
        return;
      need.CurLevel += this.EnergyProps.passiveEnergyGeneration;
    }
  }
}
