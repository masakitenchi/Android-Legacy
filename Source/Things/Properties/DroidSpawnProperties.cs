// Decompiled with JetBrains decompiler
// Type: Androids.DroidSpawnProperties
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    public class DroidSpawnProperties : DefModExtension
    {
        public List<DroidSkill> skills = new List<DroidSkill>();
        public int defaultSkillLevel;
        public BackstoryDef backstory;
        public BodyTypeDef bodyType;
        public HeadTypeDef headType;
        public Gender gender = Gender.Male;
        public bool generateHair;
        public List<string> hairTags = new List<string>();
        public HostilityResponseMode hostileResponse = HostilityResponseMode.Flee;
    }
}
