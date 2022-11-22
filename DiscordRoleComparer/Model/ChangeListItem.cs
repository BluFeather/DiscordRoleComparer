using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class ChangeListItem
    {
        public string Username { get; set; } = "";

        public ulong DiscordID { get; set; } = 0;

        public HashSet<ulong> FinalRoles { get; set; } = new HashSet<ulong>();

        public HashSet<ulong> ExistingRoles { get; set; } = new HashSet<ulong>();

        public override string ToString()
        {
            return $"Username: {Username} | ID: {DiscordID}\nExisting Roles: {string.Join(", ", ExistingRoles)}\nFinal Roles: {string.Join(", ", FinalRoles)}";
        }
    }
}
