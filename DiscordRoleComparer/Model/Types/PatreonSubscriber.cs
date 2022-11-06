using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class PatreonSubscriber
    {
        public PatreonSubscriber(string discordHandle, bool activePatron, string tier, double lifetimeAmount)
        {
            DiscordHandle = discordHandle;
            ActivePatron = activePatron;
            Tier = tier;
            LifetimeAmount = lifetimeAmount;

            UniqueTiers.Add(tier);
        }

        public string DiscordHandle { get; set; } = "";

        public bool ActivePatron { get; set; } = false;

        public string Tier { get; set; } = null;

        public double LifetimeAmount { get; set; } = 0;

        public static HashSet<string> UniqueTiers { get; set; } = new HashSet<string>();
    }
}
