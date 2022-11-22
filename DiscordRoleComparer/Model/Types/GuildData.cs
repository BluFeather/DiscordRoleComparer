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

        public bool TryFindMemberByID(ulong discordID, out DiscordMember discordMember)
        {
            foreach (DiscordMember member in Members)
            {
                if (member.UserID == discordID)
                {
                    discordMember = member;
                    return true;
                }
            }
            discordMember = null;
            return false;
        }

        public bool TryFindMemberByName(string discordUsername, out DiscordMember discordMember)
        {
            foreach (DiscordMember member in Members)
            {
                if (member.Username == discordUsername)
                {
                    discordMember = member;
                    return true;
                }
            }
            discordMember = null;
            return false;
        }

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
