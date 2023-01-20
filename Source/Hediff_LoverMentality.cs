// Decompiled with JetBrains decompiler
// Type: Androids.Hediff_LoverMentality
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Androids
{
  public class Hediff_LoverMentality : HediffWithComps, IExtraGizmos
  {
    public Pawn loverToChase;

    public override void ExposeData()
    {
      base.ExposeData();
      Scribe_References.Look<Pawn>(ref this.loverToChase, "loverToChase");
    }

    public override void Tick()
    {
      base.Tick();
      if (this.loverToChase == null || !this.loverToChase.Dead && !this.loverToChase.Destroyed)
        return;
      this.SetNewLover((Pawn) null);
    }

    public void SetNewLover(Pawn newLover)
    {
      if (this.loverToChase == newLover)
        return;
      this.loverToChase = newLover;
    }

    public IEnumerable<Gizmo> GetGizmosExtra()
    {
      Command_Action commandAction = new Command_Action();
      commandAction.defaultLabel = (string) "AndroidGizmoLoverMentalityLabel".Translate();
      commandAction.defaultDesc = (string) "AndroidGizmoLoverMentalityDescription".Translate();
      commandAction.icon = (Texture) ContentFinder<Texture2D>.Get("Icons/Upgrades/love-mystery");
      commandAction.Order = -97f;
      commandAction.action = (Action) (() =>
      {
        List<FloatMenuOption> options = new List<FloatMenuOption>();
        foreach (Pawn colonistsAndPrisoner in this.pawn.Map.mapPawns.FreeColonistsAndPrisoners)
        {
          Pawn targetPawn = colonistsAndPrisoner;
          FloatMenuOption floatMenuOption = new FloatMenuOption(targetPawn.LabelCap, (Action) (() =>
          {
            this.SetNewLover(targetPawn);
            FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Heart);
          }));
          options.Add(floatMenuOption);
        }
        FloatMenuOption floatMenuOption1 = new FloatMenuOption((string) "AndroidNone".Translate(), (Action) (() => this.SetNewLover((Pawn) null)));
        options.Add(floatMenuOption1);
        Find.WindowStack.Add((Window) new FloatMenu(options));
      });
      yield return (Gizmo) commandAction;
    }
  }
}
