using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class GuildData
    {
        public GuildData(string name, List<DiscordMember> discordMembers, Dictionary<ulong, string> roles)
        {
            Name = name;
            DiscordMembers = discordMembers;
            Roles = roles;
        }

        public string Name;

        public List<DiscordMember> DiscordMembers;

        public Dictionary<ulong, string> Roles;
    }
}
