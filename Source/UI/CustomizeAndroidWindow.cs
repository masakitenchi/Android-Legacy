// Decompiled with JetBrains decompiler
// Type: Androids.CustomizeAndroidWindow
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using AlienRace;
using System.Runtime.CompilerServices;
using System.Text;

namespace Androids
{
    [HotSwappable]
    public class CustomizeAndroidWindow : Window
    {
        #region private fields

        private static readonly Vector2 PawnPortraitSize = new Vector2(100f, 140f);
        private static readonly SimpleCurve LevelRandomCurve = new SimpleCurve()
        {
          {
            new CurvePoint(0.0f, 0.0f),
            true
          },
          {
            new CurvePoint(0.5f, 150f),
            true
          },
          {
            new CurvePoint(4f, 150f),
            true
          },
          {
            new CurvePoint(5f, 25f),
            true
          },
          {
            new CurvePoint(10f, 5f),
            true
          },
          {
            new CurvePoint(15f, 0.0f),
            true
          }
        };
        private static readonly SimpleCurve LevelFinalAdjustmentCurve = new SimpleCurve()
        {
          {
            new CurvePoint(0.0f, 0.0f),
            true
          },
          {
            new CurvePoint(10f, 10f),
            true
          },
          {
            new CurvePoint(20f, 16f),
            true
          },
          {
            new CurvePoint(27f, 20f),
            true
          }
        };
        #endregion

        public Building_AndroidPrinter androidPrinter;
        public Pawn newAndroid;
        public List<ThingOrderRequest> finalCalculatedPrintingCost = new List<ThingOrderRequest>();
        public int finalExtraPrintingTimeCost;
        public bool refreshAndroidPortrait;
        public Vector2 upgradesScrollPosition;
        public Vector2 traitsScrollPosition;
        private List<Trait> allTraits = new List<Trait>();
        public PawnKindDef currentPawnKindDef;
        public BackstoryDef newChildhoodBackstory;
        public BackstoryDef newAdulthoodBackstory;
        public Trait replacedTrait;
        public Trait newTrait;
        public List<UpgradeCommand> appliedUpgradeCommands = new List<UpgradeCommand>();
        public List<AndroidUpgradeDef> upgradedDefsToDisable = new List<AndroidUpgradeDef>();
        public List<Trait> originalTraits = new List<Trait>();
        public Color originalHairColor;
        public HairDef originalHairDef;
        public const float UpgradesOffset = 640f;
        public const float UIMargin = 3f;
        public static List<Color> DefaultHairColors = new List<Color>(new Color[13]
        {
          new Color(0.17f, 0.17f, 0.17f, 1f),
          new Color(0.02f, 0.02f, 0.02f, 1f),
          new Color(0.9f, 0.9f, 0.9f, 1f),
          new Color(0.51f, 0.25f, 0.25f, 1f),
          new Color(1f, 0.66f, 0.32f, 1f),
          new Color(0.0f, 0.5f, 1f, 1f),
          new Color(1f, 0.0f, 0.5f, 1f),
          new Color(1f, 0.0f, 0.0f, 1f),
          new Color(0.0f, 1f, 0.0f, 1f),
          new Color(0.0f, 1f, 1f, 1f),
          new Color(0.78f, 0.78f, 0.78f, 1f),
          new Color(0.92f, 0.92f, 0.29f, 1f),
          new Color(0.63f, 0.28f, 0.64f, 1f)
        });
        public List<HediffDef> _pawnHediffs;
        public List<Trait> _pawnTraits;
        public bool IsUpgrade;
        public Pawn clonedPawn;

        public override Vector2 InitialSize => new Vector2(898f, 608f);

        public void UpgradePawnHediffs()
        {
            /* if (!this.IsUpgrade)
                 return (List<HediffDef>)null;
             Pawn_HealthTracker health = this.newAndroid.health;
             List<HediffDef> hediffDefList1;
             if (health == null)
             {
                 hediffDefList1 = (List<HediffDef>)null;
             }
             else
             {
                 HediffSet hediffSet = health.hediffSet;
                 hediffDefList1 = hediffSet != null ? hediffSet.hediffs.Select(hediff => hediff.def).ToList() : null;
             }
             List<HediffDef> hediffDefList2 = hediffDefList1;
             if (this._pawnHediffs.NullOrEmpty())
                 this._pawnHediffs = hediffDefList2;
             return hediffDefList2;*/
            if (!this.IsUpgrade)
                return;

            Pawn_HealthTracker health = this.newAndroid.health;
            List<HediffDef> hediffDefList = health?.hediffSet?.hediffs.Select(hediff => hediff.def).ToList();

            if (this._pawnHediffs.NullOrEmpty())
                this._pawnHediffs = hediffDefList;
        }

        public List<Trait> UpdatePawnTraits(Pawn pawn)
        {
            if (!this.IsUpgrade)
                return null;
            List<Trait> list = pawn.story.traits.allTraits;
            if (this._pawnTraits.NullOrEmpty())
                this._pawnTraits = list;
            return list;
        }

        public bool AlreadyUpgradedOnPawn(AndroidUpgradeDef upgrade)
        {
            this.UpgradePawnHediffs();
            /*List<HediffDef> pawnHediffs = this._pawnHediffs;
            return !pawnHediffs.NullOrEmpty<HediffDef>() && pawnHediffs.Contains(upgrade.hediffToApply);*/
            return _pawnHediffs?.Contains(upgrade.hediffToApply) ?? false;
        }

        //Cannot even find a workshop version that has these lines of code
        //Really don't know if it ever had the feature of "re-entering" printer
        public CustomizeAndroidWindow(Building_AndroidPrinter androidPrinter)
        {
            this.androidPrinter = androidPrinter;
            if (androidPrinter.GetDirectlyHeldThings().FirstOrDefault(p => p is Pawn) is Pawn sourcePawn)
            {
                this.IsUpgrade = true;
                this.currentPawnKindDef = sourcePawn.kindDef;
                if (this.clonedPawn == null)
                {
                    this.clonedPawn = this.Duplicate(sourcePawn);
                    this.UpdatePawnTraits(this.clonedPawn);
                }
                this.newAndroid = sourcePawn;
            }
            else
            {
                this.currentPawnKindDef = PawnKindDef.Named("ChjAndroidColonist");
                this.newAndroid = this.GenerateNewPawn();
            }
            this.RefreshCosts();
        }

        public override void PostClose()
        {
            base.PostClose();
            if (this.androidPrinter.printerStatus != CrafterStatus.Idle)
                return;
            PawnUpdate(this.newAndroid, this.clonedPawn);
        }

