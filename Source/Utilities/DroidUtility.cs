// Decompiled with JetBrains decompiler
// Type: Androids.DroidUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using AlienRace;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Androids
{
    /// <summary>
    /// Utility creating and modifying Droids.
    /// </summary>
    public static class DroidUtility
    {
        private static List<Hediff> tmpHediffsToTend = new List<Hediff>();
        private static List<Hediff> tmpHediffs = new List<Hediff>();
        private static readonly long ageInTicks = 64800000;

        /// <summary>
        /// Creates a Droid template.
        /// </summary>
        /// <param name="raceDef">ThingDef to use as race.</param>
        /// <param name="pawnKindDef">PawnKindDef to use as kind.</param>
        /// <param name="faction">Faction that owns this Droid.</param>
        /// <param name="map">Map to spawn in.</param>
        /// <returns>New Pawn if successful. Null if not.</returns>
        public static Pawn MakeDroidTemplate(
          PawnKindDef pawnKindDef,
          Faction faction,
          int tile,
          List<SkillRequirement> skills = null,
          int defaultSkillLevel = 6)
        {
            Map map = (Map)null;
            if (tile > -1)
                map = Current.Game?.FindMap(tile);
            Pawn pawnBeingCrafted = (Pawn)ThingMaker.MakeThing(pawnKindDef.race);
            if (pawnBeingCrafted == null)
                return (Pawn)null;
            pawnBeingCrafted.kindDef = pawnKindDef;
            if (faction != null)
                pawnBeingCrafted.SetFactionDirect(faction);
            PawnComponentsUtility.CreateInitialComponents(pawnBeingCrafted);
            pawnBeingCrafted.gender = Gender.Male;
            pawnBeingCrafted.needs.SetInitialLevels();
            pawnBeingCrafted.ageTracker.AgeBiologicalTicks = ageInTicks;
            pawnBeingCrafted.ageTracker.AgeChronologicalTicks = ageInTicks;
            if (pawnBeingCrafted.RaceProps.Humanlike)
            {
                DroidSpawnProperties spawnProperties = pawnKindDef.race.GetModExtension<DroidSpawnProperties>();
                if (spawnProperties != null)
                {
                    pawnBeingCrafted.gender = spawnProperties.gender;
                    pawnBeingCrafted.playerSettings.hostilityResponse = spawnProperties.hostileResponse;
                }
                pawnBeingCrafted.style.beardDef = BeardDefOf.NoBeard;
                pawnBeingCrafted.style.BodyTattoo = TattooDefOf.NoTattoo_Body;
                pawnBeingCrafted.style.FaceTattoo = TattooDefOf.NoTattoo_Face;
                pawnBeingCrafted.story.bodyType = spawnProperties?.bodyType ?? BodyTypeDefOf.Thin;
                if (spawnProperties != null && spawnProperties.headType != null)
                    pawnBeingCrafted.story.headType = spawnProperties.headType;
                PortraitsCache.SetDirty(pawnBeingCrafted);
                pawnBeingCrafted.story.Childhood = spawnProperties?.backstory ?? DefDatabase<BackstoryDef>.AllDefsListForReading.Find(backstoryDef => backstoryDef.defName == "ChJAndroid_Droid");
                //Currently Rewrote until here.
                if (skills == null || skills.Count <= 0)
                {
                    if (spawnProperties != null)
                    {
                        foreach (SkillDef skillDef in DefDatabase<SkillDef>.AllDefsListForReading)
                            pawnBeingCrafted.skills.GetSkill(skillDef).Level = spawnProperties.defaultSkillLevel;
                        foreach (DroidSkill skill1 in spawnProperties.skills)
                        {
                            SkillRecord skill2 = pawnBeingCrafted.skills.GetSkill(skill1.def);
                            if (skill2 != null)
                            {
                                skill2.Level = skill1.level;
                                skill2.passion = skill1.passion;
                            }
                        }
                    }
                    else
                    {
                        List<SkillDef> defsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
                        for (int index = 0; index < defsListForReading.Count; ++index)
                        {
                            SkillDef skillDef = defsListForReading[index];
                            SkillRecord skill = pawnBeingCrafted.skills.GetSkill(skillDef);
                            if (skillDef == SkillDefOf.Shooting || skillDef == SkillDefOf.Melee || skillDef == SkillDefOf.Mining || skillDef == SkillDefOf.Plants)
                            {
                                skill.Level = 8;
                            }
                            else
                            {
                                int num3 = skillDef == SkillDefOf.Medicine || skillDef == SkillDefOf.Crafting ? 1 : (skillDef == SkillDefOf.Cooking ? 1 : 0);
                                skill.Level = num3 == 0 ? 6 : 4;
                            }
                            skill.passion = Passion.None;
                        }
                    }
                }
                else
                {
                    List<SkillDef> defsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
                    for (int index = 0; index < defsListForReading.Count; ++index)
                    {
                        SkillDef skillDef = defsListForReading[index];
                        SkillRecord skill = pawnBeingCrafted.skills.GetSkill(skillDef);
                        SkillRequirement skillRequirement = skills.First<SkillRequirement>((Func<SkillRequirement, bool>)(sr => sr.skill == skillDef));
                        skill.Level = skillRequirement == null ? defaultSkillLevel : skillRequirement.minLevel;
                        skill.passion = Passion.None;
                    }
                }
            }
            if (pawnBeingCrafted.workSettings != null)
                pawnBeingCrafted.workSettings.EnableAndInitialize();
            if (map != null && faction.IsPlayer)
            {
                IEnumerable<Name> source = map.mapPawns.FreeColonists.Select<Pawn, Name>((Func<Pawn, Name>)(pawn => pawn.Name));
                if (source != null)
                {
                    int num4 = source.Count<Name>((Func<Name, bool>)(name => name.ToStringShort.ToLower().StartsWith(pawnKindDef.race.label.ToLower())));
                    string nickName = (string)(pawnKindDef.race.LabelCap + " ") + num4.ToString();
                    pawnBeingCrafted.Name = (Name)DroidUtility.MakeDroidName(nickName);
                }
                else
                    pawnBeingCrafted.Name = (Name)DroidUtility.MakeDroidName((string)null);
            }
            else
                pawnBeingCrafted.Name = (Name)DroidUtility.MakeDroidName((string)null);
            return pawnBeingCrafted;
        }

        public static float HairChoiceLikelihoodFor(HairDef hair, Pawn pawn)
        {
            float num;
            if (pawn.gender == Gender.None)
            {
                num = 100f;
            }
            else
            {
                if (pawn.gender == Gender.Male)
                {
                    switch (hair.styleGender)
                    {
                        case StyleGender.Male:
                            return 70f;
                        case StyleGender.MaleUsually:
                            return 30f;
                        case StyleGender.Any:
                            return 60f;
                        case StyleGender.FemaleUsually:
                            return 5f;
                        case StyleGender.Female:
                            return 1f;
                    }
                }
                if (pawn.gender == Gender.Female)
                {
                    switch (hair.styleGender)
                    {
                        case StyleGender.Male:
                            return 1f;
                        case StyleGender.MaleUsually:
                            return 5f;
                        case StyleGender.Any:
                            return 60f;
                        case StyleGender.FemaleUsually:
                            return 30f;
                        case StyleGender.Female:
                            return 70f;
                    }
                }
                Log.Error("Unknown hair likelihood for " + (object)hair + " with " + (object)pawn);
                num = 0.0f;
            }
            return num;
        }

        public static Pawn MakeCustomDroid(PawnKindDef pawnKind, Faction faction)
        {
            PawnKindDef kind = pawnKind;
            Faction faction1 = faction;
            float? nullable1 = new float?(0.0f);
            float? nullable2 = new float?(0.0f);
            float? minChanceToRedressWorldPawn = new float?();
            float? fixedBiologicalAge = nullable1;
            float? fixedChronologicalAge = nullable2;
            Gender? fixedGender = new Gender?();
            FloatRange? excludeBiologicalAgeRange = new FloatRange?();
            FloatRange? biologicalAgeRange = new FloatRange?();
            return PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction1, minChanceToRedressWorldPawn: minChanceToRedressWorldPawn, fixedBiologicalAge: fixedBiologicalAge, fixedChronologicalAge: fixedChronologicalAge, fixedGender: fixedGender, excludeBiologicalAgeRange: excludeBiologicalAgeRange, biologicalAgeRange: biologicalAgeRange));
        }

        public static NameTriple MakeDroidName(string nickName)
        {
            string str = string.Format("D-{0:X}-{1:X}", (object)Rand.Range(0, 256), (object)Rand.Range(0, 256));
            return nickName == null ? new NameTriple(str, str, " ") : new NameTriple(str, nickName, " ");
        }

        public static void DoTend(Pawn doctor, Pawn patient, Thing medicine)
        {
            if (!patient.health.HasHediffsNeedingTend())
                return;
            if (medicine != null && medicine.Destroyed)
            {
                Log.Warning("Tried to use destroyed repair kit.");
                medicine = (Thing)null;
            }
            DroidUtility.GetOptimalHediffsToTendWithSingleTreatment(patient, medicine != null, DroidUtility.tmpHediffsToTend);
            for (int index = 0; index < DroidUtility.tmpHediffsToTend.Count; ++index)
            {
                if (medicine == null)
                    DroidUtility.tmpHediffsToTend[index].Tended(0.1f, (float)index);
                else
                    patient.health.RemoveHediff(DroidUtility.tmpHediffsToTend[index]);
            }
            if (doctor != null && doctor.Faction == Faction.OfPlayer && patient.Faction != doctor.Faction && !patient.IsPrisoner && patient.Faction != null)
                ++patient.mindState.timesGuestTendedToByPlayer;
            if (doctor != null && doctor.RaceProps.Humanlike && patient.RaceProps.Animal && RelationsUtility.TryDevelopBondRelation(doctor, patient, 0.004f) && doctor.Faction != null && doctor.Faction != patient.Faction)
                InteractionWorker_RecruitAttempt.DoRecruit(doctor, patient, false);
            patient.records.Increment(RecordDefOf.TimesTendedTo);
            doctor?.records.Increment(RecordDefOf.TimesTendedOther);
            if (doctor == patient && !doctor.Dead)
                doctor.mindState.Notify_SelfTended();
            if (medicine != null)
            {
                if (patient.Spawned || doctor != null && doctor.Spawned)
                    SoundDefOf.Building_Complete.PlayOneShot((SoundInfo)new TargetInfo(patient.Position, patient.Map));
                if (medicine.stackCount > 1)
                    --medicine.stackCount;
                else if (!medicine.Destroyed)
                    medicine.Destroy();
            }
            if (ModsConfig.IdeologyActive && doctor != null && doctor.Ideo != null)
            {
                Precept_Role role = doctor.Ideo.GetRole(doctor);
                if (role != null && role.def.roleEffects != null)
                {
                    foreach (RoleEffect roleEffect in role.def.roleEffects)
                        roleEffect.Notify_Tended(doctor, patient);
                }
            }
            if (doctor != null && doctor.Faction == Faction.OfPlayer && doctor != patient)
                QuestUtility.SendQuestTargetSignals(patient.questTags, "PlayerTended", patient.Named("SUBJECT"));
        }

        public static void GetOptimalHediffsToTendWithSingleTreatment(
          Pawn patient,
          bool usingMedicine,
          List<Hediff> outHediffsToTend,
          List<Hediff> tendableHediffsInTendPriorityOrder = null)
        {
            outHediffsToTend.Clear();
            DroidUtility.tmpHediffs.Clear();
            if (tendableHediffsInTendPriorityOrder != null)
            {
                DroidUtility.tmpHediffs.AddRange((IEnumerable<Hediff>)tendableHediffsInTendPriorityOrder);
            }
            else
            {
                List<Hediff> hediffs = patient.health.hediffSet.hediffs;
                for (int index = 0; index < hediffs.Count; ++index)
                {
                    if (hediffs[index].TendableNow())
                        DroidUtility.tmpHediffs.Add(hediffs[index]);
                }
                TendUtility.SortByTendPriority(DroidUtility.tmpHediffs);
            }
            if (!DroidUtility.tmpHediffs.Any<Hediff>())
                return;
            Hediff tmpHediff1 = DroidUtility.tmpHediffs[0];
            outHediffsToTend.Add(tmpHediff1);
            HediffCompProperties_TendDuration propertiesTendDuration = tmpHediff1.def.CompProps<HediffCompProperties_TendDuration>();
            if (propertiesTendDuration != null && propertiesTendDuration.tendAllAtOnce)
            {
                for (int index = 0; index < DroidUtility.tmpHediffs.Count; ++index)
                {
                    if (DroidUtility.tmpHediffs[index] != tmpHediff1 && DroidUtility.tmpHediffs[index].def == tmpHediff1.def)
                        outHediffsToTend.Add(DroidUtility.tmpHediffs[index]);
                }
            }
            else if (tmpHediff1 is Hediff_Injury & usingMedicine)
            {
                float severity1 = tmpHediff1.Severity;
                for (int index = 0; index < DroidUtility.tmpHediffs.Count; ++index)
                {
                    if (DroidUtility.tmpHediffs[index] != tmpHediff1 && DroidUtility.tmpHediffs[index] is Hediff_Injury tmpHediff2)
                    {
                        float severity2 = tmpHediff2.Severity;
                        if ((double)severity1 + (double)severity2 <= 20.0)
                        {
                            severity1 += severity2;
                            outHediffsToTend.Add((Hediff)tmpHediff2);
                        }
                    }
                }
            }
            DroidUtility.tmpHediffs.Clear();
        }
    }
}
