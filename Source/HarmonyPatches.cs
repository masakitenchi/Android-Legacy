// Decompiled with JetBrains decompiler
// Type: Androids.HarmonyPatches
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

global using HarmonyLib;
global using RimWorld;
global using RimWorld.Planet;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Reflection;
global using System.Reflection.Emit;
global using UnityEngine;
global using Verse;
global using Verse.AI;
global using LudeonTK;
using Androids.Integration;

namespace Androids
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        public static FieldInfo int_Pawn_NeedsTracker_GetPawn;
        public static FieldInfo int_PawnRenderer_GetPawn;
        public static FieldInfo int_ConditionalPercentageNeed_need;
        public static FieldInfo int_Pawn_HealthTracker_GetPawn;
        public static FieldInfo int_Pawn_InteractionsTracker_GetPawn;
        public static NeedDef Need_Bladder;
        public static NeedDef Need_Hygiene;
        public static bool bypassGenerationOfUpgrades = false;

        static HarmonyPatches()
        {
            Need_Bladder = DefDatabase<NeedDef>.GetNamedSilentFail("Bladder");
            Need_Hygiene = DefDatabase<NeedDef>.GetNamedSilentFail("Hygiene");
            Harmony harmony = new Harmony("ChJees.Androids");
            string str = "";
            try
            {
                str = "Pawn_NeedsTracker.ShouldHaveNeed";
				Type type1 = typeof(Pawn_NeedsTracker);
                int_Pawn_NeedsTracker_GetPawn = type1.GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                harmony.Patch(type1.GetMethod("ShouldHaveNeed", BindingFlags.Instance | BindingFlags.NonPublic), postfix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_Pawn_NeedsTracker_ShouldHaveNeed")));
                /* str = "PawnRenderer.RenderPawnInternal";
                System.Type type2 = typeof(PawnRenderer);
                HarmonyPatches.int_PawnRenderer_GetPawn = type2.GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                harmony.Patch((MethodBase)type2.GetMethod("RenderPawnInternal", BindingFlags.Instance | BindingFlags.NonPublic, System.Type.DefaultBinder, CallingConventions.Any, new System.Type[6]
                {
          typeof (Vector3),
          typeof (float),
          typeof (bool),
          typeof (Rot4),
          typeof (RotDrawMode),
          typeof (PawnRenderFlags)
                }, (ParameterModifier[])null), postfix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_PawnRenderer_RenderPawnInternal"))); */
                str = "Need_Food.Starving";
				Type type3 = typeof(Need_Food);
                MethodInfo getMethod = AccessTools.PropertyGetter(typeof(Need_Food), "Starving");
                harmony.Patch(getMethod, postfix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_Need_Food_Starving_Get")));
                str = "HealthUtility.AdjustSeverity";
				Type type4 = typeof(HealthUtility);
                harmony.Patch(type4.GetMethod("AdjustSeverity"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_HealthUtility_AdjustSeverity")));
                str = "ThinkNode_ConditionalNeedPercentageAbove.Satisfied";
				Type type5 = typeof(ThinkNode_ConditionalNeedPercentageAbove);
                int_ConditionalPercentageNeed_need = type5.GetField("need", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                harmony.Patch(type5.GetMethod("Satisfied", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_ThinkNode_ConditionalNeedPercentageAbove_Satisfied")));
                str = "Pawn_HealthTracker.DropBloodFilth";
				Type type6 = typeof(Pawn_HealthTracker);
                harmony.Patch(type6.GetMethod("DropBloodFilth"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_Pawn_HealthTracker_DropBloodFilth")));
                str = "Pawn_HealthTracker.HealthTick";
				Type type7 = typeof(Pawn_HealthTracker);
                harmony.Patch(type7.GetMethod("HealthTick"), postfix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_Pawn_HealthTracker_HealthTick")));
                str = "Pawn_HealthTracker.AddHediff";
				Type type8 = typeof(Pawn_HealthTracker);
                harmony.Patch(type8.GetMethod("AddHediff", new Type[4]
                {
          typeof (Hediff),
          typeof (BodyPartRecord),
          typeof (DamageInfo),
          typeof (DamageWorker.DamageResult)
                }), postfix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_Pawn_HealthTracker_AddHediff")));
                str = "SkillRecord.Interval";
				Type type9 = typeof(SkillRecord);
                harmony.Patch(type9.GetMethod("Interval"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_SkillRecord_Interval")));
                str = "Pawn.GetGizmos";
				Type type10 = typeof(Pawn);
                harmony.Patch(type10.GetMethod("GetGizmos"), postfix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_Pawn_GetGizmos")));
                str = "HealthAIUtility.FindBestMedicine";
				Type type11 = typeof(HealthAIUtility);
                harmony.Patch(type11.GetMethod("FindBestMedicine"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_HealthAIUtility_FindBestMedicine")));
                str = "Toils_Tend.FinalizeTend";
				Type type12 = typeof(Toils_Tend);
                harmony.Patch(type12.GetMethod("FinalizeTend"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_Toils_Tend_FinalizeTend")));
                str = "DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood";
				Type type13 = typeof(DaysWorthOfFoodCalculator);
				Type[] types = new Type[9]
                {
          typeof (List<Pawn>),
          typeof (List<ThingDefCount>),
          typeof (int),
          typeof (IgnorePawnsInventoryMode),
          typeof (Faction),
          typeof (WorldPath),
          typeof (float),
          typeof (int),
          typeof (bool)
                };
                harmony.Patch(type13.GetMethod("ApproxDaysWorthOfFood", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, Type.DefaultBinder, types, null), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_DaysWorthOfFoodCalculator_ApproxDaysWorthOfFood")));
                str = "GatheringsUtility.ShouldPawnKeepPartying";
				Type type14 = typeof(GatheringsUtility);
                harmony.Patch(type14.GetMethod("ShouldGuestKeepAttendingGathering"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_PartyUtility_ShouldPawnKeepAttending")));
                str = "GatheringsUtility.EnoughPotentialGuestsToStartGathering";
				Type type15 = typeof(GatheringsUtility);
                harmony.Patch(type15.GetMethod("EnoughPotentialGuestsToStartGathering"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_PartyUtility_EnoughPotentialGuestsToStartGathering")));
                str = "ThoughtWorker_NeedFood.CurrentStateInternal";
				Type type16 = typeof(ThoughtWorker_NeedFood);
                harmony.Patch(type16.GetMethod("CurrentStateInternal", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_ThoughtWorker_NeedFood_CurrentStateInternal")));
                str = "PawnGenerator.TryGenerateNewPawnInternal";
				Type type17 = typeof(PawnGenerator);
                harmony.Patch(AccessTools.Method(type17, "TryGenerateNewPawnInternal"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_PawnGenerator_TryGenerateNewPawnInternal")), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_PawnGenerator_TryGenerateNewPawnInternal_Post")));
                str = "FoodUtility.WillIngestStackCountOf";
				Type type18 = typeof(FoodUtility);
                harmony.Patch(type18.GetMethod("WillIngestStackCountOf"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_WillIngestStackCountOf")));
                str = "RecordWorker_TimeInBedForMedicalReasons.ShouldMeasureTimeNow";
				Type type19 = typeof(RecordWorker_TimeInBedForMedicalReasons);
                harmony.Patch(type19.GetMethod("ShouldMeasureTimeNow"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_ShouldMeasureTimeNow")));
                str = "InteractionUtility.CanInitiateInteraction";
				Type type20 = typeof(InteractionUtility);
                harmony.Patch(type20.GetMethod("CanInitiateInteraction"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_CanInitiateInteraction")));
                str = "Pawn_HealthTracker.ShouldBeDeadFromRequiredCapacity";
				Type type21 = typeof(Pawn_HealthTracker);
                int_Pawn_HealthTracker_GetPawn = type21.GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                harmony.Patch(type21.GetMethod("ShouldBeDeadFromRequiredCapacity"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_ShouldBeDeadFromRequiredCapacity")));
                str = "HediffSet.CalculatePain";
				Type type22 = typeof(HediffSet);
                harmony.Patch(type22.GetMethod("CalculatePain", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_CalculatePain")));
                str = "RestUtility.TimetablePreventsLayDown";
				Type type23 = typeof(RestUtility);
                harmony.Patch(type23.GetMethod("TimetablePreventsLayDown"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_TimetablePreventsLayDown")));
                str = "GatheringsUtility.ShouldGuestKeepAttendingGathering";
				Type type24 = typeof(GatheringsUtility);
                harmony.Patch(type24.GetMethod("ShouldGuestKeepAttendingGathering"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_ShouldGuestKeepAttendingGathering")));
                str = "JobGiver_EatInGatheringArea.TryGiveJob";
				Type type25 = typeof(JobGiver_EatInGatheringArea);
                harmony.Patch(type25.GetMethod("TryGiveJob", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_EatInPartyAreaTryGiveJob")));
                str = "JobGiver_GetJoy.TryGiveJob";
				Type type26 = typeof(JobGiver_GetJoy);
                harmony.Patch(type26.GetMethod("TryGiveJob", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_GetJoyTryGiveJob")));
                str = "Pawn_InteractionsTracker.SocialFightChance && InteractionsTrackerTick && CanInteractNowWith";
				Type type27 = typeof(Pawn_InteractionsTracker);
                int_Pawn_InteractionsTracker_GetPawn = type27.GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                harmony.Patch(type27.GetMethod("SocialFightChance"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_SocialFightChance")));
                harmony.Patch(type27.GetMethod("InteractionsTrackerTick"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_InteractionsTrackerTick")));
                harmony.Patch(type27.GetMethod("CanInteractNowWith"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_CanInteractNowWith")));
                str = "InteractionUtility.CanInitiateInteraction && CanReceiveInteraction";
				Type type28 = typeof(InteractionUtility);
                harmony.Patch(type28.GetMethod("CanInitiateInteraction"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_CanDoInteraction")));
                harmony.Patch(type28.GetMethod("CanReceiveInteraction"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_CanDoInteraction")));
                str = "PawnDiedOrDownedThoughtsUtility.AppendThoughts_ForHumanlike";
				Type type29 = typeof(PawnDiedOrDownedThoughtsUtility);
                harmony.Patch(type29.GetMethod("AppendThoughts_ForHumanlike", BindingFlags.Static | BindingFlags.NonPublic), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_AppendThoughts_ForHumanlike")));
                str = "InspirationHandler.InspirationHandlerTick";
				Type type30 = typeof(InspirationHandler);
                harmony.Patch(type30.GetMethod("InspirationHandlerTick"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_InspirationHandlerTick")));
                str = "JobDriver_Vomit.MakeNewToils";
				Type type31 = typeof(JobDriver_Vomit);
                harmony.Patch(type31.GetMethod("MakeNewToils", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_VomitJob")));
                str = "Alert_Boredom.GetReport";
                /* System.Type type32 = typeof(Alert_Boredom);
                harmony.Patch(AccessTools.Method(type32, "GetReport"), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CompatPatch_Boredom_GetReport"))); */
                str = "Caravan.NightResting";
				Type type33 = typeof(Caravan);
                harmony.Patch(AccessTools.Property(type33, "NightResting").GetGetMethod(), new HarmonyMethod(typeof(HarmonyPatches).GetMethod("Patch_Caravan_NightResting")));
                str = "CompUseEffect_InstallImplantMechlink.CanBeUsedBy";
				Type type34 = typeof(CompUseEffect_InstallImplantMechlink);
                harmony.Patch(AccessTools.Method(type34, "CanBeUsedBy"), postfix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CanInstallMechLinkPostfix")));
                str = "CompUseEffect_InstallImplantMechlink.DoEffect";
				Type type35 = typeof(CompUseEffect_InstallImplant);
                harmony.Patch(AccessTools.Method(type35, "DoEffect"), prefix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("DoEffectPrefix")));
				Type type36 = typeof(BodyDef);
                harmony.Patch(AccessTools.Method(type36, "GetPartsWithDef"), prefix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("GetPartsPrefix")));
                str = "CompAbilityEffect_BloodfeederBite.Valid";
				Type type37 = typeof(CompAbilityEffect_BloodfeederBite);
                harmony.Patch(AccessTools.Method(type37, "Valid", new Type[] {
                    typeof(LocalTargetInfo),
                        typeof(bool)
                }), postfix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("ValidPostfix")));
                str = "ThoughtWorker_LookChangeDesired.CurrentStateInternal";
				Type type39 = typeof(ThoughtWorker_LookChangeDesired);
                harmony.Patch(AccessTools.Method(type39, "CurrentStateInternal"), prefix: new HarmonyMethod(typeof(HarmonyPatches), "Patch_ThoughtWorker_LookChangeDesired"));
                str = "Pawn_StyleTracker.CanDesireLookChange_getter";
                Type type40 = typeof(Pawn_StyleTracker);
                harmony.Patch(AccessTools.PropertyGetter(type40, "CanDesireLookChange"), postfix: new HarmonyMethod(typeof(HarmonyPatches), "Patch_CanDesireLookChange_getter"));
                Type type41 = typeof(ITab_Pawn_Visitor);
                str = "ITab_Pawn_Visitor+<>c__DisplayClass8_0.<DoPrisonerTab>g__CanUsePrisonerInteractionMode|0";
                harmony.Patch(AccessTools.FirstMethod(AccessTools.Inner(type41, "<>c__DisplayClass8_0"), x => x.Name.Contains("CanUsePrisonerInteractionMode")), prefix: new HarmonyMethod(typeof(HarmonyPatches), "CanUsePrisonerInteractionMode_Prefix"));
				Type xenogerm = typeof(Xenogerm);
                harmony.Patch(AccessTools.Method(xenogerm, "GetFloatMenuOptions"), prefix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CanInstallXenoGerm_Prefix")));
                str = "Xenogerm_GetFloatMenuOptions_CanInstallXenoGerm_Prefix";
				Type xenogerm3 = typeof(Xenogerm);
                harmony.Patch(AccessTools.Method(xenogerm, "GetGizmos"), prefix: new HarmonyMethod(typeof(HarmonyPatches).GetMethod("CanInstallXenoGerm_Gizmo_Prefix")));
                str = "Xenogerm_GetGizmos_CanInstallXenoGerm_Prefix";
            }
            catch (Exception ex)
            {
                Log.Error("Last patch that went wrong is: " + str + ". Exception message: " + ex.Message);
            }
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static bool Patch_Caravan_NightResting(ref bool __result, ref Caravan __instance)
        {
            if (__instance.pawns.InnerListForReading.Any(pawn => !pawn.def.HasModExtension<MechanicalPawnProperties>()))
                return true;
            __result = false;
            return false;
        }

        public static bool Patch_PawnGenerator_TryGenerateNewPawnInternal(
          ref Pawn __result,
          ref PawnGenerationRequest request,
          out string error)
        {
            error = null;
            if (request.KindDef.race.GetModExtension<DroidSpawnProperties>() == null)
                return true;
            __result = DroidUtility.MakeDroidTemplate(request.KindDef, request.Faction, request.Tile);
            return false;
        }

        public static void Patch_PawnGenerator_TryGenerateNewPawnInternal_Post(ref Pawn __result)
        {
            if (__result == null || bypassGenerationOfUpgrades || !__result.IsAndroid())
                return;
            bool flag = false;
            if (__result.story != null)
            {
                foreach (AndroidUpgradeDef allDef in DefDatabase<AndroidUpgradeDef>.AllDefs)
                {
                    if (__result.story.Childhood != null && allDef.spawnInBackstories.Contains(__result.story.Childhood.untranslatedTitle) || __result.story.Adulthood != null && allDef.spawnInBackstories.Contains(__result.story.Adulthood.untranslatedTitle))
                    {
                        UpgradeMaker.Make(allDef).Apply(__result);
                        flag = true;
                    }
                }
            }
            if (!flag && Rand.Chance(0.1f))
                UpgradeMaker.Make(DefDatabase<AndroidUpgradeDef>.AllDefs.RandomElement()).Apply(__result);
        }

        public static bool Patch_PawnUtility_HumanFilthChancePerCell(float __result, ThingDef def)
        {
            if (!def.HasModExtension<MechanicalPawnProperties>())
                return true;
            __result = 0.0f;
            return false;
        }

        public static bool Patch_ThoughtWorker_NeedFood_CurrentStateInternal(
          ref ThoughtState __result,
          Pawn p)
        {
            if (!p.IsAndroid())
                return true;
            __result = ThoughtState.Inactive;
            return false;
        }

        public static bool Patch_PartyUtility_EnoughPotentialGuestsToStartGathering(
          ref bool __result,
          Map map,
          GatheringDef gatheringDef,
          ref IntVec3? gatherSpot)
        {
            if (!map.mapPawns.FreeColonistsSpawned.Any(p =>
            {
                MechanicalPawnProperties modExtension = p.def.GetModExtension<MechanicalPawnProperties>();
                return modExtension != null && !modExtension.canSocialize;
            }))
                return true;
            int num1 = Mathf.Clamp(Mathf.RoundToInt(map.mapPawns.FreeColonistsSpawned.Count(p =>
            {
                MechanicalPawnProperties modExtension = p.def.GetModExtension<MechanicalPawnProperties>();
                return modExtension == null || modExtension.canSocialize;
            }) * 0.65f), 2, 10);
            int num2 = 0;
            foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
            {
                if (GatheringsUtility.ShouldPawnKeepGathering(pawn, gatheringDef) && (!gatherSpot.HasValue || !gatherSpot.Value.IsForbidden(pawn)) && (!gatherSpot.HasValue || pawn.CanReach((LocalTargetInfo)gatherSpot.Value, PathEndMode.Touch, Danger.Some)))
                    ++num2;
            }
            __result = num2 >= num1;
            return false;
        }

        public static bool Patch_PartyUtility_ShouldPawnKeepAttending(ref bool __result, Pawn p)
        {
            MechanicalPawnProperties modExtension = p.def.GetModExtension<MechanicalPawnProperties>();
            if (modExtension == null || modExtension.canSocialize)
                return true;
            __result = false;
            return false;
        }

        public static bool Patch_DaysWorthOfFoodCalculator_ApproxDaysWorthOfFood(
          ref List<Pawn> pawns,
          List<ThingDefCount> extraFood,
          int tile,
          IgnorePawnsInventoryMode ignoreInventory,
          Faction faction,
          WorldPath path,
          float nextTileCostLeft,
          int caravanTicksPerMove,
          bool assumeCaravanMoving)
        {
            List<Pawn> pawnList = new List<Pawn>(pawns);
            pawnList.RemoveAll(pawn => pawn.def.HasModExtension<MechanicalPawnProperties>());
            pawns = pawnList;
            return true;
        }

        public static bool Patch_HealthAIUtility_FindBestMedicine(
          ref Thing __result,
          Pawn healer,
          Pawn patient)
        {
            if (!patient.def.HasModExtension<MechanicalPawnProperties>())
                return true;
            Thing thing;
            if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
                thing = null;
            else if (GetMedicineCountToFullyHeal(patient) <= 0)
            {
                thing = null;
            }
            else
            {
                Predicate<Thing> validator = m => !m.IsForbidden(healer) && patient.playerSettings.medCare.AllowsMedicine(m.def) && healer.CanReserve((LocalTargetInfo)m, 10, 1) && m.def.GetModExtension<DroidRepairProperties>() != null;
                Func<Thing, float> priorityGetter = t =>
                {
                    DroidRepairProperties modExtension = t.def.GetModExtension<DroidRepairProperties>();
                    return modExtension == null ? 0.0f : modExtension.repairPotency;
                };
                thing = GenClosest.ClosestThing_Global_Reachable(patient.Position, patient.Map, patient.Map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver), PathEndMode.ClosestTouch, TraverseParms.For(healer), validator: validator, priorityGetter: priorityGetter);
            }
            __result = thing;
            return false;
            static int GetMedicineCountToFullyHeal(Pawn pawn)
            {
                int num = 0;
                int num2 = pawn.health.hediffSet.hediffs.Count + 1;
                List<Hediff> tendableHediffsInTendPriorityOrder = new List<Hediff>();
                List<Hediff> tmpHediffs = new List<Hediff>();
                List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                for (int i = 0; i < hediffs.Count; i++)
                {
                    if (hediffs[i].TendableNow())
                    {
                        tendableHediffsInTendPriorityOrder.Add(hediffs[i]);
                    }
                }
                TendUtility.SortByTendPriority(tendableHediffsInTendPriorityOrder);
                int num3 = 0;
                while (true)
                {
                    num++;
                    if (num > num2)
                    {
                        Log.Error("Too many iterations.");
                        break;
                    }
                    TendUtility.GetOptimalHediffsToTendWithSingleTreatment(pawn, usingMedicine: true, tmpHediffs, tendableHediffsInTendPriorityOrder);
                    if (!tmpHediffs.Any())
                    {
                        break;
                    }
                    num3++;
                    for (int j = 0; j < tmpHediffs.Count; j++)
                    {
                        tendableHediffsInTendPriorityOrder.Remove(tmpHediffs[j]);
                    }
                }
                tmpHediffs.Clear();
                tendableHediffsInTendPriorityOrder.Clear();
                return num3;
            }
        }

        public static bool Patch_Toils_Tend_FinalizeTend(ref Toil __result, Pawn patient)
        {
            if (!patient.def.HasModExtension<MechanicalPawnProperties>())
                return true;
            Toil toil = new Toil();
            toil.initAction = () =>
            {
                Pawn actor = toil.actor;
                Thing thing = actor.CurJob.targetB.Thing;
                float num = !patient.RaceProps.Animal ? 500f : 175f;
                float tendXpGainFactor = RimWorld.ThingDefOf.MedicineIndustrial.MedicineTendXpGainFactor;
                actor.skills?.Learn(SkillDefOf.Crafting, num * tendXpGainFactor);
                DroidUtility.DoTend(actor, patient, thing);
                if (thing != null && thing.Destroyed)
                    actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
                if (!toil.actor.CurJob.endAfterTendedOnce)
                    return;
                actor.jobs.EndCurrentJob(JobCondition.Succeeded);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Instant;
            __result = toil;
            return false;
        }

        public static bool Patch_SkillRecord_Interval(SkillRecord __instance)
        {
            MechanicalPawnProperties modExtension = ((Thing)AccessTools.Field(typeof(SkillRecord), "pawn").GetValue(__instance)).def.GetModExtension<MechanicalPawnProperties>();
            return modExtension == null || !modExtension.noSkillLoss;
        }

        public static void Patch_Pawn_GetGizmos(Pawn __instance, ref IEnumerable<Gizmo> __result)
        {
            if (!__instance.IsColonistPlayerControlled)
                return;
            List<Gizmo> gizmoList1 = new List<Gizmo>(__result);
            if (__instance.needs.TryGetNeed<Need_Energy>() != null)
            {
                List<Gizmo> gizmoList2 = gizmoList1;
                Command_Action commandAction = new Command_Action();
                commandAction.defaultLabel = (string)"AndroidGizmoRechargeNowLabel".Translate();
                commandAction.defaultDesc = (string)"AndroidGizmoRechargeNowDescription".Translate();
                commandAction.icon = ContentFinder<Texture2D>.Get("UI/Commands/TryReconnect");
                commandAction.Order = -98f;
                commandAction.action = () =>
                {
                    Thing targetA = EnergyNeedUtility.ClosestPowerSource(__instance);
                    if (targetA == null)
                        return;
					Job job = null;
                    Building t = targetA as Building;
                    if (targetA != null && t != null && t.PowerComp != null && (double)t.PowerComp.PowerNet.CurrentStoredEnergy() > 50.0)
                    {
                        IntVec3 position = targetA.Position;
                        if (position.Walkable(__instance.Map) && position.InAllowedArea(__instance) && __instance.CanReserve(new LocalTargetInfo(position)) && __instance.CanReach((LocalTargetInfo)position, PathEndMode.OnCell, Danger.Deadly))
                            job = new Job(JobDefOf.ChJAndroidRecharge, (LocalTargetInfo)targetA);
                        if (job == null)
                        {
                            foreach (IntVec3 intVec3 in (IEnumerable<IntVec3>)GenAdj.CellsAdjacentCardinal(t).OrderByDescending(selector => selector.DistanceTo(__instance.Position)))
                            {
                                if (intVec3.Walkable(__instance.Map) && intVec3.InAllowedArea(__instance) && __instance.CanReserve(new LocalTargetInfo(intVec3)) && __instance.CanReach((LocalTargetInfo)intVec3, PathEndMode.OnCell, Danger.Deadly))
                                    job = new Job(JobDefOf.ChJAndroidRecharge, (LocalTargetInfo)targetA, (LocalTargetInfo)intVec3);
                            }
                        }
                    }
                    if (job != null)
                        __instance.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.SatisfyingNeeds));
                };
                gizmoList2.Add(commandAction);
            }
            foreach (Hediff hediff in __instance.health.hediffSet.hediffs)
            {
                if (hediff is IExtraGizmos extraGizmos)
                {
                    foreach (Gizmo gizmo in extraGizmos.GetGizmosExtra())
                        gizmoList1.Add(gizmo);
                }
            }
            __result = gizmoList1;
        }

        public static void Patch_Pawn_HealthTracker_AddHediff(
          Pawn_HealthTracker __instance,
          Hediff hediff,
          BodyPartRecord part,
          ref DamageInfo dinfo,
          DamageWorker.DamageResult result)
        {
            Pawn pawn = Pawn_HealthTracker_GetPawn(__instance);
            if (!pawn.health.hediffSet.HasHediff(HediffDefOf.ChjAndroidLike) || pawn.Dead || ThingDefOf.ChjAndroid.race.hediffGiverSets == null)
                return;
            for (int index1 = 0; index1 < ThingDefOf.ChjAndroid.race.hediffGiverSets.Count; ++index1)
            {
                HediffGiverSetDef hediffGiverSet = ThingDefOf.ChjAndroid.race.hediffGiverSets[index1];
                for (int index2 = 0; index2 < hediffGiverSet.hediffGivers.Count; ++index2)
                    hediffGiverSet.hediffGivers[index2].OnHediffAdded(pawn, hediff);
            }
        }

        public static void Patch_Pawn_HealthTracker_HealthTick(Pawn_HealthTracker __instance)
        {
            Pawn pawn = Pawn_HealthTracker_GetPawn(__instance);
            if (!pawn.health.hediffSet.HasHediff(HediffDefOf.ChjAndroidLike) || pawn.Dead)
                return;
            List<HediffGiverSetDef> hediffGiverSets = ThingDefOf.ChjAndroid.race.hediffGiverSets;
            if (hediffGiverSets != null && pawn.IsHashIntervalTick(60))
            {
                for (int index1 = 0; index1 < hediffGiverSets.Count; ++index1)
                {
                    List<HediffGiver> hediffGivers = hediffGiverSets[index1].hediffGivers;
                    for (int index2 = 0; index2 < hediffGivers.Count; ++index2)
                    {
                        hediffGivers[index2].OnIntervalPassed(pawn, null);
                        if (pawn.Dead)
                            return;
                    }
                }
            }
            pawn.health.hediffSet.hediffs.RemoveAll(hediff => hediff.def == RimWorld.HediffDefOf.BloodLoss);
        }

        public static bool Patch_Pawn_HealthTracker_DropBloodFilth(Pawn_HealthTracker __instance)
        {
            Pawn pawn = Pawn_HealthTracker_GetPawn(__instance);
            if (!pawn.health.hediffSet.HasHediff(HediffDefOf.ChjAndroidLike) || !pawn.Spawned && !(pawn.ParentHolder is Pawn_CarryTracker) || !pawn.SpawnedOrAnyParentSpawned || pawn.RaceProps.BloodDef == null)
                return true;
            FilthMaker.TryMakeFilth(pawn.PositionHeld, pawn.MapHeld, ThingDefOf.ChjAndroid.race.BloodDef, pawn.LabelIndefinite());
            return false;
        }

        public static bool CompatPatch_Boredom_GetReport(
          ref Alert_Boredom __instance,
          ref AlertReport __result)
        {
            IEnumerable<Pawn> __result1 = null;
            CompatPatch_BoredPawns(ref __result1);
            __result = AlertReport.CulpritsAre(__result1.ToList());
            return false;
        }

        public static bool CompatPatch_BoredPawns(ref IEnumerable<Pawn> __result)
        {
            List<Pawn> pawnList = new List<Pawn>();
            foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsAndPrisonersSpawned)
            {
                if (pawn.needs.joy != null && ((double)pawn.needs.joy.CurLevelPercentage < 0.24000000953674316 || pawn.GetTimeAssignment() == TimeAssignmentDefOf.Joy) && pawn.needs.joy.tolerances.BoredOfAllAvailableJoyKinds(pawn))
                    pawnList.Add(pawn);
            }
            __result = pawnList;
            return false;
        }

        public static bool CompatPatch_VomitJob(
          ref JobDriver_Vomit __instance,
          ref IEnumerable<Toil> __result)
        {
            if (!__instance.pawn.def.HasModExtension<MechanicalPawnProperties>())
                return true;
            JobDriver_Vomit instance = __instance;
            __result = new List<Toil>()
            {
                new Toil()
                {
                    initAction =  () => instance.pawn.jobs.StopAll()
                }
            };
            return false;
        }

        public static bool CompatPatch_CanDoInteraction(ref bool __result, ref Pawn pawn)
        {
            MechanicalPawnProperties modExtension = pawn.def.GetModExtension<MechanicalPawnProperties>();
            if (modExtension == null || modExtension.canSocialize)
                return true;
            __result = false;
            return false;
        }

        public static bool CompatPatch_InspirationHandlerTick(ref InspirationHandler __instance) => !__instance.pawn.def.HasModExtension<MechanicalPawnProperties>();

        public static bool CompatPatch_AppendThoughts_ForHumanlike(ref Pawn victim)
        {
            MechanicalPawnProperties modExtension = victim.def.GetModExtension<MechanicalPawnProperties>();
            return modExtension == null || modExtension.colonyCaresIfDead;
        }

        public static bool CompatPatch_InteractionsTrackerTick(ref Pawn_InteractionsTracker __instance)
        {
            MechanicalPawnProperties modExtension = Pawn_InteractionsTracker_GetPawn(__instance).def.GetModExtension<MechanicalPawnProperties>();
            return modExtension == null || modExtension.canSocialize;
        }

        public static bool CompatPatch_CanInteractNowWith(
          ref Pawn_InteractionsTracker __instance,
          ref bool __result,
          ref Pawn recipient)
        {
            MechanicalPawnProperties modExtension = recipient.def.GetModExtension<MechanicalPawnProperties>();
            if (modExtension == null || modExtension.canSocialize)
                return true;
            __result = false;
            return false;
        }

        public static bool CompatPatch_SocialFightChance(
          ref Pawn_InteractionsTracker __instance,
          ref float __result,
          ref InteractionDef interaction,
          ref Pawn initiator)
        {
            MechanicalPawnProperties modExtension1 = Pawn_InteractionsTracker_GetPawn(__instance).def.GetModExtension<MechanicalPawnProperties>();
            int num;
            if (modExtension1 == null || modExtension1.canSocialize)
            {
                MechanicalPawnProperties modExtension2 = initiator.def.GetModExtension<MechanicalPawnProperties>();
                num = modExtension2 == null ? 0 : (!modExtension2.canSocialize ? 1 : 0);
            }
            else
                num = 1;
            if (num == 0)
                return true;
            __result = 0.0f;
            return false;
        }

        public static void MechanoidsFixerAncient(ref bool __result, PawnKindDef kind)
        {
            if (!kind.race.HasModExtension<MechanicalPawnProperties>())
                return;
            __result = false;
        }

        public static void MechanoidsFixer(ref bool __result, PawnKindDef def)
        {
            if (!def.race.HasModExtension<MechanicalPawnProperties>())
                return;
            __result = false;
        }

        public static bool CompatPatch_GetJoyTryGiveJob(
          ref JobGiver_EatInGatheringArea __instance,
          ref Job __result,
          ref Pawn pawn)
        {
            if (!pawn.def.HasModExtension<MechanicalPawnProperties>())
                return true;
            __result = null;
            return false;
        }

        public static bool CompatPatch_EatInPartyAreaTryGiveJob(
          ref JobGiver_EatInGatheringArea __instance,
          ref Job __result,
          ref Pawn pawn)
        {
            if (!pawn.def.HasModExtension<MechanicalPawnProperties>())
                return true;
            __result = null;
            return false;
        }

        public static bool CompatPatch_ShouldGuestKeepAttendingGathering(ref bool __result, ref Pawn p)
        {
            MechanicalPawnProperties modExtension = p.def.GetModExtension<MechanicalPawnProperties>();
            if (modExtension == null || modExtension.canSocialize)
                return true;
            __result = !p.Downed && (double)p.health.hediffSet.BleedRateTotal <= 0.0 && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff() && !p.InAggroMentalState && !p.IsPrisoner;
            return false;
        }

        public static bool CompatPatch_TimetablePreventsLayDown(ref bool __result, ref Pawn pawn)
        {
            if (!pawn.def.HasModExtension<MechanicalPawnProperties>())
                return true;
            __result = pawn.timetable != null && !pawn.timetable.CurrentAssignment.allowRest;
            return false;
        }

        public static bool CompatPatch_CalculatePain(ref HediffSet __instance, ref float __result)
        {
            if (!__instance.pawn.def.HasModExtension<MechanicalPawnProperties>())
                return true;
            __result = 0.0f;
            return false;
        }

        public static bool CompatPatch_ShouldBeDeadFromRequiredCapacity(
          ref Pawn_HealthTracker __instance,
          ref PawnCapacityDef __result)
        {
            if (!Pawn_HealthTracker_GetPawn(__instance).def.HasModExtension<MechanicalPawnProperties>())
                return true;
            List<PawnCapacityDef> defsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
            for (int index = 0; index < defsListForReading.Count; ++index)
            {
                PawnCapacityDef capacity = defsListForReading[index];
                if (defsListForReading[index] == PawnCapacityDefOf.Consciousness && !__instance.capacities.CapableOf(capacity))
                {
                    __result = capacity;
                    return false;
                }
            }
            __result = null;
            return false;
        }

        public static bool CompatPatch_WillIngestStackCountOf(
          int __result,
          ref Pawn ingester,
          ref ThingDef def)
        {
            if (ingester == null || ingester?.needs.TryGetNeed(NeedDefOf.Food) != null)
                return true;
            __result = 0;
            return false;
        }

        public static bool CompatPatch_ShouldMeasureTimeNow(bool __result, ref Pawn pawn)
        {
            if (pawn == null || pawn?.needs.TryGetNeed(NeedDefOf.Rest) != null)
                return true;
            __result = pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || HealthAIUtility.ShouldSeekMedicalRest(pawn) && pawn.CurJob.restUntilHealed);
            return false;
        }

        public static bool CompatPatch_CanInitiateInteraction(bool __result, ref Pawn pawn)
        {
            MechanicalPawnProperties modExtension = pawn.def.GetModExtension<MechanicalPawnProperties>();
            if (modExtension == null || modExtension.canSocialize)
                return true;
            __result = false;
            return false;
        }

        public static Pawn Pawn_HealthTracker_GetPawn(Pawn_HealthTracker instance) => (Pawn)int_Pawn_HealthTracker_GetPawn.GetValue(instance);
        public static Pawn Pawn_InteractionsTracker_GetPawn(Pawn_InteractionsTracker instance) => (Pawn)int_Pawn_InteractionsTracker_GetPawn.GetValue(instance);
        public static Pawn Pawn_NeedsTracker_GetPawn(Pawn_NeedsTracker instance) => (Pawn)int_Pawn_NeedsTracker_GetPawn.GetValue(instance);
        public static Pawn PawnRenderer_GetPawn_GetPawn(PawnRenderer instance) => (Pawn)int_PawnRenderer_GetPawn.GetValue(instance);

        public static NeedDef ThinkNode_ConditionalNeedPercentageAbove_GetNeed(
          ThinkNode_ConditionalNeedPercentageAbove instance)
        {
            return (NeedDef)int_ConditionalPercentageNeed_need.GetValue(instance);
        }

        public static bool Patch_ThinkNode_ConditionalNeedPercentageAbove_Satisfied(
          ref ThinkNode_ConditionalNeedPercentageAbove __instance,
          ref bool __result,
          ref Pawn pawn)
        {
            NeedDef need = ThinkNode_ConditionalNeedPercentageAbove_GetNeed(__instance);
            if (pawn.needs.TryGetNeed(need) != null)
                return true;
            __result = false;
            return false;
        }

        public static bool Patch_HealthUtility_AdjustSeverity(
          Pawn pawn,
          HediffDef hdDef,
          float sevOffset)
        {
            return !pawn.IsAndroid() || hdDef != RimWorld.HediffDefOf.Malnutrition;
        }

        public static void Patch_Pawn_NeedsTracker_ShouldHaveNeed(
          ref Pawn_NeedsTracker __instance,
          ref bool __result,
          ref NeedDef nd)
        {
            Pawn pawn = Pawn_NeedsTracker_GetPawn(__instance);
            if (NeedsDefOf.ChJEnergy != null && nd == NeedsDefOf.ChJEnergy)
            {
                int num = pawn.IsAndroid() ? 1 : (pawn.def.HasModExtension<MechanicalPawnProperties>() ? 1 : 0);
                __result = num != 0;
            }
            if (AndroidsModSettings.Instance.droidCompatibilityMode || nd != NeedDefOf.Food && nd != NeedDefOf.Rest && nd != NeedsDefOf.Joy && nd != NeedsDefOf.Beauty && nd != NeedsDefOf.Comfort && nd != NeedsDefOf.RoomSize && nd != NeedsDefOf.Outdoors && (Need_Bladder == null || nd != Need_Bladder) && (Need_Hygiene == null || nd != Need_Hygiene) || !pawn.def.HasModExtension<MechanicalPawnProperties>())
                return;
            __result = false;
        }

        /* public static void Patch_PawnRenderer_RenderPawnInternal(
          ref PawnRenderer __instance,
          Vector3 rootLoc,
          float angle,
          bool renderBody,
          Rot4 bodyFacing,
          RotDrawMode bodyDrawType,
          PawnRenderFlags flags)
        {
            if (flags.FlagSet(PawnRenderFlags.Invisible) || __instance == null || !AndroidsModSettings.Instance.androidEyeGlow)
                return;
            Pawn pawnGetPawn = HarmonyPatches.PawnRenderer_GetPawn_GetPawn(__instance);
            if (pawnGetPawn != null && pawnGetPawn.IsAndroid() && !pawnGetPawn.Dead && !flags.FlagSet(PawnRenderFlags.HeadStump) && ((flags.FlagSet(PawnRenderFlags.Portrait) || pawnGetPawn?.jobs?.curDriver == null ? (flags.FlagSet(PawnRenderFlags.Portrait) ? 1 : 0) : (!pawnGetPawn.jobs.curDriver.asleep ? 1 : 0)) != 0 || flags.FlagSet(PawnRenderFlags.Portrait)))
            {
                Quaternion quat = Quaternion.AngleAxis(angle, Vector3.up);
                Vector3 vector3_1 = rootLoc;
                if (bodyFacing != Rot4.North)
                {
                    vector3_1.y += 0.0281250011f;
                    rootLoc.y += 3f / 128f;
                }
                else
                {
                    vector3_1.y += 3f / 128f;
                    rootLoc.y += 0.0281250011f;
                }
                Vector3 vector3_2 = quat * __instance.BaseHeadOffsetAt(bodyFacing);
                Vector3 loc = vector3_1 + vector3_2 + new Vector3(0.0f, 0.01f, 0.0f);
                if (bodyFacing != Rot4.North)
                {
                    Mesh mesh = MeshPool.humanlikeHeadSet.MeshAt(bodyFacing);
                    if (bodyFacing.IsHorizontal)
                        GenDraw.DrawMeshNowOrLater(mesh, loc, quat, EffectTextures.GetEyeGraphic(false, pawnGetPawn.story.HairColor.SaturationChanged(0.6f)).MatSingle, flags.FlagSet(PawnRenderFlags.Portrait));
                    else
                        GenDraw.DrawMeshNowOrLater(mesh, loc, quat, EffectTextures.GetEyeGraphic(true, pawnGetPawn.story.HairColor.SaturationChanged(0.6f)).MatSingle, flags.FlagSet(PawnRenderFlags.Portrait));
                }
            }
        } */

        public static void Patch_Need_Food_Starving_Get(Need_Food __instance, ref bool __result)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if (pawn == null || !pawn.IsAndroid())
                return;
            __result = false;
        }

        #region Ideology
        public static bool Patch_ThoughtWorker_LookChangeDesired(ref ThoughtState __result, Pawn p)
        {
            if (p.def.HasModExtension<MechanicalPawnProperties>())
            {
                __result = ThoughtState.Inactive;
                return false;
            }
            return true;
        }

        public static void Patch_CanDesireLookChange_getter(ref bool __result, Pawn_StyleTracker __instance)
        {
            if (__instance.pawn.def.HasModExtension<MechanicalPawnProperties>())
                __result = false;
        }
        #endregion

        #region Biotech
        public static void CanInstallMechLinkPostfix(Pawn p, ref AcceptanceReport __result)
        {
            if (!__result && p.IsAndroid())
            {
                __result = true;
            }
        }

        public static bool CanInstallXenoGerm_Prefix(Pawn selPawn)
        {
            if (selPawn != null && selPawn.def.GetModExtension<DroidSpawnProperties>() != null)
            {
                selPawn.genes = null;
            }
            return true;
        }


        public static bool CanInstallXenoGerm_Gizmo_Prefix(Xenogerm __instance)
        {
            foreach (Pawn item in __instance.Map.mapPawns.AllPawnsSpawned)
            {
                if (item != null && item.def.GetModExtension<DroidSpawnProperties>() != null)
                {
                    item.genes = null;
                }
            }
            return true;
        }

        public static bool DoEffectPrefix(Pawn user, CompUseEffect_InstallImplant __instance)
        {
            if (!user.IsAndroid())
                return true;
            BodyPartRecord part = user.RaceProps.body.GetPartsWithDef(AndroidBodyPartDefOf.ArtificialAndroidBrain).FirstOrFallback();
            if (part == null)
                return false;
            Hediff firstHediffOfDef = user.health.hediffSet.GetFirstHediffOfDef(__instance.Props.hediffDef);
            if (firstHediffOfDef == null && !__instance.Props.requiresExistingHediff)
            {
                user.health.AddHediff(__instance.Props.hediffDef, part);
            }
            else
            {
                if (!__instance.Props.canUpgrade)
                    return false;
                ((Hediff_Level)firstHediffOfDef).ChangeLevel(1);
            }
            return false;
        }

        public static bool GetPartsPrefix(BodyDef __instance, ref BodyPartDef def)
        {
            if (__instance.corePart.def == AndroidBodyPartDefOf.AndroidThorax && def == AndroidBodyPartDefOf.Brain)
                def = AndroidBodyPartDefOf.ArtificialAndroidBrain;
            return true;
        }

        public static void ValidPostfix(CompAbilityEffect_BloodfeederBite __instance, ref bool __result, LocalTargetInfo target, bool throwMessages)
        {
            if (__instance.parent.def.HasModExtension<AbilityModExtension>() && target.Thing is Pawn p)
            {
                if (!__instance.parent.def.GetModExtension<AbilityModExtension>().canTargetAndroids && p.IsAndroid())
                {
                    if (throwMessages)
                    {
                        Messages.Message("MessageCantUseOnAndroid".Translate(__instance.parent.def.Named("ABILITY")), p, MessageTypeDefOf.RejectInput, historical: false);
                    }
                    __result = false;
                    return;
                }
                if (!__instance.parent.def.GetModExtension<AbilityModExtension>().canTargetDroids && p.def.HasModExtension<MechanicalPawnProperties>())
                {
                    if (throwMessages)
                    {
                        Messages.Message("MessageCantUseOnDroid".Translate(__instance.parent.def.Named("ABILITY")), p, MessageTypeDefOf.RejectInput, historical: false);
                    }
                    __result = false;
                    return;
                }
            }
        }

        public static bool CanUsePrisonerInteractionMode_Prefix(ref bool __result, Pawn pawn, PrisonerInteractionModeDef mode)
        {
            if (pawn.def.HasModExtension<AndroidPawnProperties>() && (mode == PrisonerInteractionModeDefOf.Bloodfeed || mode == PrisonerInteractionModeDefOf.HemogenFarm))
            {
                __result = false;
                return false;
            }
            return true;
        }
        #endregion Biotech

        #region VPE
        /* public static IEnumerable<CodeInstruction> GetGizmoTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            //File.WriteAllLines("E:\\before.txt",instructions.Select(x=>x.ToString()));
            List<CodeInstruction> instructions1 = instructions.ToList();
            int ceq = instructions1.FirstIndexOf(x => x.opcode == OpCodes.Ceq);
            if (ceq++ == -1)
            {
                Log.Error("Old Transpiler");
                return instructions;
            }
            Label brtrue = generator.DefineLabel();
            instructions1[ceq].labels.Add(brtrue); // ret should get this label
            instructions1.InsertRange(ceq, new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Brtrue,brtrue),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(BodyPartRecord), "def")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(AndroidBodyPartDefOf), "AndroidFinger")),
                new CodeInstruction(OpCodes.Ceq)
            });
            //File.WriteAllLines("E:\\after.txt", instructions1.Select(x => x.ToString()));
            return instructions1;
        } */
        /*return x.def == VPE_DefOf.Finger;
	        IL_0000: ldarg.1
	        IL_0001: ldfld class ['Assembly-CSharp']Verse.BodyPartDef ['Assembly-CSharp']Verse.BodyPartRecord::def
	        IL_0006: ldsfld class ['Assembly-CSharp']Verse.BodyPartDef VanillaPsycastsExpanded.VPE_DefOf::Finger
	        IL_000b: ceq
	        IL_000d: ret
        */
        /* public static IEnumerable<CodeInstruction> CastTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            //File.WriteAllLines("E:\\beforecast.txt", instructions.Select(x => x.ToString()));
            List<CodeInstruction> instructions1 = instructions.ToList();
            int ceq = instructions1.FirstIndexOf(x => x.opcode == OpCodes.Ceq);
            if (ceq++ == -1)
            {
                Log.Error("Old Transpiler");
                return instructions;
            }
            Label brtrue = generator.DefineLabel();
            instructions1[ceq].labels.Add(brtrue); // ret should get this label
            instructions1.InsertRange(ceq, new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Brtrue,brtrue),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(BodyPartRecord), "def")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(AndroidBodyPartDefOf), "AndroidFinger")),
                new CodeInstruction(OpCodes.Ceq)
            });
            //File.WriteAllLines("E:\\aftercast.txt", instructions1.Select(x => x.ToString()));
            return instructions1;
        } */
        #endregion VPE
    }
}