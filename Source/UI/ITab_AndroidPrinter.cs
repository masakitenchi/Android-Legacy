// Decompiled with JetBrains decompiler
// Type: Androids.ITab_AndroidPrinter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using UnityEngine;
using Verse;

namespace Androids
{
    public class ITab_AndroidPrinter : ITab
    {
        private const float TopAreaHeight = 35f;
        private ThingFilterUI.UIState state;
        private static readonly Vector2 WinSize = new Vector2(300f, 480f);

        private IStoreSettingsParent SelStoreSettingsParent => (IStoreSettingsParent)this.SelObject;

        public override bool IsVisible => this.SelStoreSettingsParent.StorageTabVisible;

        public ITab_AndroidPrinter()
        {
            this.size = ITab_AndroidPrinter.WinSize;
            this.labelKey = "AndroidTab";
            this.state = new ThingFilterUI.UIState();
            this.state.scrollPosition = new Vector2();
        }

        protected override void FillTab()
        {
            IStoreSettingsParent storeSettingsParent = this.SelStoreSettingsParent;
            StorageSettings storeSettings = storeSettingsParent.GetStoreSettings();
            Rect rect = new Rect(0.0f, 0.0f, ITab_AndroidPrinter.WinSize.x, ITab_AndroidPrinter.WinSize.y).ContractedBy(10f);
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(rect) { height = 32f }, "AndroidTabTitle".Translate());
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            ThingFilter parentFilter = (ThingFilter)null;
            if (storeSettingsParent.GetParentStoreSettings() != null)
                parentFilter = storeSettingsParent.GetParentStoreSettings().filter;
            ThingFilterUI.DoThingFilterConfigWindow(new Rect(0.0f, 40f, rect.width, rect.height - 40f), this.state, storeSettings.filter, parentFilter, 8);
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.StorageTab, KnowledgeAmount.FrameDisplayed);
            GUI.EndGroup();
        }
    }
}
