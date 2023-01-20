// Decompiled with JetBrains decompiler
// Type: Androids.Gizmo_EnergySourceFueled
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Androids
{
  [StaticConstructorOnStartup]
  public class Gizmo_EnergySourceFueled : Gizmo
  {
    public Apparel apparel;
    public EnergySource_Fueled fueledEnergySource;
    private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.5f, 0.0f));
    private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

    public Gizmo_EnergySourceFueled() => this.Order = -100f;

    public override float GetWidth(float maxWidth) => 140f;

    public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
    {
      Rect overRect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
      Find.WindowStack.ImmediateWindow(984689, overRect, WindowLayer.GameUI, (Action) (() =>
      {
        Rect rect1 = overRect.AtZero().ContractedBy(6f);
        Rect rect2 = rect1 with
        {
          height = overRect.height / 2f
        };
        Text.Font = GameFont.Tiny;
        Widgets.Label(rect2, this.apparel.LabelCap);
        Rect rect3 = rect1 with
        {
          yMin = overRect.height / 2f
        };
        float fillPercent = 1f - this.fueledEnergySource.MissingFuelPercentage;
        Widgets.FillableBar(rect3, fillPercent, Gizmo_EnergySourceFueled.FullShieldBarTex, Gizmo_EnergySourceFueled.EmptyShieldBarTex, false);
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(rect3, this.fueledEnergySource.fuelAmountLoaded.ToString("F0") + " / " + this.fueledEnergySource.EnergyProps.maxFuelAmount.ToString("F0"));
        Text.Anchor = TextAnchor.UpperLeft;
      }));
      return new GizmoResult(GizmoState.Clear);
    }
  }
}
