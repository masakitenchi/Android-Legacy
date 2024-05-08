// Decompiled with JetBrains decompiler
// Type: Androids.Hediff_BlackBox
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Androids
{
    public class Hediff_BlackBox : HediffWithComps, IExtraGizmos
    {
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            if (this.pawn.Corpse == null)
                return;
            GenExplosion.DoExplosion(this.pawn.Corpse.Position, this.pawn.Corpse.Map, 50f, RimWorld.DamageDefOf.Bomb, null, 500, 15f, null, null, null, null, null, 0.0f, 1, null, false, null, 0.0f, 1, 0.0f, false, new float?(), null);
        }

        public IEnumerable<Gizmo> GetGizmosExtra()
        {
            yield return new Command_Action
            {
                defaultLabel = "AndroidGizmoDetonateBlackBoxLabel".Translate(),
                defaultDesc = "AndroidGizmoDetonateBlackBoxDescription".Translate(),
                icon = ContentFinder<Texture2D>.Get("Icons/Upgrades/BlackBoxIcon"),
                Order = -97f,
                action = delegate
                {
                    Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("AndroidSelfDetonationConfirmationDialogText".Translate(pawn.Name.ToStringFull), delegate
                    {
                        pawn.Kill(null, null);
                    }, destructive: true, "AndroidGizmoSelfDetonationLabel".Translate());
                    Find.WindowStack.Add(window);
                }
            };
        }

        public override string TipStringExtra => (string)"AndroidHediffBlackBox".Translate();
    }
}
