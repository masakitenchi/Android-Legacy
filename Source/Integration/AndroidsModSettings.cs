// Decompiled with JetBrains decompiler
// Type: Androids.Integration.AndroidsModSettings
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using Verse;

namespace Androids.Integration
{
  public class AndroidsModSettings : ModSettings
  {
    public static AndroidsModSettings Instance;
    public bool androidEyeGlow = true;
    public bool androidExplodesOnDeath = true;
    public float androidExplosionRadius = 3.5f;
    public bool droidCompatibilityMode = false;
    public bool droidDetonationConfirmation = true;
    public bool droidWearDown = true;
    public bool droidWearDownQuadrum = true;

    public AndroidsModSettings() => AndroidsModSettings.Instance = this;

    public override void ExposeData()
    {
      Scribe_Values.Look<bool>(ref this.androidEyeGlow, "androidEyeGlow", true);
      Scribe_Values.Look<bool>(ref this.androidExplodesOnDeath, "androidExplodesOnDeath", true);
      Scribe_Values.Look<float>(ref this.androidExplosionRadius, "androidExplosionRadius", 3.5f);
      Scribe_Values.Look<bool>(ref this.droidCompatibilityMode, "droidCompatibilityMode");
      Scribe_Values.Look<bool>(ref this.droidDetonationConfirmation, "droidDetonationConfirmation", true);
      Scribe_Values.Look<bool>(ref this.droidWearDown, "droidWearDown", true);
      Scribe_Values.Look<bool>(ref this.droidWearDownQuadrum, "droidWearDownQuadrum", true);
    }
  }
}
