namespace DiscordRoleComparer
{
    public class IsTierRoleRequirement : RoleRequirement
    {
        public IsTierRoleRequirement(string tierName)
        {
            TierName = tierName;
        }

        public string TierName = "";

        public override bool RequirementMet(ChangeListItem changeListItem)
        {
            return changeListItem.PatreonSubscriberData.Tier == TierName;
        }
    }
}
