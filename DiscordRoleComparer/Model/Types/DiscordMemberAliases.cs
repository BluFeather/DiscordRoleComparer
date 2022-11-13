using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class DiscordMemberAliases
    {
        public DiscordMemberAliases(ulong memberID, HashSet<string> usernames)
        {
            MemberID = memberID;
            Usernames = usernames;
        }

        public ulong MemberID { get; set; } = 0;

        public HashSet<string> Usernames { get; set; } = new HashSet<string>();
    }
}
