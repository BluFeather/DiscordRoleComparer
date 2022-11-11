using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class RoleComparer
    {
        private readonly List<PatreonSubscriber> PatreonSubscriberRoles;

        private readonly List<OLDDiscordMember> DiscordSubscriberRoles;

        public RoleComparer(List<PatreonSubscriber> patreonSubscriberRoles, List<OLDDiscordMember> discordSubscriberRoles)
        {
            PatreonSubscriberRoles = patreonSubscriberRoles;
            DiscordSubscriberRoles = discordSubscriberRoles;
        }
    }
}
