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

        public override string ToString()
        {
            return Name;
        }

        public string SummarizeAsString()
        {
            return $"Guild Name: {Name} | Member Count: {Members?.Count} | Roles: {string.Join(", ", Roles)}";
        }
    }
}
