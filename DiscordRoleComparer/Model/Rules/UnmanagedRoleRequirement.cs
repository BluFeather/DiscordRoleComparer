using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordRoleComparer
{
    public class UnmanagedRoleRequirement : RoleRequirement
    {
        public override bool RequirementMet(ChangeListItem changeListItem)
        {
            return true;
        }
    }
}
