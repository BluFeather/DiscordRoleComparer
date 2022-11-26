using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class SaveData
    {
        // Dictionary of discord IDs to known Usernames.
        public Dictionary<ulong, HashSet<string>> DiscordMemberAliases { get; set; } = new Dictionary<ulong, HashSet<string>>();

        public void AddDiscordAliasToSaveData(ulong discordID, string username)
        {
            if (DiscordMemberAliases.ContainsKey(discordID))
            {
                DiscordMemberAliases[discordID].Add(username);
                return;
            }
            
            DiscordMemberAliases.Add(discordID, new HashSet<string>() { username });
        }

        public override string ToString()
        {
            return $"Known Discord Users: {DiscordMemberAliases.Count}";
        }
    }
}
