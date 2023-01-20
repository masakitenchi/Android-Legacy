// Decompiled with JetBrains decompiler
// Type: Androids.Hediff_VanometricCell
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using Verse;

namespace Androids
{
  public class Hediff_VanometricCell : HediffWithComps
  {
    public override void Tick()
    {
      base.Tick();
      Need_Food food = this.pawn?.needs?.food;
      if (food != null)
        food.CurLevel = food.MaxLevel;
      Need_Energy need = this.pawn?.needs?.TryGetNeed<Need_Energy>();
      if (need == null)
        return;
      need.CurLevel = need.MaxLevel;
    }

    public override string TipStringExtra => (string) "AndroidHediffVanometricCell".Translate();
  }
}
