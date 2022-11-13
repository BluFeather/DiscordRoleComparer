using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class SaveData
    {
        public List<DiscordMemberAliases> DiscordMemberAliases { get; set; } = new List<DiscordMemberAliases>();

        public override string ToString()
        {
            return $"Known Discord Users: {DiscordMemberAliases.Count}";
        }
    }
}