        public override void DoWindowContents(Rect inRect)
        {
            HandlePortraitRefresh();
            HandleBackstoryChange();
            HandleTraitChange();

            if (this.newAndroid == null)
                return;
            inRect = inRect.GetInnerRect();
            // 3 split
            inRect.SplitVertically(inRect.width / 3f, out Rect leftRect, out Rect sadRect);
            sadRect.SplitVertically(inRect.width / 3f, out Rect middleRect, out Rect rightRect);
            leftRect = leftRect.ContractedBy(8f);
            middleRect = middleRect.ContractedBy(8f);
            float row = 0f;
            // Draw the left side
            row = DrawNameInput(new Rect(leftRect.x, row, leftRect.width, 32f)) + UIMargin;
            row = DrawBackstorySelection(new Rect(leftRect.x, row, leftRect.width, 48f)) + UIMargin;
            if (Widgets.ButtonText(new Rect(leftRect.x, row, leftRect.width, 24f), "AndroidCustomizationRerollSkills".Translate()))
            {
                RefreshSkills();
            }
            DrawSkillUI(new Rect(leftRect.x, row, leftRect.width, leftRect.yMax - row), ref row);

            // Draw the middle side
            row = 0f;
            if (!IsUpgrade)
            {
                row = DrawRaceSelection(new Rect(middleRect.x, row, middleRect.width, 32f)) + UIMargin;
                row = DrawPawnGenerationButtons(new Rect(middleRect.x, row, middleRect.width, 24f)) + UIMargin;
            }
            row = DrawHairCustomization(middleRect, row);
            Rect pawnRect = GetPawnRect(middleRect, row);
            //GUI.DrawTexture(pawnRect, BaseContent.BlackTex); // background
            row = DrawPawnPortrait(pawnRect);
            row = DrawPrintButton(middleRect, row);
            row = pawnRect.yMax + UIMargin;
            row = DrawCostList(middleRect, row) + UIMargin;
            DrawTraitSection(new Rect(middleRect.x, row, middleRect.width, middleRect.yMax - row), row);

            // right side
            row = 0f;
            //GUI.DrawTexture(rightRect, BaseContent.BlackTex); // background
            DrawUpgradeSection(rightRect.ContractedBy(8f), ref row);

            Text.Anchor = TextAnchor.UpperLeft;
        }

        private void HandlePortraitRefresh()
        {
            if (this.refreshAndroidPortrait)
            {
                this.newAndroid.Drawer.renderer.SetAllGraphicsDirty();
                PortraitsCache.SetDirty(this.newAndroid);
                PortraitsCache.PortraitsCacheUpdate();
                this.refreshAndroidPortrait = false;
            }
        }

        private void HandleBackstoryChange()
        {
            if (this.newChildhoodBackstory != null)
            {
                this.newAndroid.story.Childhood = newChildhoodBackstory;
                this.newChildhoodBackstory = null;
                this.RefreshPawn();
            }
            if (this.newAdulthoodBackstory != null)
            {
                this.newAndroid.story.Adulthood = newAdulthoodBackstory;
                this.newAdulthoodBackstory = null;
                this.RefreshPawn();
            }
        }

