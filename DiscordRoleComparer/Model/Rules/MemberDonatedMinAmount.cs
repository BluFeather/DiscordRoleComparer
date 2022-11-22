using System;

namespace DiscordRoleComparer
{
    public class MemberDonatedMinAmount : Rule
    {
        public override bool MemberMatchesRule(ChangeListItem changeListItem)
        {
            throw new NotImplementedException();
            //return discordMemberEdit.PatreonSubscriber.LifetimeAmount >= minimumDonationAmount;
        }

        public double minimumDonationAmount { get; set; } = 0.0;
    }
}
