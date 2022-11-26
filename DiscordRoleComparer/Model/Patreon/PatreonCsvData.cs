using System.Collections.Generic;

namespace DiscordRoleComparer.Model.Patreon
{
    public class PatreonCsvData
    {
        public PatreonCsvData(List<PatronInfo> connectedPatrons)
        {
            foreach (PatronInfo connectedPatron in connectedPatrons)
            {
                AddPatronInfo(connectedPatron);
            }
        }

        public void AddPatronInfo(PatronInfo patronInfo)
        {
            if (!ConnectedPatrons.TryAdd(patronInfo.Discord, patronInfo))
            {
                ConnectedPatrons[patronInfo.Discord].TryCombinePatronInfo(patronInfo);
            }
        }

        public Dictionary<string, PatronInfo> ConnectedPatrons { get; } = new Dictionary<string, PatronInfo>();
    }
}
