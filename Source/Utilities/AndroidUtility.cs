// Decompiled with JetBrains decompiler
// Type: Androids.AndroidUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using AlienRace;

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
            foreach (Hediff hediff1 in pawn.health.hediffSet.hediffs.ToList())
            {
                Hediff hediff = hediff1;
                if (hediff is Hediff_Injury hd && hd.IsPermanent())
                {
                    pawn.health.hediffSet.hediffs.Remove(hd);
                    hd.PostRemoved();
                    pawn.health.Notify_HediffChanged(null);
                }
                else if (pawn.def.race.hediffGiverSets.Any(setDef => setDef.hediffGivers.Any(hediffGiver =>
                {
                    if (!(hediffGiver is HediffGiver_Birthday hediffGiverBirthday2) || hediffGiverBirthday2.hediff != hediff.def)
                        return false;
                    List<HediffStage> stages = hediffGiverBirthday2.hediff.stages;
                    return stages != null && stages.Any(stage =>
                    {
                        List<PawnCapacityModifier> capMods = stage.capMods;
                        if ((capMods != null ? (capMods.Any(cap => cap.offset < 0.0) ? 1 : 0) : 0) == 0 && !stage.lifeThreatening && stage.partEfficiencyOffset >= 0.0)
                        {
                            List<StatModifier> statOffsets = stage.statOffsets;
                            if ((statOffsets != null ? (statOffsets.Any(stat => stat.value < 0.0) ? 1 : 0) : 0) == 0 && stage.painOffset <= 0.0)
                                return stage.painFactor > 1.0;
                        }
                        return true;
                    });
                })))
                {
                    pawn.health.hediffSet.hediffs.Remove(hediff);
                    hediff.PostRemoved();
                    pawn.health.Notify_HediffChanged(null);
                }
            }
        }
    }
}
