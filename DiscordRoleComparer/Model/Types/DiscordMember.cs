using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class DiscordMember
    {
        public DiscordMember(ulong userID, string username, HashSet<ulong> roleIDs)
        {
            this.UserID = userID;
            this.Username = username;
            this.RoleIDs = roleIDs;
        }

        public ulong UserID { get; set; }

        public string Username { get; set; }

        public HashSet<ulong> RoleIDs { get; set; }
    }
}
