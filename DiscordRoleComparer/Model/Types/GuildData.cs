using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class GuildData
    {
        public GuildData(string name, List<DiscordMember> discordMembers, Dictionary<ulong, string> roles)
        {
            Name = name;
            Members = discordMembers;
            Roles = roles;
        }

        public string Name;

        public List<DiscordMember> Members;

        public Dictionary<ulong, string> Roles;
    }
}
