// Decompiled with JetBrains decompiler
// Type: Androids.DeathActionWorker_Android
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using Androids.Integration;
using Verse;

namespace Androids
{
    public class DeathActionWorker_Android : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            if (!AndroidsModSettings.Instance.androidExplodesOnDeath)
                return;
            Pawn pawn = corpse.InnerPawn;
            EnergyTrackerComp energy = pawn.TryGetComp<EnergyTrackerComp>();
            if (energy != null)
            {
                //Make sure we did not die from natural causes. As natural as an Android can be.
                bool shouldBeDeadByNaturalCauses = pawn.health.hediffSet.hediffs.Any(hediff => hediff.CauseDeathNow());

                Hediff overheatingHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjOverheating);
                //Overheating death is excepted.
                if (overheatingHediff != null && !shouldBeDeadByNaturalCauses)
                {
                    float explosionRadius = overheatingHediff.Severity * AndroidsModSettings.Instance.androidExplosionRadius * energy.energy;

                    //if (deadFromOverheating)
                    //    explosionRadius *= 2;

                    //Scale explosion strength from how much remaining energy we got.
                    if (explosionRadius >= 1f)
                    {
                        GenExplosion.DoExplosion(corpse.Position, corpse.Map, explosionRadius, RimWorld.DamageDefOf.Bomb, corpse.InnerPawn);
                    }
                }
            }
            else
            {
                //Attempt to get the energy need directly.
                if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjAndroidLike) is AndroidLikeHediff androidLikeForReal)
                {
                    //Make sure we did not die from natural causes. As natural as an Android can be.
                    bool shouldBeDeadByNaturalCauses = pawn.health.hediffSet.hediffs.Any(hediff => hediff.CauseDeathNow());

                    Hediff overheatingHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjOverheating);
                    //bool deadFromOverheating = overheatingHediff != null ? overheatingHediff.Severity >= 1f : false;

                    if (overheatingHediff == null)
                        return;

                    //Overheating death is excepted.
                    if (overheatingHediff != null && !shouldBeDeadByNaturalCauses)
                    {
                        float explosionRadius = overheatingHediff.Severity * AndroidsModSettings.Instance.androidExplosionRadius * androidLikeForReal.energyTracked;

                        //if (deadFromOverheating)
                        //    explosionRadius *= 2;

                        //Scale explosion strength from how much remaining energy we got.
                        if (overheatingHediff != null && explosionRadius >= 1f)
                            GenExplosion.DoExplosion(corpse.Position, corpse.Map, explosionRadius, RimWorld.DamageDefOf.Bomb, corpse.InnerPawn);
                    }
                    return;
                }
                else
                {
                    Log.Warning("Androids.DeathActionWorker_Android: EnergyTrackerComp is null at or is not Android Like either: " + corpse.ThingID);
                    return;
                }
            }
        }
    }
}
