using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class SaveData
    {
        // Dictionary of discord IDs to known Usernames.
        public Dictionary<ulong, HashSet<string>> DiscordMemberAliases { get; set; } = new Dictionary<ulong, HashSet<string>>();

        public override string ToString()
        {
            return $"Known Discord Users: {DiscordMemberAliases.Count}";
        }
    }
}
