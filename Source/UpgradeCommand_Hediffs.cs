// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeCommand_Hediffs
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
            Pawn pawn = customTarget ?? this.customizationWindow.newAndroid;
            if (customTarget == null && this.customizationWindow == null)
            {
                Log.Error("customizationWindow is null! Impossible to add Hediffs without it.");
            }
            else
            {
                if (this.def.hediffs.Count <= 0)
                    return;
                //Now double-check body groups and body parts for precision.
                foreach (HediffApplication hediff1 in this.def.hediffs)
                {
                    //Log.Warning($"Hediff:{hediff1.def.defName}");
                    foreach (BodyPartRecord notMissingPart in pawn.health.hediffSet.GetNotMissingParts(depth: BodyPartDepth.Outside))
                    {
                        //Log.Warning($"Part:{notMissingPart}\nGroup:{string.Join("\n", notMissingPart.groups)}\nHediff1.group:{hediff1.group}");
                        if (notMissingPart.IsInGroup(hediff1.group) && notMissingPart.def == hediff1.part)
                        {
                            Hediff hediff = HediffMaker.MakeHediff(hediff1.def, pawn, notMissingPart);
                            hediff.Severity = hediff1.severity;
                            this.appliedHediffs.Add(hediff);
                            pawn.health.AddHediff(hediff);
                            break;
                        }
                    }
                    //Hediff hediff2 = HediffMaker.MakeHediff(hediff1.def, pawn);
                    //hediff2.Severity = hediff1.severity;
                    //this.appliedHediffs.Add(hediff2);
                    //BodyPartRecord part = (BodyPartRecord)null;
                    //if (hediff1.part != null)
                    //    part = pawn.def.race.body.GetPartsWithDef(hediff1.part).FirstOrDefault<BodyPartRecord>();
                    //pawn.health.AddHediff(hediff2, part);
                }
                //foreach (HediffApplication hediff1 in this.def.hediffs)
                //{
                //    Log.Warning($"pawn:{pawn.Name}\nrace:{pawn.def.race}");
                //    Log.Warning($"Hediff:{hediff1.def.defName}\nGroup:{hediff1.group}");
                //    hediff1.part.parts
                //    //Log.Warning($"Hediff:{hediff1.def.defName}");
                //    //Hediff hediff2 = HediffMaker.MakeHediff(hediff1.def, pawn);
                //    //hediff2.Severity = hediff1.severity;
                //    //BodyPartRecord part = (BodyPartRecord) null;
                //    //if (hediff1.part != null)
                //    //{
                //    //    part = new BodyPartRecord();
                //    //    part.def = hediff1.part;
                //    //}
                //    //this.appliedHediffs.Add(pawn.health.AddHediff(hediff2.def, part));
                //}
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
