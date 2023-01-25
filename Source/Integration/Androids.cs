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
      Integration.Androids.Instance = this;
      AndroidsModSettings.Instance = this.GetSettings<AndroidsModSettings>();
      if (AndroidsModSettings.Instance == null)
        return;
      this.explosionRadiusBuffer = AndroidsModSettings.Instance.androidExplosionRadius.ToString();
    }

    public override string SettingsCategory() => nameof (Androids);

    public override void DoSettingsWindowContents(Rect inRect)
    {
      int row1 = 0;
      float rowHeight = 24f;
      Rect inRect1 = new Rect(inRect);
      inRect1.width /= 2f;
      Rect rowRect1 = UIHelper.GetRowRect(inRect1, rowHeight, row1);
      int row2 = row1 + 1;
      string label1 = (string) "AndroidSettingsEyeGlow".Translate();
      ref bool local1 = ref AndroidsModSettings.Instance.androidEyeGlow;
      Widgets.CheckboxLabeled(rowRect1, label1, ref local1);
      Rect rowRect2 = UIHelper.GetRowRect(inRect1, rowHeight, row2);
      int row3 = row2 + 1;
      string label2 = (string) "AndroidSettingsExplodeOnDeath".Translate();
      ref bool local2 = ref AndroidsModSettings.Instance.androidExplodesOnDeath;
      Widgets.CheckboxLabeled(rowRect2, label2, ref local2);
      Rect rowRect3 = UIHelper.GetRowRect(inRect1, rowHeight, row3);
      int row4 = row3 + 1;
      string label3 = (string) "AndroidSettingsExplosionRadius".Translate();
      ref float local3 = ref AndroidsModSettings.Instance.androidExplosionRadius;
      ref string local4 = ref this.explosionRadiusBuffer;
      double radialPatternRadius = (double) GenRadial.MaxRadialPatternRadius;
      Widgets.TextFieldNumericLabeled<float>(rowRect3, label3, ref local3, ref local4, 1.25f, (float) radialPatternRadius);
      Rect rowRect4 = UIHelper.GetRowRect(inRect1, rowHeight, row4);
      int row5 = row4 + 1;
      Widgets.CheckboxLabeled(rowRect4, (string) "AndroidSettingsDroidCompatibilityMode".Translate(), ref AndroidsModSettings.Instance.droidCompatibilityMode);
      TooltipHandler.TipRegion(rowRect4, (TipSignal) "AndroidSettingsDroidCompatibilityModeTooltip".Translate());
      Rect rowRect5 = UIHelper.GetRowRect(inRect1, rowHeight, row5);
      int row6 = row5 + 1;
      Widgets.CheckboxLabeled(rowRect5, (string) "AndroidSettingsDroidDetonationDialog".Translate(), ref AndroidsModSettings.Instance.droidDetonationConfirmation);
      TooltipHandler.TipRegion(rowRect5, (TipSignal) "AndroidSettingsDroidDetonationDialogTooltip".Translate());
      Rect rowRect6 = UIHelper.GetRowRect(inRect1, rowHeight, row6);
      int row7 = row6 + 1;
      string label4 = (string) "AndroidSettingsDroidWearDown".Translate();
      ref bool local5 = ref AndroidsModSettings.Instance.droidWearDown;
      Widgets.CheckboxLabeled(rowRect6, label4, ref local5);
      Rect rowRect7 = UIHelper.GetRowRect(inRect1, rowHeight, row7);
      int num = row7 + 1;
      string label5 = (string) "AndroidSettingsDroidWearDownQuadrum".Translate();
      ref bool local6 = ref AndroidsModSettings.Instance.droidWearDownQuadrum;
      Widgets.CheckboxLabeled(rowRect7, label5, ref local6);
    }
  }
}
