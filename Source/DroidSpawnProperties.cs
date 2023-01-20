// Decompiled with JetBrains decompiler
// Type: Androids.DroidSpawnProperties
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class DroidSpawnProperties : DefModExtension
  {
    public List<DroidSkill> skills = new List<DroidSkill>();
    public int defaultSkillLevel = 0;
    public BackstoryDef backstory;
    public BodyTypeDef bodyType;
    public HeadTypeDef headType;
    public Gender gender = Gender.Male;
    public bool generateHair = false;
    public List<string> hairTags = new List<string>();
    public HostilityResponseMode hostileResponse = HostilityResponseMode.Flee;
  }
}
