using System;
using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class PatreonSubscriber
    {
        public PatreonSubscriber(string discord, string patronStatus, double lifetimeAmount, string tier, DateTime lastChargeDate)
        {
            Discord = discord;
            PatronStatus = patronStatus;
            LifetimeAmount = lifetimeAmount;
            Tier = tier;
            LastChargeDate = lastChargeDate;

            UniqueTiers.Add(tier);
        }

        public string Discord { get; private set; } = "";

        public string PatronStatus { get; private set; } = "";

        public double LifetimeAmount { get; private set; } = 0;

        public string Tier { get; private set; } = null;

        public DateTime LastChargeDate = DateTime.MinValue;
        
        public string SummarizeAsString()
        {
            return $"Discord: {Discord} | Patron Status: {PatronStatus} | Lifetime Amount: {LifetimeAmount} | Tier: {Tier} | Last Charge Date: {LastChargeDate}";
        }

        public bool CombineIfDiscordsMatch(PatreonSubscriber patreonSubscriber)
        {
            if (Discord != patreonSubscriber.Discord) return false;

            LifetimeAmount += patreonSubscriber.LifetimeAmount;

            if (LastChargeDate < patreonSubscriber.LastChargeDate)
            {
                PatronStatus = patreonSubscriber.PatronStatus;
                Tier = patreonSubscriber.Tier;
                LastChargeDate = patreonSubscriber.LastChargeDate;
            }
            return true;
        }

        public static HashSet<string> UniqueTiers { get; private set; } = new HashSet<string>();
    }
}
