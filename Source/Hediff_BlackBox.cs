// Decompiled with JetBrains decompiler
// Type: Androids.Hediff_BlackBox
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Androids
{
  public class Hediff_BlackBox : HediffWithComps, IExtraGizmos
  {
    public override void Notify_PawnDied()
    {
      base.Notify_PawnDied();
      if (this.pawn.Corpse == null)
        return;
      GenExplosion.DoExplosion(this.pawn.Corpse.Position, this.pawn.Corpse.Map, 50f, RimWorld.DamageDefOf.Bomb, (Thing) null, 500, 15f);
    }

    public IEnumerable<Gizmo> GetGizmosExtra()
    {
      Command_Action commandAction = new Command_Action();
      commandAction.defaultLabel = (string) "AndroidGizmoDetonateBlackBoxLabel".Translate();
      commandAction.defaultDesc = (string) "AndroidGizmoDetonateBlackBoxDescription".Translate();
      commandAction.icon = (Texture) ContentFinder<Texture2D>.Get("Icons/Upgrades/BlackBoxIcon");
      commandAction.Order = -97f;
      commandAction.action = (Action) (() => Find.WindowStack.Add((Window) Dialog_MessageBox.CreateConfirmation("AndroidSelfDetonationConfirmationDialogText".Translate((NamedArgument) this.pawn.Name.ToStringFull), (Action) (() => this.pawn.Kill(null)), true, (string) "AndroidGizmoSelfDetonationLabel".Translate())));
      yield return (Gizmo) commandAction;
    }

    public override string TipStringExtra => (string) "AndroidHediffBlackBox".Translate();
  }
}
