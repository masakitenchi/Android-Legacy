// Decompiled with JetBrains decompiler
// Type: Androids.Integration.Androids
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using UnityEngine;
using Verse;

namespace Androids.Integration
{
    public class Androids : Mod
    {
        public static Integration.Androids Instance;
        private string explosionRadiusBuffer = "3.5";

        public Androids(ModContentPack content)
          : base(content)
        {
            Instance = this;
            AndroidsModSettings.Instance = this.GetSettings<AndroidsModSettings>();
            if (AndroidsModSettings.Instance == null)
                return;
            this.explosionRadiusBuffer = AndroidsModSettings.Instance.androidExplosionRadius.ToString();
        }

        public override string SettingsCategory() => nameof(Androids);

        public override void DoSettingsWindowContents(Rect inRect)
        {
            int row = 0;
            float rowHeight = 24f;
            Rect ininRect = new Rect(inRect);
            ininRect.width /= 2f;
            Widgets.CheckboxLabeled(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsEyeGlow".Translate(), ref AndroidsModSettings.Instance.androidEyeGlow);
            row++;
            Widgets.CheckboxLabeled(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsExplodeOnDeath".Translate(), ref AndroidsModSettings.Instance.androidExplodesOnDeath);
            row++;
            Widgets.TextFieldNumericLabeled(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsExplosionRadius".Translate(), ref AndroidsModSettings.Instance.androidExplosionRadius, ref explosionRadiusBuffer, 1.25f, GenRadial.MaxRadialPatternRadius);
            row++;
            Widgets.CheckboxLabeled(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsDroidCompatibilityMode".Translate(), ref AndroidsModSettings.Instance.droidCompatibilityMode);
            TooltipHandler.TipRegion(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsDroidCompatibilityModeTooltip".Translate());
            row++;
            Widgets.CheckboxLabeled(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsDroidDetonationDialog".Translate(), ref AndroidsModSettings.Instance.droidDetonationConfirmation);
            TooltipHandler.TipRegion(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsDroidDetonationDialogTooltip".Translate());
            row++;
            Widgets.CheckboxLabeled(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsDroidWearDown".Translate(), ref AndroidsModSettings.Instance.droidWearDown);
            row++;
            Widgets.CheckboxLabeled(UIHelper.GetRowRect(ininRect, rowHeight, row), "AndroidSettingsDroidWearDownQuadrum".Translate(), ref AndroidsModSettings.Instance.droidWearDownQuadrum);

        }
    }
}
