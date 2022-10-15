namespace DiscordRoleComparer
{
    public class PatreonSubscriber
    {
        public PatreonSubscriber(string discordHandle, bool activePatron, SubscriberRole? tier, double lifetimeAmount)
        {
            DiscordHandle = discordHandle;
            ActivePatron = activePatron;
            Tier = tier;
            LifetimeAmount = lifetimeAmount;
        }

        public string DiscordHandle { get; set; } = "";

        public bool ActivePatron { get; set; } = false;

        public SubscriberRole? Tier { get; set; } = null;

        public double LifetimeAmount { get; set; } = 0;
    }
}
