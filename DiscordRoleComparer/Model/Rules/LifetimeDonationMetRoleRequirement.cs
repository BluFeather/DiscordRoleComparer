namespace DiscordRoleComparer
{
    public class LifetimeDonationMetRoleRequirement : RoleRequirement
    {
        public LifetimeDonationMetRoleRequirement(double minDonatedAmount)
        {
            MinDonatedAmount = minDonatedAmount;
        }

        public double MinDonatedAmount = 0;

        public override bool RequirementMet(ChangeListItem changeListItem)
        {
            return changeListItem.PatreonSubscriberData.LifetimeAmount >= MinDonatedAmount;
        }
    }
}
