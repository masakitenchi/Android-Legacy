// Decompiled with JetBrains decompiler
// Type: Androids.AndroidUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using AlienRace;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Androids
{
  public static class AndroidUtility
  {
        public static void Androidify(Pawn pawn)
        {
            ThingDef_AlienRace chjAndroid = ThingDefOf.ChjAndroid as ThingDef_AlienRace;
            pawn.TryGetComp<AlienPartGenerator.AlienComp>();
            PortraitsCache.SetDirty(pawn);
            PortraitsCache.PortraitsCacheUpdate();
            pawn.health.AddHediff(HediffDefOf.ChjAndroidLike);
            foreach (Hediff hediff1 in pawn.health.hediffSet.hediffs.ToList<Hediff>())
            {
                Hediff hediff = hediff1;
                if (hediff is Hediff_Injury hd && hd.IsPermanent())
                {
                    pawn.health.hediffSet.hediffs.Remove((Hediff)hd);
                    hd.PostRemoved();
                    pawn.health.Notify_HediffChanged((Hediff)null);
                }
                else if (pawn.def.race.hediffGiverSets.Any<HediffGiverSetDef>((Predicate<HediffGiverSetDef>)(setDef => setDef.hediffGivers.Any<HediffGiver>((Predicate<HediffGiver>)(hediffGiver =>
                {
                    if (!(hediffGiver is HediffGiver_Birthday hediffGiverBirthday2) || hediffGiverBirthday2.hediff != hediff.def)
                        return false;
                    List<HediffStage> stages = hediffGiverBirthday2.hediff.stages;
                    return stages != null && stages.Any<HediffStage>((Predicate<HediffStage>)(stage =>
                    {
                        List<PawnCapacityModifier> capMods = stage.capMods;
                        if ((capMods != null ? (capMods.Any<PawnCapacityModifier>((Predicate<PawnCapacityModifier>)(cap => (double)cap.offset < 0.0)) ? 1 : 0) : 0) == 0 && !stage.lifeThreatening && (double)stage.partEfficiencyOffset >= 0.0)
                        {
                            List<StatModifier> statOffsets = stage.statOffsets;
                            if ((statOffsets != null ? (statOffsets.Any<StatModifier>((Predicate<StatModifier>)(stat => (double)stat.value < 0.0)) ? 1 : 0) : 0) == 0 && (double)stage.painOffset <= 0.0)
                                return (double)stage.painFactor > 1.0;
                        }
                        return true;
                    }));
                })))))
                {
                    pawn.health.hediffSet.hediffs.Remove(hediff);
                    hediff.PostRemoved();
                    pawn.health.Notify_HediffChanged((Hediff)null);
                }
            }
        }
    }
}
