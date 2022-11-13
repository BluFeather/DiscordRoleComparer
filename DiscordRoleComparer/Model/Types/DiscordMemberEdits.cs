using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class DiscordMemberEdits
    {
        public DiscordMemberEdits(ulong discordID, PatreonSubscriber patreonSubscriber)
        {
            DiscordID = discordID;
            PatreonSubscriber = patreonSubscriber;
        }

        public ulong DiscordID { get; }

        public PatreonSubscriber PatreonSubscriber { get; }

        public HashSet<ulong> RoleIdsToApply { get; set; } = new HashSet<ulong>();
    }
}
