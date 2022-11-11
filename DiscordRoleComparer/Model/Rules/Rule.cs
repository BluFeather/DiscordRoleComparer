namespace DiscordRoleComparer
{
    public abstract class Rule
    {
        public abstract bool MemberMatchesRule(DiscordMemberEdits discordMember);

        public ulong roleID = 0;
    }
}
