// Decompiled with JetBrains decompiler
// Type: Androids.IPawnCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using Verse;

namespace Androids
{
  public interface IPawnCrafter
  {
    Pawn PawnBeingCrafted();

    CrafterStatus PawnCrafterStatus();

    void InitiatePawnCrafting();

    void StopPawnCrafting();
  }
}
