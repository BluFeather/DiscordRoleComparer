using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public static class RoleManagement
    {
        private static HashSet<string> _discordRoles = new HashSet<string>();
        public static HashSet<string> DiscordRoles
        {
            get
            {
                return _discordRoles;
            }
            set
            {
                _discordRoles = value;
            }
        }

        private static HashSet<string> _patreonTiers = new HashSet<string>();
        public static HashSet<string> PatreonTiers
        {
            get
            {
                return _patreonTiers;
            }
            set
            {
                _patreonTiers = value;
            }
        }
    }
}
