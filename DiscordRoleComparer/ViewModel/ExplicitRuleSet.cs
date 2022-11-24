namespace DiscordRoleComparer
{
    public static class ExplicitRuleSet
    {
        public static bool MemberFoundInCsv(ChangeListItem changeListItem)
        {
            return changeListItem.FoundInPatreonCSV;
        }

        public static bool MemberRemainsUnedited(ChangeListItem changeListItem)
        {
            return changeListItem.PatreonSubscriberData.LifetimeAmount >= 60;
        }

        public static bool MemberIsStillSubscribed(ChangeListItem changeListItem)
        {
            return changeListItem.PatreonSubscriberData.PatronStatus == "Active patron";
        }

        public static bool MemberNeedsEquusMinor(ChangeListItem changeListItem)
        {
            return changeListItem.PatreonSubscriberData.Tier == "Equus Minor (Early Access)";
        }

        public static bool MemberNeedsEquusMagnus(ChangeListItem changeListItem)
        {
            return changeListItem.PatreonSubscriberData.Tier == "Equus Magnus";
        }

        public static bool MemberNeedsEquusMinimi(ChangeListItem changeListItem)
        {
            return changeListItem.PatreonSubscriberData.Tier == "Equus Minimi";
        }

        public static bool MemberNeedsEquusMaximus(ChangeListItem changeListItem)
        {
            return changeListItem.PatreonSubscriberData.Tier == "Equus Maximus";
        }
    }
}
