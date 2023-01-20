// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeCommand_Hediffs
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Androids
{
  public class UpgradeCommand_Hediffs : UpgradeCommand
  {
    public List<Hediff> appliedHediffs = new List<Hediff>();

    public override void Apply(Pawn customTarget = null)
    {
      Pawn pawn = customTarget == null ? this.customizationWindow.newAndroid : customTarget;
      if (customTarget == null && this.customizationWindow == null)
      {
        Log.Error("customizationWindow is null! Impossible to add Hediffs without it.");
      }
      else
      {
        if (this.def.hediffs.Count <= 0)
          return;
        foreach (HediffApplication hediff1 in this.def.hediffs)
        {
          Hediff hediff2 = HediffMaker.MakeHediff(hediff1.def, pawn);
          hediff2.Severity = hediff1.severity;
          this.appliedHediffs.Add(hediff2);
          BodyPartRecord part = (BodyPartRecord) null;
          if (hediff1.part != null)
            part = pawn.def.race.body.GetPartsWithDef(hediff1.part).FirstOrDefault<BodyPartRecord>();
          pawn.health.AddHediff(hediff2, part);
        }
      }
    }

    public override string GetExplanation()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.def.hediffToApply.ConcreteExample.TipStringExtra);
      return stringBuilder.ToString();
    }

    public override void Undo()
    {
      if (this.customizationWindow == null)
      {
        Log.Error("customizationWindow is null! Impossible to remove Hediffs without it.");
      }
      else
      {
        foreach (Hediff appliedHediff in this.appliedHediffs)
          this.customizationWindow.newAndroid.health.RemoveHediff(appliedHediff);
        this.appliedHediffs.Clear();
      }
    }
  }
}
