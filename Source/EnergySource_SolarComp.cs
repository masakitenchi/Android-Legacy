// Decompiled with JetBrains decompiler
// Type: Androids.EnergySource_SolarComp
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Androids
{
  public class EnergySource_SolarComp : EnergySourceComp
  {
    public override void RechargeEnergyNeed(Pawn targetPawn)
    {
      if ((double) GenLocalDate.DayPercent((Thing) targetPawn) < 0.20000000298023224 || (double) GenLocalDate.DayPercent((Thing) targetPawn) > 0.699999988079071 || targetPawn.InContainerEnclosed || !targetPawn.IsCaravanMember() && targetPawn.Position.Roofed(targetPawn.Map))
        return;
      Need_Energy need = targetPawn.needs.TryGetNeed<Need_Energy>();
      if (need == null)
        return;
      need.CurLevel += this.EnergyProps.passiveEnergyGeneration;
    }
  }
}
