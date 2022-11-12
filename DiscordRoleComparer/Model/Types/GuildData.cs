using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class GuildData
    {
        public GuildData(string name, ulong serverID, List<DiscordMember> discordMembers, Dictionary<ulong, string> roles)
        {
            Name = name;
            Members = discordMembers;
            ServerID = serverID;
            Roles = roles;
        }

        public string Name;

        public ulong ServerID;

        public List<DiscordMember> Members;

        public Dictionary<ulong, string> Roles;

        public override string ToString()
        {
            return Name;
        }

        public string SummarizeAsString()
        {
            return $"Guild Name: {Name} | Server ID: {ServerID} | Member Count: {Members?.Count} | Roles: {string.Join(", ", Roles)}";
        }
    }
}
