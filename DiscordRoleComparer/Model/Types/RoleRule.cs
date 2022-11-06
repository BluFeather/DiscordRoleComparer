namespace DiscordRoleComparer
{
    /*
     * Class that specifies a condition in which a Discord Role should be applied. Reads as below
     * Apply "Role" if "Selected Rule" for "Value" is true.
     * Example: Apply Donator if Patreon_Tier_Is Donor.
    */
    public class RoleRule
    {
        public string SelectedRole;

        public Rules? SelectedRule;

        public string SelectedTier;
    }
}
