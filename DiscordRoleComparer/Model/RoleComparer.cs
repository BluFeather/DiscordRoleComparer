using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class RoleComparer
    {
        private readonly List<PatreonSubscriber> PatreonSubscriberRoles;

        private readonly List<DiscordMember> DiscordSubscriberRoles;

        public RoleComparer(List<PatreonSubscriber> patreonSubscriberRoles, List<DiscordMember> discordSubscriberRoles)
        {
            PatreonSubscriberRoles = patreonSubscriberRoles;
            DiscordSubscriberRoles = discordSubscriberRoles;
        }
    }
}
