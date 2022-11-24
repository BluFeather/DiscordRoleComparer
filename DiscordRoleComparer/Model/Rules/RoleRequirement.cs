namespace DiscordRoleComparer
{
    public abstract class RoleRequirement
    {
        public string RoleName = "";

        public ulong RoleID = 0;

        public abstract bool RequirementMet(ChangeListItem changeListItem);
    }
}
