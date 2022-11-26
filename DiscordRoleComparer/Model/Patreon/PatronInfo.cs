using System;

namespace DiscordRoleComparer.Model.Patreon
{
    public class PatronInfo
    {
        public PatronInfo(string discord, EPatronStatus? patronStatus, double lifetimeAmount, string tier, DateTime lastChargeDate)
        {
            Discord = discord;
            PatronStatus = patronStatus;
            LifetimeAmount = lifetimeAmount;
            Tier = tier;
            LastChargeDate = lastChargeDate;
        }

        // Discord Username at the time of the Patron joining.
        public string Discord { get; private set; }

        // Last known status of the Patron.
        public EPatronStatus? PatronStatus { get; private set; }

        // Amount of money the Patron has paid since their first join date.
        public double LifetimeAmount { get; private set; }

        // Last known Tier assigned to the Patron.

        public string Tier { get; private set; }

        // Last time Patreon attempted to charge this Patron.
        public DateTime LastChargeDate { get; private set; }

        // Combines Lifetime Amount of other Patron Info if the Discord Usernames are the same.
        public bool TryCombinePatronInfo(PatronInfo patronInfo)
        {
            if (Discord != patronInfo.Discord) return false;

            LifetimeAmount += patronInfo.LifetimeAmount;

            if (LastChargeDate < patronInfo.LastChargeDate)
            {
                PatronStatus = patronInfo.PatronStatus;
                Tier = patronInfo.Tier;
                LastChargeDate = patronInfo.LastChargeDate;
            }
            return true;
        }

        public override string ToString()
        {
            return $"Discord: {Discord} | Patron Status: {PatronStatus} | Lifetime Amount: {LifetimeAmount} | Tier: {Tier} | Last Charge Date {LastChargeDate}";
        }
    }
}
