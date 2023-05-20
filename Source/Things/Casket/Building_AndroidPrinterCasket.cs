// Decompiled with JetBrains decompiler
// Type: Androids.Building_AndroidPrinterCasket
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace Androids
{
    public class Building_AndroidPrinterCasket : Building_Casket, ISuspendableThingHolder
    {
        public bool IsContentsSuspended => (this as Building_AndroidPrinter).printerStatus == CrafterStatus.Crafting;
        public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (!base.TryAcceptThing(thing, allowSpecialEffects))
                return false;
            if (allowSpecialEffects)
                SoundDefOf.CryptosleepCasket_Accept.PlayOneShot((SoundInfo)new TargetInfo(this.Position, this.Map));
            (this as Building_AndroidPrinter).pawnToPrint = thing as Pawn;
            return true;
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            Building_AndroidPrinterCasket dest = this;
            foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(myPawn))
                yield return floatMenuOption;
            if (dest.innerContainer.Count == 0 && myPawn.IsAndroid())
            {
                if (!myPawn.CanReach((LocalTargetInfo)(Thing)dest, PathEndMode.InteractionCell, Danger.Deadly))
                {
                    yield return new FloatMenuOption((string)"CannotUseNoPath".Translate(), (Action)null);
                }
                else
                {
                    JobDef jobDef = DefDatabase<JobDef>.GetNamedSilentFail("EnterAndroidPrinterCasket");
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption((string)"EnterAndroidPrinter".Translate(), (Action)(() => myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(jobDef, (LocalTargetInfo)(Thing)this)))), myPawn, (LocalTargetInfo)(Thing)dest, "ReservedBy");
                }
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
                yield return gizmo;
            CrafterStatus printerStatus = (this as Building_AndroidPrinter).printerStatus;
            bool flag = printerStatus == CrafterStatus.Idle || printerStatus == CrafterStatus.Finished;
            if (base.Faction == Faction.OfPlayer && this.innerContainer.Count > 0 && this.def.building.isPlayerEjectable && flag)
            {
                Command_Action commandAction = new Command_Action();
                commandAction.action = new Action(((Building_Casket)this).EjectContents);
                commandAction.defaultLabel = (string)"AndroidPrinterEject".Translate();
                commandAction.defaultDesc = (string)"AndroidPrinterEjectDesc".Translate();
                if (this.innerContainer.Count == 0)
                    commandAction.Disable((string)"CommandPodEjectFailEmpty".Translate());
                commandAction.hotKey = KeyBindingDefOf.Misc1;
                commandAction.icon = (Texture)ContentFinder<Texture2D>.Get("UI/Commands/PodEject");
                yield return (Gizmo)commandAction;
            }
        }

        public override void Open()
        {
            CrafterStatus printerStatus = (this as Building_AndroidPrinter).printerStatus;
            (this as Building_AndroidPrinter).upgradesToApply.Clear();
            if (printerStatus != CrafterStatus.Idle && printerStatus != CrafterStatus.Finished) return;
            base.Open();
        }

        public override void EjectContents()
        {
            Find.WindowStack.TryRemove(typeof(CustomizeAndroidWindow), false);
            (this as Building_AndroidPrinter).upgradesToApply.Clear();
            foreach (Thing thing in (IEnumerable<Thing>)this.innerContainer)
            {
                if (thing is Pawn pawn)
                    PawnComponentsUtility.AddComponentsForSpawn(pawn);
            }
            if (!this.Destroyed)
                SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.Position, this.Map)));
            base.EjectContents();
        }

        public static Building_AndroidPrinterCasket FindCryptosleepCasketFor(
          Pawn p,
          Pawn traveler,
          bool ignoreOtherReservations = false)
        {
            foreach (ThingDef singleDef in DefDatabase<ThingDef>.AllDefs.Where<ThingDef>((Func<ThingDef, bool>)(def => typeof(Building_AndroidPrinterCasket).IsAssignableFrom(def.thingClass))))
            {
                Building_AndroidPrinterCasket cryptosleepCasketFor = (Building_AndroidPrinterCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(traveler), validator: ((Predicate<Thing>)(x => !((Building_Casket)x).HasAnyContents && traveler.CanReserve((LocalTargetInfo)x, ignoreOtherReservations: ignoreOtherReservations))));
                if (cryptosleepCasketFor != null)
                    return cryptosleepCasketFor;
            }
            return (Building_AndroidPrinterCasket)null;
        }
    }
}