        private void HandleTraitChange()
        {
            if (this.newTrait != null)
            {
                if (this.replacedTrait != null)
                {
                    this.newAndroid.story.traits.allTraits.Remove(this.replacedTrait);
                    this.replacedTrait = null;
                }
                this.newAndroid.story.traits.allTraits.Add(new Trait(this.newTrait.def, this.newTrait.Degree));
                this.newAndroid.workSettings?.EnableAndInitialize();
                this.newAndroid.skills?.Notify_SkillDisablesChanged();
                if (this.newAndroid.RaceProps.Humanlike)
                    this.newAndroid.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
                this.RefreshPawn();
                this.newTrait = null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Rect GetPawnRect(Rect inRect, float row)
        {
            return new Rect(inRect)
            {
                height = PawnPortraitSize.y + 32f,
                width = PawnPortraitSize.x + 32f,
                y = inRect.y + row + 16f,
                x = inRect.x + 16f,
            };
        }

        private float DrawPawnPortrait(Rect pawnRect)
        {
            Rect pawnRenderRect = new Rect(pawnRect).ContractedBy(16f);
            //GUI.DrawTexture(pawnRenderRect, BaseContent.BlackTex); // background
            GUI.DrawTexture(pawnRenderRect, PortraitsCache.Get(this.newAndroid, PawnPortraitSize, Rot4.South));
            Widgets.InfoCardButton(pawnRenderRect.xMax - 8f, pawnRenderRect.y, newAndroid);
            return pawnRenderRect.yMax;
        }

        private float DrawNameInput(Rect nameRect)
        {
            if (this.newAndroid.Name is NameTriple name)
            {
                Rect First = new Rect(nameRect);
                First.width *= 0.333f;
                Rect Middle = new Rect(nameRect);
                Middle.width *= 0.333f;
                Middle.x += Middle.width;
                Rect Last = new Rect(nameRect);
                Last.width *= 0.333f;
                Last.x += Middle.width * 2f;
                string first = name.First;
                string nick = name.Nick;
                string last = name.Last;
                CharacterCardUtility.DoNameInputRect(First, ref first, 12);
                if (name.Nick == name.First || name.Nick == name.Last)
                    GUI.color = new Color(1f, 1f, 1f, 0.5f);
                CharacterCardUtility.DoNameInputRect(Middle, ref nick, 9);
                GUI.color = Color.white;
                CharacterCardUtility.DoNameInputRect(Last, ref last, 12);
                if (name.First != first || name.Nick != nick || name.Last != last)
                    this.newAndroid.Name = new NameTriple(first, nick, last);
                TooltipHandler.TipRegion(First, (TipSignal)"FirstNameDesc".Translate());
                TooltipHandler.TipRegion(Middle, (TipSignal)"ShortIdentifierDesc".Translate());
                TooltipHandler.TipRegion(Last, (TipSignal)"LastNameDesc".Translate());
            }
            else
            {
                nameRect.width = 999f;
                Text.Font = GameFont.Medium;
                Widgets.Label(nameRect, this.newAndroid.Name.ToStringFull);
                Text.Font = GameFont.Small;
            }
            return nameRect.yMax;
        }

        private float DrawHairCustomization(Rect inRect, float row)
        {
            //Hair customization
            Rect outRect = new Rect(inRect)
            {
                y = row,
                height = 24f,
            };
            Rect hairColorRect = new Rect(outRect);
            hairColorRect.width = hairColorRect.height;
            Widgets.DrawBoxSolid(hairColorRect, this.newAndroid.story.HairColor);
            Widgets.DrawBox(hairColorRect);
            Widgets.DrawHighlightIfMouseover(hairColorRect);
            if (Widgets.ButtonInvisible(hairColorRect))
            {
                Action func(Color color) => () =>
                {
                    this.newAndroid.story.HairColor = color;
                    this.newAndroid.Drawer.renderer.SetAllGraphicsDirty();
                    PortraitsCache.SetDirty(this.newAndroid);
                    PortraitsCache.PortraitsCacheUpdate();
                };
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (Color hairColor1 in DefaultHairColors)
                {
                    Color HairColor = hairColor1;
                    options.Add(new FloatMenuOption((string)"AndroidCustomizationChangeColor".Translate(),
                    func(HairColor), extraPartWidth: 24f, extraPartOnGUI: rect =>
                    {
                        Rect rect6 = new Rect(rect);
                        rect6.x += 8f;
                        Widgets.DrawBoxSolid(rect6, HairColor);
                        Widgets.DrawBox(rect6);
                        this.RefreshUpgrades();
                        this.RefreshCosts();
                        return false;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(options));
            }
            Rect hairTypeRect = new Rect(outRect);
            hairTypeRect.width -= hairColorRect.width;
            hairTypeRect.width -= 8f;
            hairTypeRect.x = (float)((double)hairColorRect.x + (double)hairColorRect.width + 8.0);
            if (Widgets.ButtonText(hairTypeRect, (string)(this.newAndroid?.story?.hairDef?.LabelCap ?? (TaggedString)"Bald"), true, true, true))
            {
                IEnumerable<HairDef> hairs = DefDatabase<HairDef>.AllDefs.Where(hairdef =>
               {
                   if (this.newAndroid.gender == Gender.Female && (hairdef.styleGender == StyleGender.Any || hairdef.styleGender == StyleGender.Female || hairdef.styleGender == StyleGender.FemaleUsually))
                       return true;
                   if (this.newAndroid.gender != Gender.Male)
                       return false;
                   return hairdef.styleGender == StyleGender.Any || hairdef.styleGender == StyleGender.Male || hairdef.styleGender == StyleGender.MaleUsually;
               });
                if (hairs != null)
                    FloatMenuUtility.MakeMenu(hairs, hairDef => (string)hairDef.LabelCap, hairDef => () =>
                {
                    this.newAndroid.story.hairDef = hairDef;
                    this.newAndroid.Drawer.renderer.SetAllGraphicsDirty();
                    PortraitsCache.SetDirty(this.newAndroid);
                    PortraitsCache.PortraitsCacheUpdate();
                    this.RefreshUpgrades();
                    this.RefreshCosts();
                });
            }
            return outRect.yMax;
        }

        private float DrawPrintButton(Rect inRect, float row)
        {
            Rect outRect = new Rect(inRect)
            {
                y = row,
                height = 32f
            };
            Text.Font = GameFont.Medium;
            string str1 = (string)"AndroidCustomizationPrint".Translate();
            if (Widgets.ButtonText(outRect, str1, true, true, true))
            {
                if (!this.finalCalculatedPrintingCost.NullOrEmpty())
                    this.androidPrinter.orderProcessor.requestedItems = this.finalCalculatedPrintingCost;
                this.androidPrinter.extraTimeCost = this.finalExtraPrintingTimeCost;
                this.androidPrinter.pawnToPrint = this.newAndroid;
                this.androidPrinter.printerStatus = CrafterStatus.Filling;
                this.Close();
            }
            return outRect.yMax;
        }

        // 24f for title & labels, 2f for margins
        private float DrawCostList(Rect inRect, float row)
        {
            Rect outRect = new Rect(inRect)
            {
                y = inRect.y + row,
                height = 52f
            };
            Widgets.DrawTitleBG(outRect.TopHalf());
            Widgets.Label(outRect.ContractedBy(2f), "AndroidCustomizationCostLabel".Translate());
            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.LowerLeft;
            Rect resourceRect = new Rect(outRect.BottomHalf().ContractedBy(2f)) { width = 24f };
            Widgets.DrawTextureFitted(resourceRect, ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast"), 1f);
            TooltipHandler.TipRegion(resourceRect, (TipSignal)("AndroidCustomizationTimeCost".Translate() + ": " + (this.androidPrinter.PrinterProperties.ticksToCraft + this.finalExtraPrintingTimeCost).ToStringTicksToPeriodVerbose()));
            Widgets.DrawHighlightIfMouseover(resourceRect);
            Widgets.Label(resourceRect.ExpandedBy(8f), (this.androidPrinter.PrinterProperties.ticksToCraft + this.finalExtraPrintingTimeCost).ToStringTicksToPeriodVerbose() ?? "");
            Text.Anchor = TextAnchor.LowerRight;
            foreach (ThingOrderRequest thingOrderRequest in this.finalCalculatedPrintingCost)
            {
                resourceRect.x += 26f;
                if (thingOrderRequest.nutrition)
                {
                    Widgets.ThingIcon(resourceRect, RimWorld.ThingDefOf.Meat_Human);
                    TooltipHandler.TipRegion(resourceRect, (TipSignal)"AndroidNutrition".Translate());
                }
                else
                {
                    Widgets.ThingIcon(resourceRect, thingOrderRequest.thingDef);
                    TooltipHandler.TipRegion(resourceRect, (TipSignal)thingOrderRequest.thingDef.LabelCap);
                }
                Widgets.DrawHighlightIfMouseover(resourceRect);
                Widgets.Label(resourceRect, thingOrderRequest.amount.ToString() ?? "");
            }
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;
            row = outRect.yMax;
            return row;
        }

        private float DrawRaceSelection(Rect outRect)
        {
            Text.Font = GameFont.Small;
            if (RaceUtility.AvailableRacesForPrinter.Count() > 1)
            {
                if (Widgets.ButtonText(outRect, (string)this.currentPawnKindDef.race.LabelCap))
                    FloatMenuUtility.MakeMenu(RaceUtility.AvailableRacesForPrinter,
                    raceKind => (string)raceKind.race.LabelCap,
                    raceKind => () =>
                    {
                        this.currentPawnKindDef = raceKind;
                        Gender gender = Gender.Female;
                        if (this.currentPawnKindDef.race is ThingDef_AlienRace race2 && race2.alienRace.generalSettings.maleGenderProbability >= 1.0)
                            gender = Gender.Male;
                        this.newAndroid = this.GenerateNewPawn(gender);
                        this.RefreshUpgrades();
                        this.RefreshCosts();
                    });
            }
            return outRect.yMax;
        }

        private float DrawPawnGenerationButtons(Rect outRect)
        {
            //Generate new pawn
            if (this.androidPrinter.PawnInside == null)
            {
                if (this.currentPawnKindDef.race is ThingDef_AlienRace race && race.alienRace.generalSettings.maleGenderProbability < 1.0 && Widgets.ButtonText(outRect.LeftHalf(), (string)"AndroidCustomizationRollFemale".Translate(), true, true, true))
                {
                    this.newAndroid.SetFactionDirect(null);
                    this.newAndroid.Destroy(DestroyMode.Vanish);
                    this.newAndroid = this.GenerateNewPawn();
                    this.RefreshUpgrades();
                    this.RefreshCosts();
                }
                if (Widgets.ButtonText(outRect.RightHalf(), (string)"AndroidCustomizationRollMale".Translate(), true, true, true))
                {
                    this.newAndroid.SetFactionDirect(null);
                    this.newAndroid.Destroy(DestroyMode.Vanish);
                    this.newAndroid = this.GenerateNewPawn(Gender.Male);
                    this.RefreshUpgrades();
                    this.RefreshCosts();
                }
            }
            return outRect.yMax;
        }

        private float DrawBackstorySelection(Rect outRect)
        {
            Widgets.DrawBox(outRect.TopHalf());
            Widgets.DrawHighlightIfMouseover(outRect.TopHalf());
            string childStory = this.newAndroid.story.Childhood == null ? (string)("AndroidCustomizationFirstIdentity".Translate() + " " + "AndroidNone".Translate()) : (string)("AndroidCustomizationFirstIdentity".Translate() + " " + this.newAndroid.story.Childhood.TitleCapFor(this.newAndroid.gender));
            if (Widgets.ButtonText(outRect.TopHalf(), childStory))
                FloatMenuUtility.MakeMenu(
                    DefDatabase<BackstoryDef>.AllDefsListForReading.Where(backstory => (backstory.spawnCategories.Any(category => this.currentPawnKindDef.backstoryCategories != null && this.currentPawnKindDef.backstoryCategories.Any(subCategory => subCategory == category)) || backstory.spawnCategories.Contains("ChjAndroid")) && backstory.slot == BackstorySlot.Childhood),
                    backstory => backstory.TitleCapFor(this.newAndroid.gender),
                    backstory => () => this.newChildhoodBackstory = backstory);
            if (this.newAndroid.story.Childhood != null)
                TooltipHandler.TipRegion(outRect.TopHalf(), (TipSignal)this.newAndroid.story.Childhood.FullDescriptionFor(this.newAndroid));
            Widgets.DrawBox(outRect.BottomHalf());
            Widgets.DrawHighlightIfMouseover(outRect.BottomHalf());
            string adultStory = this.newAndroid.story.Adulthood == null ? (string)("AndroidCustomizationSecondIdentity".Translate() + " " + "AndroidNone".Translate()) : (string)("AndroidCustomizationSecondIdentity".Translate() + " " + this.newAndroid.story.Adulthood.TitleCapFor(this.newAndroid.gender));
            if (Widgets.ButtonText(outRect.BottomHalf(), adultStory))
                FloatMenuUtility.MakeMenu(
                    DefDatabase<BackstoryDef>.AllDefsListForReading.Where(backstory => (backstory.spawnCategories.Any(category => this.currentPawnKindDef.backstoryCategories != null && this.currentPawnKindDef.backstoryCategories.Any(subCategory => subCategory == category)) || backstory.spawnCategories.Contains("ChjAndroid")) && backstory.slot == BackstorySlot.Adulthood),
                    backstory => backstory.TitleCapFor(this.newAndroid.gender),
                    backstory => () => this.newAdulthoodBackstory = backstory);
            if (this.newAndroid.story.Adulthood != null)
                TooltipHandler.TipRegion(outRect.BottomHalf(), (TipSignal)this.newAndroid.story.Adulthood.FullDescriptionFor(this.newAndroid));
            return outRect.yMax;
        }

        private void DrawSkillUI(Rect outRect, ref float row)
        {
            //SkillUI
            SkillUI.DrawSkillsOf(this.newAndroid, new Vector2(32f, row + 27f), SkillUI.SkillDrawMode.Gameplay, new Rect(32f, row, 256f, 27f));
            row += 32f;
        }

        private float DrawTraitSection(Rect outRect, float row)
        {
            const float traitHeight = 24f;
            //GUI.DrawTexture(outRect, BaseContent.BlackTex);
            Text.Anchor = TextAnchor.MiddleLeft;
            Text.Font = GameFont.Medium;
            Rect title = outRect.TopPartPixels(24f);
            Rect allTraitRect = new Rect(title.x, outRect.y + 24f, title.width, 8 * 16f);
            //GUI.DrawTexture(allTraitRect, BaseContent.WhiteTex);
            Widgets.DrawTitleBG(title);
            Widgets.Label(title, "AndroidCustomizationTraitsLabel".Translate());
            if (Widgets.ButtonText(title.RightPartPixels(36f), "AddTrait"))
            {
                PickTraitMenu(null);
            }
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Trait trait1 = null;
            float width = 256f;
            Rect traitViewRect = new Rect(allTraitRect);
            //GUI.DrawTexture(traitViewRect, BaseContent.GreyTex);
            Widgets.BeginScrollView(allTraitRect, ref this.traitsScrollPosition, new Rect(traitViewRect)
            {
                x = 0f,
                y = 0f,
                height = traitHeight * 8,
                width = allTraitRect.width - 24f
            });
            float traitPos = 0f;
            foreach (Trait allTrait in this.newAndroid.story.traits.allTraits)
            {
                Rect traitRect = new Rect(0f, traitPos, allTraitRect.width - 24f, traitHeight);
                //GUI.DrawTexture(traitRect, BaseContent.BadTex);
                Widgets.DrawBox(traitRect);
                Widgets.DrawHighlightIfMouseover(traitRect);
                Rect rect20 = new Rect(traitRect);
                rect20.width -= rect20.height;
                //GUI.DrawTexture(rect20, BaseContent.YellowTex);
                Rect butRect = new Rect(traitRect);
                butRect.width = butRect.height;
                butRect.x = rect20.xMax;
                if (this.originalTraits.Any(otherTrait => otherTrait.def == allTrait.def && otherTrait.Degree == allTrait.Degree))
                    Widgets.Label(rect20, "<" + allTrait.LabelCap + ">");
                else
                    Widgets.Label(rect20, allTrait.LabelCap);
                TooltipHandler.TipRegion(rect20, (TipSignal)allTrait.TipString(this.newAndroid));
                if (Widgets.ButtonInvisible(rect20))
                    PickTraitMenu(allTrait);
                if (Widgets.ButtonImage(butRect, TexCommand.ForbidOn))
                    trait1 = allTrait;
                traitPos += traitHeight + UIMargin;
            }
            Text.Anchor = TextAnchor.MiddleRight;
            Rect source2 = new Rect(traitViewRect.xMax, traitPos, width, traitHeight);
            Rect rect21 = new Rect(source2);
            rect21.width -= rect21.height;
            Rect butRect1 = new Rect(source2);
            GUI.DrawTexture(rect21, BaseContent.YellowTex);
            GUI.DrawTexture(butRect1, BaseContent.WhiteTex);
            butRect1.width = butRect1.height;
            butRect1.x = rect21.xMax;
            Widgets.Label(rect21, "AndroidCustomizationAddTraitLabel".Translate((NamedArgument)this.newAndroid.story.traits.allTraits.Count, (NamedArgument)AndroidCustomizationTweaks.maxTraitsToPick));
            if (Widgets.ButtonImage(butRect1, TexCommand.Install) && this.newAndroid.story.traits.allTraits.Count < AndroidCustomizationTweaks.maxTraitsToPick)
                this.PickTraitMenu(null);
            Widgets.EndScrollView();
            Text.Anchor = TextAnchor.UpperLeft;
            if (trait1 != null)
            {
                this.newAndroid.story.traits.allTraits.Remove(trait1);
                this.RefreshPawn();
            }
            return row;
        }

        private void DrawUpgradeSection(Rect inRect, ref float row)
        {
            //Upgrades
            float rowHeight = 32f;
            Rect upgradesTitleRect = new Rect(inRect.x, row, inRect.width, rowHeight);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(upgradesTitleRect, "AndroidCustomizationUpgrades".Translate());
            Widgets.DrawLineHorizontal(upgradesTitleRect.x, upgradesTitleRect.y + 32f, upgradesTitleRect.width);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            row += 35f;
            Rect upgradeSizeBase = new Rect(0.0f, 0.0f, AndroidCustomizationTweaks.upgradeBaseSize, AndroidCustomizationTweaks.upgradeBaseSize);
            int itemsPerRow = (int)Math.Floor(inRect.width / upgradeSizeBase.width);

            Rect outerUpgradesFrameRect = new Rect(inRect)
            {
                y = row,
                height = inRect.height - row - 16f,
            };

            float innerUpgradesHeight = 0.0f;
            foreach (AndroidUpgradeGroupDef allDef in DefDatabase<AndroidUpgradeGroupDef>.AllDefs)
            {
                innerUpgradesHeight += allDef.calculateNeededHeight(upgradeSizeBase, outerUpgradesFrameRect.width - 16f);
                innerUpgradesHeight += 52f;
            }
            Rect innerUpgradesFrameRect = new Rect(outerUpgradesFrameRect)
            {
                x = 0f,
                y = 0f,
                height = innerUpgradesHeight,
                width = outerUpgradesFrameRect.width - 16f
            };
            float innerRow = 0f;
            //upgradesScrollPosition
            Widgets.BeginScrollView(outerUpgradesFrameRect, ref this.upgradesScrollPosition, innerUpgradesFrameRect);
            foreach (AndroidUpgradeGroupDef androidUpgradeGroupDef in DefDatabase<AndroidUpgradeGroupDef>.AllDefs.OrderBy(upgradeGroup => upgradeGroup.orderID))
            {
                Rect groupTitleRect = new Rect(upgradesTitleRect)
                {
                    x = 0f,
                    y = innerRow,
                    height = 32f,
                    width = innerUpgradesFrameRect.width
                };
                innerRow += 30f;
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.DrawTitleBG(groupTitleRect);
                Widgets.Label(groupTitleRect, androidUpgradeGroupDef.label);
                Widgets.DrawLineHorizontal(groupTitleRect.x, groupTitleRect.y + 22f, groupTitleRect.width);
                Text.Anchor = TextAnchor.UpperLeft;
                float neededHeight = androidUpgradeGroupDef.calculateNeededHeight(upgradeSizeBase, outerUpgradesFrameRect.width);
                int upgradeItem = 0;
                float upgradeItemRow = 0.0f;
                foreach (AndroidUpgradeDef upgradeDef in androidUpgradeGroupDef.Upgrades.OrderBy(upgradeSubGroup => upgradeSubGroup.orderID))
                {
                    if (upgradeItem >= itemsPerRow)
                    {
                        upgradeItem = 0;
                        upgradeItemRow += upgradeSizeBase.height;
                    }
                    Rect upgradeItemRect = new Rect(upgradeSizeBase.width * upgradeItem, innerRow + upgradeItemRow, upgradeSizeBase.width, upgradeSizeBase.height);

                    //Button
                    bool needsFulfilled = false;
                    if (Mouse.IsOver(upgradeItemRect))
                    {
                        StringBuilder tooltip = new StringBuilder();
                        tooltip.AppendLine(upgradeDef.label);
                        tooltip.AppendLine();
                        tooltip.AppendLine(upgradeDef.description);
                        tooltip.AppendLine();
                        if (upgradeDef.hediffToApply != null && upgradeDef.hediffToApply.ConcreteExample != null)
                        {
                            //Using TipStringExtra will result in null reference hediff instance since there's no pawn have this hediff yet. Manually send null for Hediff instance solve this problem.
                            foreach (StatDrawEntry item in HediffStatsUtility.SpecialDisplayStats(upgradeDef.hediffToApply.ConcreteExample.CurStage, null))
                            {
                                if (item.ShouldDisplay())
                                    tooltip.AppendLine("  - " + item.LabelCap + ": " + item.ValueString);
                            }
                            //stringBuilder.AppendLine(upgrade.hediffToApply.ConcreteExample.TipStringExtra.TrimEndNewlines());
                            tooltip.AppendLine();
                        }
                        if (upgradeDef.newBodyType != null)
                        {
                            tooltip.AppendLine((string)"AndroidCustomizationChangeBodyType".Translate());
                            tooltip.AppendLine();
                        }
                        if (upgradeDef.changeSkinColor)
                        {
                            tooltip.AppendLine((string)"AndroidCustomizationChangeSkinColor".Translate());
                            tooltip.AppendLine();
                        }
                        tooltip.AppendLine(this.androidPrinter.FormatIngredientCosts(out needsFulfilled, upgradeDef.costList, false));
                        tooltip.AppendLine((string)("AndroidCustomizationTimeCost".Translate() + ": " + upgradeDef.extraPrintingTime.ToStringTicksToPeriodVerbose()));
                        if (upgradeDef.requiredResearch != null && !upgradeDef.requiredResearch.IsFinished)
                        {
                            tooltip.AppendLine();
                            tooltip.AppendLine((string)("AndroidCustomizationRequiredResearch".Translate() + ": " + upgradeDef.requiredResearch.LabelCap));
                        }
                        TooltipHandler.TipRegion(upgradeItemRect, (TipSignal)tooltip.ToString());
                    }

                    //(upgrade.requiredResearch != null && upgrade.requiredResearch.IsFinished)
                    //Checks applied upgrades that disables this upgrade
                    bool disabledUpgrade = upgradeDef.requiredResearch == null ?
                        this.appliedUpgradeCommands.Any(appUpgrade => appUpgrade.def != upgradeDef && appUpgrade.def.exclusivityGroups.Any(group => upgradeDef.exclusivityGroups.Contains(group))) : !upgradeDef.requiredResearch.IsFinished || this.appliedUpgradeCommands.Any(appUpgrade => appUpgrade.def != upgradeDef && appUpgrade.def.exclusivityGroups.Any(group => upgradeDef.exclusivityGroups.Contains(group)));

                    //Checks upgrades to disable when this upgrade is applied
                    if (this.AlreadyUpgradedOnPawn(upgradeDef))
                    {
                        foreach (AndroidUpgradeDef androidUpgradeDef2 in androidUpgradeGroupDef.Upgrades.OrderBy(upgradeSubGroup => upgradeSubGroup.orderID))
                        {
                            if (androidUpgradeDef2 != upgradeDef && androidUpgradeDef2.exclusivityGroups.Any(upgradeDef.exclusivityGroups.Contains))
                            {
                                if (!this.upgradedDefsToDisable.NullOrEmpty())
                                {
                                    if (!this.upgradedDefsToDisable.Contains(androidUpgradeDef2))
                                        this.upgradedDefsToDisable.Add(androidUpgradeDef2);
                                }
                                else
                                    this.upgradedDefsToDisable.Add(androidUpgradeDef2);
                            }
                        }
                    }
                    if (this.upgradedDefsToDisable?.Contains(upgradeDef) ?? false)
                        disabledUpgrade = true;
                    if (disabledUpgrade)
                    {
                        Widgets.DrawRectFast(upgradeItemRect, Color.red);
                    }
                    else
                    {
                        if (this.appliedUpgradeCommands.Any(upgradeCommand => upgradeCommand.def == upgradeDef))
                            Widgets.DrawRectFast(upgradeItemRect, Color.white);
                        if (this.AlreadyUpgradedOnPawn(upgradeDef))
                            Widgets.DrawRectFast(upgradeItemRect, Color.white);
                    }
                    if (upgradeDef.iconTexturePath != null)
                        Widgets.DrawTextureFitted(upgradeItemRect.ContractedBy(3f), ContentFinder<Texture2D>.Get(upgradeDef.iconTexturePath), 1f);
                    Widgets.DrawHighlightIfMouseover(upgradeItemRect);
                    UpgradeCommand upgradeCommand1 = this.appliedUpgradeCommands.FirstOrDefault((Func<UpgradeCommand, bool>)(upgradeCommand => upgradeCommand.def == upgradeDef));
                    if (!disabledUpgrade && Widgets.ButtonInvisible(upgradeItemRect) && !this.AlreadyUpgradedOnPawn(upgradeDef))
                    {
                        if (upgradeCommand1 != null)
                        {
                            upgradeCommand1.Undo();
                            this.appliedUpgradeCommands.Remove(upgradeCommand1);
                            this.androidPrinter.upgradesToApply.Remove(upgradeDef);
                        }
                        else
                        {
                            UpgradeCommand upgradeCommand2 = UpgradeMaker.Make(upgradeDef, this);
                            this.androidPrinter.upgradesToApply.Add(upgradeDef);
                            //upgradeCommand2.Apply();
                            upgradeCommand2.Notify_UpgradeAdded();
                            this.appliedUpgradeCommands.Add(upgradeCommand2);
                        }
                        this.RefreshCosts();
                    }
                    upgradeCommand1?.ExtraOnGUI(upgradeItemRect);
                    ++upgradeItem;
                }
                innerRow += neededHeight + 22f;
            }
            Widgets.EndScrollView();
        }

        public void PickTraitMenu(Trait oldTrait)
        {
            this.allTraits.Clear();
            foreach (TraitDef def in DefDatabase<TraitDef>.AllDefsListForReading)
            {
                foreach (TraitDegreeData degreeData in def.degreeDatas)
                    this.allTraits.Add(new Trait(def, degreeData.degree));
            }
            if (this.newAndroid.def is ThingDef_AlienRace def1)
            {
                List<Trait> disallowedTraits = def1.alienRace?.generalSettings?.disallowedTraits?.Where(x => x.chance == 0f).Select(x => new Trait(x.entry.def, x.entry.degree)).ToList();
                this.allTraits.RemoveAll(x => disallowedTraits.Exists(y => y.def == x.def && y.Degree == x.Degree));
            }
            foreach (Trait allTrait in this.newAndroid.story.traits.allTraits)
            {
                Trait trait = allTrait;
                this.allTraits.RemoveAll(aTrait => aTrait.def == trait.def);
                this.allTraits.RemoveAll(aTrait => trait.def.conflictingTraits.Contains(aTrait.def));
            }
            FloatMenuUtility.MakeMenu(allTraits, labelTrait => this.originalTraits.Any(originalTrait => originalTrait.def == labelTrait.def && originalTrait.Degree == labelTrait.Degree) ? (string)"AndroidCustomizationOriginalTraitFloatMenu".Translate((NamedArgument)labelTrait.LabelCap) : labelTrait.LabelCap, theTrait => () =>
            {
                this.replacedTrait = oldTrait;
                this.newTrait = theTrait;
            });
        }

        public void RefreshUpgrades()
        {
            foreach (UpgradeCommand appliedUpgradeCommand in this.appliedUpgradeCommands)
                appliedUpgradeCommand.Apply();
            this.refreshAndroidPortrait = true;
        }

        void RefreshCosts()
        {
            this.finalCalculatedPrintingCost.Clear();
            this.finalExtraPrintingTimeCost = 0;
            PawnCrafterProperties modExtension = this.androidPrinter.def.GetModExtension<PawnCrafterProperties>();
            if (!this.IsUpgrade)
            {
                foreach (ThingOrderRequest cost in modExtension.costList)
                    this.finalCalculatedPrintingCost.Add(new ThingOrderRequest()
                    {
                        amount = cost.amount,
                        nutrition = cost.nutrition,
                        thingDef = cost.thingDef
                    });
            }
            List<ThingDef> source = new List<ThingDef>();
            foreach (UpgradeCommand appliedUpgradeCommand in this.appliedUpgradeCommands)
            {
                foreach (ThingOrderRequest cost in appliedUpgradeCommand.def.costList)
                {
                    ThingOrderRequest upgradeCost = cost;
                    ThingOrderRequest thingOrderRequest = this.finalCalculatedPrintingCost.FirstOrDefault((Func<ThingOrderRequest, bool>)(finalCost =>
                   {
                       if (finalCost.thingDef == upgradeCost.thingDef)
                           return true;
                       return finalCost.nutrition && upgradeCost.nutrition;
                   }));
                    if (thingOrderRequest != null)
                        thingOrderRequest.amount += upgradeCost.amount;
                    else
                        this.finalCalculatedPrintingCost.Add(new ThingOrderRequest()
                        {
                            amount = upgradeCost.amount,
                            nutrition = upgradeCost.nutrition,
                            thingDef = upgradeCost.thingDef
                        });
                }
                source.AddRange(appliedUpgradeCommand.def.costsNotAffectedByBodySize);
                this.finalExtraPrintingTimeCost += appliedUpgradeCommand.def.extraPrintingTime;
            }
            if (source.Count > 0)
                source = new List<ThingDef>(source.Distinct());
            if (this._pawnTraits.NullOrEmpty() && this.clonedPawn != null)
                this.UpdatePawnTraits(this.clonedPawn);
            //Hair Def/Color Change Extra Time
            int HairColorChangeExtratime = 5000;
            int HairDefChangeExtratime = 10000;
            if (this.newAndroid.story.hairDef != this.originalHairDef)
                this.finalExtraPrintingTimeCost += HairDefChangeExtratime;
            if (this.newAndroid.story.HairColor != this.originalHairColor)
                this.finalExtraPrintingTimeCost += HairColorChangeExtratime;
            //Trait Change Extra Time
            int num3 = 45000;
            int num4 = 0;
            foreach (Trait allTrait in this.newAndroid.story.traits.allTraits)
            {
                Trait trait = allTrait;
                num4 += num3;
                if (this._pawnTraits != null && this._pawnTraits.Contains(trait))
                    num4 -= num3;
                if (this.originalTraits.Any(originalTrait => originalTrait.def == trait.def && originalTrait.Degree == trait.Degree))
                    num4 -= num3;
            }
            foreach (Trait originalTrait1 in this.originalTraits)
            {
                Trait originalTrait = originalTrait1;
                if ((this._pawnTraits == null || !this._pawnTraits.Contains(originalTrait)) && !this.newAndroid.story.traits.allTraits.Any(trait => originalTrait.def == trait.def && originalTrait.Degree == trait.Degree))
                    num4 += num3;
            }
            this.finalExtraPrintingTimeCost += num4;
            foreach (ThingOrderRequest thingOrderRequest in this.finalCalculatedPrintingCost)
            {
                if (!source.Contains(thingOrderRequest.thingDef))
                    thingOrderRequest.amount = (float)Math.Ceiling(thingOrderRequest.amount * (double)this.newAndroid.def.race.baseBodySize);
            }
        }

        public void RefreshPawn()
        {
            this.newAndroid.Notify_DisabledWorkTypesChanged();
            this.newAndroid.skills.Notify_SkillDisablesChanged();
            this.RefreshSkills();
            this.RefreshUpgrades();
            this.RefreshCosts();
        }

        private void RefreshSkills()
        {
            Log.Message("Refreshing skills for " + this.newAndroid.Name.ToStringShort);
            List<SkillDef> defsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
            for (int index = 0; index < defsListForReading.Count; ++index)
            {
                SkillDef skillDef = defsListForReading[index];
                int skillLvl = FinalLevelOfSkill(this.newAndroid, skillDef);
                SkillRecord skill = this.newAndroid.skills.GetSkill(skillDef);
                skill.Level = skillLvl;
                skill.passion = Passion.None;
                if (!skill.TotallyDisabled)
                {
                    float num2 = skillLvl * 0.11f;
                    float num3 = Rand.Value;
                    if ((double)num3 < (double)num2)
                        skill.passion = (double)num3 >= (double)num2 * 0.20000000298023224 ? Passion.Minor : Passion.Major;
                }
            }
        }

        private static int FinalLevelOfSkill(Pawn pawn, SkillDef sk)
        {
            float x = !sk.usuallyDefinedInBackstories ? Rand.ByCurve(LevelRandomCurve) : Rand.RangeInclusive(0, 4);

            if (pawn.story.Childhood.skillGains.Count > 0)
            {
                x += pawn.story.Childhood.skillGains.FindAll(x => x.skill == sk).Sum(gain => gain.amount);
            }
            if (pawn.story.Adulthood.skillGains.Count > 0)
            {
                x += pawn.story.Adulthood.skillGains.FindAll(x => x.skill == sk).Sum(gain => gain.amount);
            }
            if (pawn.story.traits.allTraits.Count > 0)
            {
                x += pawn.story.traits.allTraits
                    .SelectMany(trait => trait.CurrentData.skillGains)
                    .Where(gain => gain.skill == sk)
                    .Sum(gain => gain.amount);
            }

            return Mathf.Clamp(Mathf.RoundToInt(LevelFinalAdjustmentCurve.Evaluate(x)), 0, 20);
        }

        private Pawn GenerateNewPawn(Gender gender = Gender.Female)
        {
            Pawn pawn;
            var kindDef = this.currentPawnKindDef;
            var faction = this.androidPrinter.Faction;

            HarmonyPatches.bypassGenerationOfUpgrades = true;

            PawnGenerationRequest request;
            if (kindDef.race != ThingDefOf.ChjAndroid)
            {
                request = new PawnGenerationRequest(
                    kindDef,
                    faction,
                    forceGenerateNewPawn: true,
                    canGeneratePawnRelations: false,
                    colonistRelationChanceFactor: 0f,
                    allowGay: false,
                    allowPregnant: false,
                    allowAddictions: false,
                    forceRedressWorldPawnIfFormerColonist: true,
                    fixedGender: gender
                );
                pawn = PawnGenerator.GeneratePawn(request);
                AndroidUtility.Androidify(pawn);
                long ticks = 64800000;
                pawn.ageTracker.AgeBiologicalTicks = ticks;
                pawn.ageTracker.AgeChronologicalTicks = ticks;
            }
            else
            {
                request = new PawnGenerationRequest(
                    kindDef,
                    faction,
                    forceGenerateNewPawn: true,
                    canGeneratePawnRelations: false,
                    colonistRelationChanceFactor: 0f,
                    allowGay: false,
                    allowPregnant: false,
                    allowAddictions: false,
                    forceRedressWorldPawnIfFormerColonist: true,
                    fixedBiologicalAge: 20f,
                    fixedChronologicalAge: 20f,
                    fixedGender: gender
                );
                pawn = PawnGenerator.GeneratePawn(request);
            }
            PawnComponentsUtility.CreateInitialComponents(pawn);
            HarmonyPatches.bypassGenerationOfUpgrades = false;

            pawn?.equipment.DestroyAllEquipment();
            pawn?.inventory.DestroyAll();
            pawn?.apparel.DestroyAll();
            if (pawn.apparel.CanWearWithoutDroppingAnything(ThingDefOf.ChJAndroidThermalBandages))
                pawn.apparel.Wear((Apparel)ThingMaker.MakeThing(ThingDefOf.ChJAndroidThermalBandages, ThingDef.Named("Synthread")));

            pawn.workSettings?.EnableAndInitialize();
            pawn.skills?.Notify_SkillDisablesChanged();
            if (!pawn.Dead && pawn.RaceProps.Humanlike)
                pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();

            this.originalTraits.Clear();
            foreach (var trait in pawn.story.traits.allTraits)
                this.originalTraits.Add(new Trait(trait.def, trait.Degree, trait.ScenForced));

            return pawn;
        }

        private Pawn Duplicate(Pawn pawn)
        {
            // Borrowed from RimWorld.GameComponent_PawnDuplicator
            float ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
            float num = pawn.ageTracker.AgeChronologicalYearsFloat;
            if (num > ageBiologicalYearsFloat)
            {
                num = ageBiologicalYearsFloat;
            }
            PawnGenerationRequest request = new PawnGenerationRequest(pawn.kindDef, pawn.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: false, mustBeCapableOfViolence: false, 0f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 0f, null, null, null, null, null, fixedGender: pawn.gender, fixedIdeo: pawn.Ideo, fixedBiologicalAge: ageBiologicalYearsFloat, fixedChronologicalAge: num, fixedLastName: null, fixedBirthName: null, fixedTitle: null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: true, forceDead: false, forcedXenogenes: null, forcedEndogenes: null, forcedXenotype: pawn.genes.Xenotype, forcedCustomXenotype: pawn.genes.CustomXenotype, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Adult, pawnKindDefGetter: null, excludeBiologicalAgeRange: null, biologicalAgeRange: null, forceRecruitable: false, dontGiveWeapon: false, onlyUseForcedBackstories: false, maximumAgeTraits: -1, minimumAgeTraits: 0, forceNoGear: true);
            request.ForceNoIdeoGear = true;
            request.CanGeneratePawnRelations = false;
            request.DontGivePreArrivalPathway = true;


            HarmonyPatches.bypassGenerationOfUpgrades = true;
            Pawn pawn2 = PawnGenerator.GeneratePawn(request);
            HarmonyPatches.bypassGenerationOfUpgrades = false;
            pawn2.Name = NameTriple.FromString(pawn.Name.ToString());
            if (ModsConfig.BiotechActive)
            {
                pawn2.ageTracker.growthPoints = pawn.ageTracker.growthPoints;
                pawn2.ageTracker.vatGrowTicks = pawn.ageTracker.vatGrowTicks;
                pawn2.genes.xenotypeName = pawn.genes.xenotypeName;
                pawn2.genes.iconDef = pawn.genes.iconDef;
            }
            pawn2.needs.SetInitialLevels();
            pawn2.ageTracker = pawn.ageTracker;
            PawnUpdate(pawn2, pawn);
            pawn2?.equipment.DestroyAllEquipment();
            pawn2?.inventory.DestroyAll();
            pawn2.apparel.DestroyAll();
            if (pawn2.apparel.CanWearWithoutDroppingAnything(ThingDefOf.ChJAndroidThermalBandages))
                pawn2.apparel.Wear((Apparel)ThingMaker.MakeThing(ThingDefOf.ChJAndroidThermalBandages, ThingDef.Named("Synthread")));
            return pawn2;
        }

        public static void PawnUpdate(Pawn pawn, Pawn sourcePawn)
        {
            GenerateSkillsFromSourcePawn(pawn, sourcePawn);
            GenerateBioFromSource(pawn, sourcePawn);
            pawn.story.Childhood = sourcePawn.story.Childhood;
            pawn.story.Adulthood = sourcePawn.story.Adulthood;
            pawn.story.headType = sourcePawn.story.headType;
            pawn.story.bodyType = sourcePawn.story.bodyType;
            pawn.story.hairDef = sourcePawn.story.hairDef;
            pawn.story.HairColor = sourcePawn.story.HairColor;
            GenerateTraitsFromSourcePawn(pawn, sourcePawn);
            GenerateHediffsFromSourcePawn(pawn, sourcePawn);
            pawn.skills?.Notify_SkillDisablesChanged();
            if (pawn.workSettings == null)
                return;
            pawn.workSettings.EnableAndInitialize();
        }

        public static void GenerateSkillsFromSourcePawn(Pawn pawn, Pawn sourcePawn)
        {
            foreach (SkillRecord skill in pawn.skills.skills)
            {
                SkillRecord skillRecord = sourcePawn.skills.GetSkill(skill.def);
                if (skillRecord != null)
                {
                    skill.Level = skillRecord.Level;
                    skill.xpSinceLastLevel = skillRecord.xpSinceLastLevel;
                    skill.passion = skillRecord.passion;
                }
            }
        }

        public static void GenerateTraitsFromSourcePawn(Pawn pawn, Pawn sourcePawn)
        {
            List<Trait> traitList1 = new List<Trait>();
            List<Trait> traitList2 = new List<Trait>();
            foreach (Trait allTrait1 in pawn.story.traits.allTraits)
            {
                foreach (Trait allTrait2 in sourcePawn.story.traits.allTraits)
                {
                    if (!pawn.story.traits.allTraits.Contains(allTrait2))
                        traitList2.Add(allTrait2);
                    if (!sourcePawn.story.traits.allTraits.Contains(allTrait1))
                        traitList1.Add(allTrait1);
                }
            }
            foreach (Trait trait in traitList1)
                pawn.story.traits.allTraits.Remove(trait);
            foreach (Trait trait in traitList2)
            {
                if (!pawn.story.traits.HasTrait(trait.def))
                    pawn.story.traits.allTraits.Add(trait);
            }
        }

        public static void GenerateHediffsFromSourcePawn(Pawn pawn, Pawn sourcePawn)
        {
            List<Hediff> hediffList1 = new List<Hediff>();
            List<Hediff> hediffList2 = new List<Hediff>();
            foreach (Hediff hediff1 in pawn.health.hediffSet.hediffs)
            {
                foreach (Hediff hediff2 in sourcePawn.health.hediffSet.hediffs)
                {
                    if (!pawn.health.hediffSet.HasHediff(hediff2.def))
                        hediffList2.Add(hediff2);
                    if (!sourcePawn.health.hediffSet.HasHediff(hediff1.def))
                        hediffList1.Add(hediff1);
                }
            }
            foreach (Hediff hediff in hediffList1)
                pawn.health.hediffSet.hediffs.Remove(hediff);
            foreach (Hediff hediff in hediffList2)
                pawn.health.hediffSet.hediffs.Add(hediff);
        }

        public static void GenerateBioFromSource(Pawn pawn, Pawn sourcePawn)
        {
            sourcePawn.Name = CurPawnName(sourcePawn);
            NameTriple name = sourcePawn.Name as NameTriple;
            pawn.Name = name;
        }

        private static NameTriple CurPawnName(Pawn pawn)
        {
            if (pawn.Name is NameTriple name)
                return new NameTriple(name.First, name.Nick, name.Last);
            throw new InvalidOperationException();
        }
    }
}
