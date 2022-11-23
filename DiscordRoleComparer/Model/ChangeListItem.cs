using System.Collections.Generic;

namespace DiscordRoleComparer
{
    public class ChangeListItem
    {
        public string DiscordUsername { get; set; } = "";

        public ulong DiscordID { get; set; } = 0;

        public PatreonSubscriber PatreonSubscriberData { get; set; } = null;

        public bool FoundInPatreonCSV { get { return PatreonSubscriberData != null; } }

        public HashSet<ulong> FinalRoles { get; set; } = new HashSet<ulong>();

        public HashSet<ulong> ExistingRoles { get; set; } = new HashSet<ulong>();

        public override string ToString()
        {
            if (FoundInPatreonCSV)
            {
                return $"Username: {DiscordUsername} | ID: {DiscordID}\nExisting Roles: {string.Join(", ", ExistingRoles)}\nPatreon Subscriber Data: {PatreonSubscriberData?.SummarizeAsString()}\nFinal Roles: {string.Join(", ", FinalRoles)}";
            }
            return $"Username: {DiscordUsername} was not found in the CSV file! | ID: {DiscordID}\nExisting Roles: {string.Join(", ", ExistingRoles)}\nFinal Roles: {string.Join(", ", FinalRoles)}";
        }
    }
}
