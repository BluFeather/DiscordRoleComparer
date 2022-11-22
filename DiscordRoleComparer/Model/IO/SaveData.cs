using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class SaveData
    {
        // Dictonary of encountered Usernames with Discriminators to Discord IDs.
        public Dictionary<string, ulong> DiscordMemberIDs { get; set; } = new Dictionary<string, ulong>();

        public override string ToString()
        {
            return $"Known Discord Users: {DiscordMemberIDs.Count}";
        }
    }
}
