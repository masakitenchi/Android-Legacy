// Decompiled with JetBrains decompiler
// Type: Androids.Hediff_MechaniteHive
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using Verse;

namespace Androids
{
  public class Hediff_MechaniteHive : HediffWithComps
  {
    public override void Tick()
    {
      base.Tick();
      if (!this.pawn.IsHashIntervalTick(2000))
        return;
      foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
      {
        if (hediff is Hediff_Injury hediffInjury && hediffInjury.Bleeding)
          hediffInjury.Tended(1f, 1f, 0);
      }
    }

    public override string TipStringExtra => (string) "AndroidMechaniteHive".Translate();
  }
}
