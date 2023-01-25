﻿// Decompiled with JetBrains decompiler
// Type: Androids.Hediff_LoverMentality
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

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
            this.SetNewLover((Pawn)null);
        }

        public void SetNewLover(Pawn newLover)
        {
            if (this.loverToChase == newLover)
                return;
            this.loverToChase = newLover;
        }

        public IEnumerable<Gizmo> GetGizmosExtra()
        {
            yield return new Command_Action
            {
                defaultLabel = "AndroidGizmoLoverMentalityLabel".Translate(),
                defaultDesc = "AndroidGizmoLoverMentalityDescription".Translate(),
                icon = ContentFinder<Texture2D>.Get("Icons/Upgrades/love-mystery"),
                Order = -97f,
                action = delegate
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    foreach (Pawn targetPawn in pawn.Map.mapPawns.FreeColonistsAndPrisoners)
                    {
                        FloatMenuOption item = new FloatMenuOption(targetPawn.LabelCap, delegate
                        {
                            SetNewLover(targetPawn);
                            FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.Heart);
                        });
                        list.Add(item);
                    }
                    FloatMenuOption item2 = new FloatMenuOption("AndroidNone".Translate(), delegate
                    {
                        SetNewLover(null);
                    });
                    list.Add(item2);
                    FloatMenu window = new FloatMenu(list);
                    Find.WindowStack.Add(window);
                }
            };
        }
    }
}
