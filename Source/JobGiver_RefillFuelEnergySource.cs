// Decompiled with JetBrains decompiler
// Type: Androids.JobGiver_RefillFuelEnergySource
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using Verse;
using Verse.AI;

namespace Androids
{
  public class JobGiver_RefillFuelEnergySource : ThinkNode_JobGiver
  {
    public JobDef refillJob;

    public override ThinkNode DeepCopy(bool resolve = true)
    {
      JobGiver_RefillFuelEnergySource fuelEnergySource = (JobGiver_RefillFuelEnergySource) base.DeepCopy(resolve);
      fuelEnergySource.refillJob = this.refillJob;
      return (ThinkNode) fuelEnergySource;
    }

    public override float GetPriority(Pawn pawn) => FuelUtility.FueledEnergySourceNeedRefilling(pawn) != null ? 10f : 0.0f;

    protected override Job TryGiveJob(Pawn pawn)
    {
      if (pawn.Downed || pawn.InBed())
        return (Job) null;
      Thing thing = FuelUtility.FueledEnergySourceNeedRefilling(pawn);
      if (thing == null)
        return (Job) null;
      EnergySource_Fueled comp = thing.TryGetComp<EnergySource_Fueled>();
      if (!comp.autoRefuel)
        return (Job) null;
      Thing suitableFuelForPawn = FuelUtility.FindSuitableFuelForPawn(pawn, comp);
      if (suitableFuelForPawn == null)
        return (Job) null;
      return new Job(this.refillJob, (LocalTargetInfo) thing, (LocalTargetInfo) suitableFuelForPawn)
      {
        count = comp.CalculateFuelNeededToRefill(suitableFuelForPawn)
      };
    }
  }
}
