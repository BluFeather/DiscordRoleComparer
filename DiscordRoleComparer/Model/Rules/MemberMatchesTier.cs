namespace DiscordRoleComparer
{
    public class MemberMatchesTier : Rule
    {
        public override bool MemberMatchesRule(DiscordMemberEdits discordMemberEdit)
        {
            return discordMemberEdit.PatreonSubscriber.Tier == TierName;
        }

        public string TierName { get; set; } = "";
    }
}
