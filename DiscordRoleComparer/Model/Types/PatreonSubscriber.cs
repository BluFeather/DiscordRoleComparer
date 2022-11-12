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

        public string Discord { get; } = "";

        public string Tier { get; } = null;

        public double LifetimeAmount { get; } = 0;

        public string SummarizeAsString()
        {
            return $"{Discord} has tier {Tier} and has donated {LifetimeAmount}.";
        }

        public static HashSet<string> UniqueTiers { get; private set; } = new HashSet<string>();
    }
}
