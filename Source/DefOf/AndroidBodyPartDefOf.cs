using RimWorld;
using Verse;

namespace Androids
{
    [DefOf]
    internal static class AndroidBodyPartDefOf
    {
        public static BodyPartDef AndroidThorax;
        public static BodyPartDef ArtificialAndroidBrain;
        public static BodyPartDef AndroidFinger;
        static AndroidBodyPartDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AndroidBodyPartDefOf));
        }
    }
    
}
