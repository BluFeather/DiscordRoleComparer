using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class RoleComparer
    {
        private readonly List<PatreonSubscriber> PatreonSubscriberRoles;

        private readonly Dictionary<string, List<string>> DiscordSubscriberRoles;

        public RoleComparer(List<PatreonSubscriber> patreonSubscriberRoles, Dictionary<string, List<string>> discordSubscriberRoles)
        {
            PatreonSubscriberRoles = patreonSubscriberRoles;
            DiscordSubscriberRoles = discordSubscriberRoles;
        }
    }
}
