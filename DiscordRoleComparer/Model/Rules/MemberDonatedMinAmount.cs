namespace DiscordRoleComparer
{
    public class MemberDonatedMinAmount : Rule
    {
        public override bool MemberMatchesRule(DiscordMemberEdits discordMemberEdit)
        {
            return discordMemberEdit.PatreonSubscriber.LifetimeAmount >= minimumDonationAmount;
        }

        double minimumDonationAmount { get; set; } = 0.0;
    }
}
