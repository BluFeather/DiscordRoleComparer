using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class PatreonSubscriber
    {
        public PatreonSubscriber(string discord, string tier, double lifetimeAmount)
        {
            Discord = discord;
            Tier = tier;
            LifetimeAmount = lifetimeAmount;

            UniqueTiers.Add(tier);
        }

        public string Discord { get; set; } = "";

        public string Tier { get; set; } = null;

        public double LifetimeAmount { get; set; } = 0;

        public static HashSet<string> UniqueTiers { get; set; } = new HashSet<string>();
    }
}
