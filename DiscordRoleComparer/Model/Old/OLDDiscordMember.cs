using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class OLDDiscordMember
    {
        public OLDDiscordMember(string handle, List<string> roles)
        {
            Handle = handle;
            Roles = roles;
        }

        public string Handle = null;

        public List<string> Roles = null;

        public static HashSet<string> UniqueRoles { get; set; } = new HashSet<string>();
    }
}
