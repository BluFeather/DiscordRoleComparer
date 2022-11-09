using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class DiscordMember
    {
        public DiscordMember(ulong userID, HashSet<ulong> roleIDs)
        {
            this.userID = userID;
            this.roleIDs = roleIDs;
        }

        public ulong userID;

        public HashSet<ulong> roleIDs;
    }
}
