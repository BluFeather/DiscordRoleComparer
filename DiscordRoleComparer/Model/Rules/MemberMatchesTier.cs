using System;
using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class MemberMatchesTier : Rule
    {
        public override bool MemberMatchesRule(ChangeListItem changeListItem)
        {
            throw new NotImplementedException();
            //return discordMemberEdit.PatreonSubscriber.Tier == TierName;
        }

        public string TierName { get; set; } = "";
    }
}
