// Decompiled with JetBrains decompiler
// Type: Androids.Integration.AndroidsModSettings
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids.Integration
{
    public class AndroidsModSettings : ModSettings
    {
        public static AndroidsModSettings Instance;
        public bool androidEyeGlow = true;
        public bool androidExplodesOnDeath;
        public float androidExplosionRadius = 1.5f;
        public bool droidCompatibilityMode;
        public bool droidDetonationConfirmation = true;
        public bool droidWearDown = true;
        public bool droidWearDownQuadrum = true;

        public AndroidsModSettings() => Instance = this;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref this.androidEyeGlow, "androidEyeGlow", true);
            Scribe_Values.Look(ref this.androidExplodesOnDeath, "androidExplodesOnDeath");
            Scribe_Values.Look(ref this.androidExplosionRadius, "androidExplosionRadius", 1.5f);
            Scribe_Values.Look(ref this.droidCompatibilityMode, "droidCompatibilityMode");
            Scribe_Values.Look(ref this.droidDetonationConfirmation, "droidDetonationConfirmation", true);
            Scribe_Values.Look(ref this.droidWearDown, "droidWearDown", true);
            Scribe_Values.Look(ref this.droidWearDownQuadrum, "droidWearDownQuadrum", true);
        }
    }
}
