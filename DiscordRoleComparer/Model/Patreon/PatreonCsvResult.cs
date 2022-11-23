using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordRoleComparer
{
    public class PatreonCsvResult
    {
        public PatreonCsvResult(List<PatreonSubscriber> patrons)
        {
            Patrons = patrons;
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
    }
}
