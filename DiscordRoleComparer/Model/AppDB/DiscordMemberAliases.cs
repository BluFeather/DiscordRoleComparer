using System.Collections.Generic;

namespace DiscordRoleComparer.Model.DB
{
    public class DiscordMemberAliases
    {
        // Dictionary of DiscordIDs and their associated Username/Discriminators.
        public Dictionary<ulong, HashSet<string>> MemberAliases { get; private set; } = new Dictionary<ulong, HashSet<string>>();

        // Tries to add specified username to discordID. Returns true if discordID or username are new entries.
        public bool TryAddAlias(ulong discordID, string username)
        {
            // if discordID exists, add username to UserAliases.
            if (MemberAliases.TryGetValue(discordID, out HashSet<string> knownAliases))
            {
                bool bIsNewAlias = !knownAliases.Contains(username);
                knownAliases.Add(username);
                return bIsNewAlias;
            }

            // if discordID does not exist, add discordID and username to UserAliases.
            if (!MemberAliases.ContainsKey(discordID))
            {
                MemberAliases.Add(discordID, new HashSet<string>() { username });
                return true;
            }

            return false;
        }

        // Returns number of known discord members as a string.
        public override string ToString()
        {
            return $"Number of known Discord Members: {MemberAliases.Keys.Count}";
        }
    }
}
