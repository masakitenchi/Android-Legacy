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
            Rect rowRect = UIHelper.GetRowRect(ininRect, rowHeight, row);
            row++;
            Widgets.CheckboxLabeled(rowRect, "AndroidSettingsEyeGlow".Translate(), ref AndroidsModSettings.Instance.androidEyeGlow);
            row++;
            Widgets.CheckboxLabeled(rowRect, "AndroidSettingsExplodeOnDeath".Translate(), ref AndroidsModSettings.Instance.androidExplodesOnDeath);
            row++;
            Widgets.TextFieldNumericLabeled(rowRect, "AndroidSettingsExplosionRadius".Translate(), ref AndroidsModSettings.Instance.androidExplosionRadius, ref explosionRadiusBuffer, 1.25f, GenRadial.MaxRadialPatternRadius);
            row++;
            Widgets.CheckboxLabeled(rowRect, "AndroidSettingsDroidCompatibilityMode".Translate(), ref AndroidsModSettings.Instance.droidCompatibilityMode);
            TooltipHandler.TipRegion(rowRect, "AndroidSettingsDroidCompatibilityModeTooltip".Translate());
            row++;
            Widgets.CheckboxLabeled(rowRect, "AndroidSettingsDroidDetonationDialog".Translate(), ref AndroidsModSettings.Instance.droidDetonationConfirmation);
            TooltipHandler.TipRegion(rowRect, "AndroidSettingsDroidDetonationDialogTooltip".Translate());
            row++;
            Widgets.CheckboxLabeled(rowRect, "AndroidSettingsDroidWearDown".Translate(), ref AndroidsModSettings.Instance.droidWearDown);
            row++;
            Widgets.CheckboxLabeled(rowRect, "AndroidSettingsDroidWearDownQuadrum".Translate(), ref AndroidsModSettings.Instance.droidWearDownQuadrum);

        }
    }
}
