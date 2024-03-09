using RimWorld;
using Verse;
using RimWorld.Planet;
using System.Collections.Generic;

namespace Androids.GameComponent
{
    public class UpgradeTracker : Verse.GameComponent
    {
        private static List<ResearchProjectDef> researches;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref researches, "researches");
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();
            if (researches is not null)
                researches.Clear();
            else
                researches = new();
        }
    }
}
