// Decompiled with JetBrains decompiler
// Type: Androids.Hediff_MechaniteHive
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

namespace Androids
{
    public class Hediff_MechaniteHive : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();
            if (!this.pawn.IsHashIntervalTick(2000))
                return;
            foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
            {
                if (hediff is Hediff_Injury hediffInjury && hediffInjury.Bleeding)
                    hediffInjury.Tended(1f, 1f, 0);
            }
        }

        public override string TipStringExtra => (string)"AndroidMechaniteHive".Translate();
    }
}
