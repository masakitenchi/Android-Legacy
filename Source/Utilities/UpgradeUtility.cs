using Verse;


namespace Androids
{
    public static class UpgradeUtility
    {
        public static bool Conflicts(this AndroidUpgradeDef upgrade, AndroidUpgradeDef otherUpgrade)
        {
            if (upgrade == otherUpgrade) return true;
            if (!upgrade.exclusivityGroups.NullOrEmpty() && !otherUpgrade.exclusivityGroups.NullOrEmpty())
            {
                for (int i = 0; i < upgrade.exclusivityGroups.Count; i++)
                {
                    if (otherUpgrade.exclusivityGroups.Contains(upgrade.exclusivityGroups[i]))
                        return true;
                }
            }
            return false;
        }
    }
}
