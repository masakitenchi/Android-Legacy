// Decompiled with JetBrains decompiler
// Type: Androids.Recipe_Disassemble
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Androids
{
  public class Recipe_Disassemble : RecipeWorker
  {
    public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
    {
      if (pawn.def.HasModExtension<MechanicalPawnProperties>())
        yield return (BodyPartRecord) null;
    }

    public override void ApplyOnPawn(
      Pawn pawn,
      BodyPartRecord part,
      Pawn billDoer,
      List<Thing> ingredients,
      Bill bill)
    {
      Need_Energy need = pawn.needs.TryGetNeed<Need_Energy>();
      EnergyTrackerComp comp = pawn.TryGetComp<EnergyTrackerComp>();
      if (need != null)
        need.CurLevelPercentage = 0.0f;
      if (comp != null)
        comp.energy = 0.0f;
      ButcherUtility.SpawnDrops(pawn, pawn.Position, pawn.Map);
      pawn.Kill(null);
    }
  }
}
