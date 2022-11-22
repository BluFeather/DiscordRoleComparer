namespace DiscordRoleComparer
{
    public abstract class Rule
    {
        public abstract bool MemberMatchesRule(ChangeListItem changeListItem);

        public ulong roleID = 0;
    }
}
