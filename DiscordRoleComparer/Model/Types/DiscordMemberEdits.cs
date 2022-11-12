using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class DiscordMemberEdits
    {
        public DiscordMemberEdits(ulong serverID, DiscordMember discordMember, PatreonSubscriber patreonSubscriber)
        {
            ServerID = serverID;
            DiscordMember = discordMember;
            PatreonSubscriber = patreonSubscriber;
        }

        public ulong ServerID { get; }

        public DiscordMember DiscordMember { get; }

        public PatreonSubscriber PatreonSubscriber { get; }

        public HashSet<ulong> RoleIdsToApply { get; set; } = new HashSet<ulong>();
    }
}
