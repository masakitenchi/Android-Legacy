// Decompiled with JetBrains decompiler
// Type: Androids.IPawnCrafter
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

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
