using RimWorld;
using Verse;

namespace Androids
{
    [DefOf]
    public static class AndroidBodyPartDefOf
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
