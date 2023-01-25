// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeCommand_Hediff
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Collections.Generic;
using System.Text;
using Verse;

namespace Androids
{
  public class UpgradeCommand_Hediff : UpgradeCommand
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
        if (this.def.hediffToApply == null)
          return;
        if (this.def.partsToApplyTo != null)
        {
          foreach (BodyPartGroupDef group in this.def.partsToApplyTo)
          {
            foreach (BodyPartRecord notMissingPart in pawn.health.hediffSet.GetNotMissingParts(depth: BodyPartDepth.Outside))
            {
              if (notMissingPart.IsInGroup(group) && (this.def.partsDepth == BodyPartDepth.Undefined || notMissingPart.depth == this.def.partsDepth))
              {
                Hediff hediff = HediffMaker.MakeHediff(this.def.hediffToApply, pawn, notMissingPart);
                hediff.Severity = this.def.hediffSeverity;
                this.appliedHediffs.Add(hediff);
                pawn.health.AddHediff(hediff);
              }
            }
          }
        }
        else
        {
          Hediff hediff = HediffMaker.MakeHediff(this.def.hediffToApply, pawn);
          hediff.Severity = this.def.hediffSeverity;
          this.appliedHediffs.Add(hediff);
          pawn.health.AddHediff(hediff);
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
