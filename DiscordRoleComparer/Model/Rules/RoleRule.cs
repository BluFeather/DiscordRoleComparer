using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class RoleRule
    {
        public RoleRule(string roleName, ulong roleID)
        {
            RoleName = roleName;
            RoleID = roleID;
        }

        public string RoleName { get; private set; } = "";

        public ulong RoleID { get; private set; } = 0;

        private RoleRequirement roleRequirement { get; set; } = null;

        public static HashSet<string> PatreonTierNames { get; set; } = new HashSet<string>();
    }
}
