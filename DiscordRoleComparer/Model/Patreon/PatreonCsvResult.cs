using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class PatreonCsvResult
    {
        public PatreonCsvResult(List<PatreonSubscriber> patrons)
        {
            Patrons = patrons;
            foreach (var patron in patrons)
            {
                TierNames.Add(patron.Tier);
            }
        }

        List<PatreonSubscriber> Patrons { get; }

        public int Count
        {
            get
            {
                return Patrons.Count;
            }
        }

        public bool TryFindPatronByUsername(string username, out PatreonSubscriber patreonSubscriber)
        {
            foreach (PatreonSubscriber patron in Patrons)
            {
                if (patron.Discord == username)
                {
                    patreonSubscriber = patron;
                    return true;
                }
            }
            patreonSubscriber = null;
            return false;
        }

        public static HashSet<string> TierNames { get; private set; } = new HashSet<string>();
    }
}
