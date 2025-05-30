﻿// Decompiled with JetBrains decompiler
// Type: Androids.Gizmo_EnergySourceFueled
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
            Find.WindowStack.ImmediateWindow(984689, overRect, WindowLayer.GameUI, () =>
            {
                Rect rect1;
                Rect rect2 = (rect1 = overRect.AtZero().ContractedBy(6f)) with
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
                Widgets.FillableBar(rect3, fillPercent, FullShieldBarTex, EmptyShieldBarTex, false);
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(rect3, this.fueledEnergySource.fuelAmountLoaded.ToString("F0") + " / " + this.fueledEnergySource.EnergyProps.maxFuelAmount.ToString("F0"));
                Text.Anchor = TextAnchor.UpperLeft;
            });
            return new GizmoResult(GizmoState.Clear);
        }
    }
}
