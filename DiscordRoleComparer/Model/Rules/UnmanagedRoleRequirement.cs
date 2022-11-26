namespace DiscordRoleComparer
{
    public class UnmanagedRoleRequirement : RoleRequirement
    {
        public override bool RequirementMet(ChangeListItem changeListItem)
        {
            return true;
        }
    }
}
