// Decompiled with JetBrains decompiler
// Type: Androids.UpgradeCommand_Body
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using AlienRace;
using RimWorld;
using Verse;

namespace Androids
{
  public class UpgradeCommand_Body : UpgradeCommand_Hediff
  {
    public BodyTypeDef originalBodyType;

    public override void Apply(Pawn customTarget = null)
    {
      base.Apply(customTarget);
      Pawn pawn = customTarget == null ? this.customizationWindow.newAndroid : customTarget;
      this.originalBodyType = pawn.story.bodyType;
      if (!(pawn.def is ThingDef_AlienRace def) || def.alienRace.generalSettings.alienPartGenerator.bodyTypes.Contains(this.def.newBodyType))
        pawn.story.bodyType = this.def.newBodyType;
      if (this.customizationWindow == null)
        return;
      this.customizationWindow.refreshAndroidPortrait = true;
    }

    public override void Undo()
    {
      base.Undo();
      this.customizationWindow.newAndroid.story.bodyType = this.originalBodyType;
      this.customizationWindow.refreshAndroidPortrait = true;
    }
  }
}
