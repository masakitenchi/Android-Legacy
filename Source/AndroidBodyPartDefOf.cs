using RimWorld;
using Verse;

namespace Androids
{
    [DefOf]
    internal static class AndroidBodyPartDefOf
    {
        public static BodyPartDef AndroidThorax;
        public static BodyPartDef ArtificialAndroidBrain;

        static AndroidBodyPartDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
        }
    }
    
}
