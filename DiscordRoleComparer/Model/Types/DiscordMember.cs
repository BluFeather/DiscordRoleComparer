using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class DiscordMember
    {
        public DiscordMember(ulong userID, string username, HashSet<ulong> roleIDs)
        {
            this.userID = userID;
            this.username = username;
            this.roleIDs = roleIDs;
        }

        public ulong userID;

        public string username;

        public HashSet<ulong> roleIDs;
    }
}
