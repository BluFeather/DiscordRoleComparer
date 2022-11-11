using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class DiscordMemberEdits
    {
        public DiscordMemberEdits(DiscordMember discordMember, PatreonSubscriber patreonSubscriber)
        {
            this.discordMember = discordMember;
            PatreonSubscriber = patreonSubscriber;
        }

        public DiscordMember discordMember { get; }

        public PatreonSubscriber PatreonSubscriber { get; }

        public HashSet<ulong> roleIdsToApply { get; set; } = new HashSet<ulong>();
    }
}
