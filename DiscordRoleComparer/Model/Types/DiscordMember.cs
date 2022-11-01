using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class DiscordMember
    {
        public DiscordMember(string handle, List<string> roles)
        {
            Handle = handle;
            Roles = roles;

            foreach (string role in roles)
            {
                _uniqueRoles.Add(role);
            }
        }

        public string Handle = null;

        public List<string> Roles = null;

        private static HashSet<string> _uniqueRoles = new HashSet<string>();

        public static HashSet<string> UniqueRoles { get { return _uniqueRoles; } }
    }
}
