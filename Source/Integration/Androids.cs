// Decompiled with JetBrains decompiler
// Type: Androids.Integration.Androids
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

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
      Widgets.CheckboxLabeled(rowRect1, (string) "AndroidSettingsEyeGlow".Translate(), ref AndroidsModSettings.Instance.androidEyeGlow);
      Rect rowRect2 = UIHelper.GetRowRect(inRect1, rowHeight, row2);
      int row3 = row2 + 1;
      Widgets.CheckboxLabeled(rowRect2, (string) "AndroidSettingsExplodeOnDeath".Translate(), ref AndroidsModSettings.Instance.androidExplodesOnDeath);
      Rect rowRect3 = UIHelper.GetRowRect(inRect1, rowHeight, row3);
      int row4 = row3 + 1;
      Widgets.TextFieldNumericLabeled<float>(rowRect3, (string) "AndroidSettingsExplosionRadius".Translate(), ref AndroidsModSettings.Instance.androidExplosionRadius, ref this.explosionRadiusBuffer, 1.25f, GenRadial.MaxRadialPatternRadius);
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
      Widgets.CheckboxLabeled(rowRect6, (string) "AndroidSettingsDroidWearDown".Translate(), ref AndroidsModSettings.Instance.droidWearDown);
      Rect rowRect7 = UIHelper.GetRowRect(inRect1, rowHeight, row7);
      int num = row7 + 1;
      Widgets.CheckboxLabeled(rowRect7, (string) "AndroidSettingsDroidWearDownQuadrum".Translate(), ref AndroidsModSettings.Instance.droidWearDownQuadrum);
    }
  }
}
