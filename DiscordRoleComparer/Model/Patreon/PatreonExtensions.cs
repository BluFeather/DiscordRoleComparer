namespace DiscordRoleComparer.Model.Patreon
{
    public static class PatreonExtensions
    {
        public static EPatronStatus? ParseAsPatronStatus(this string inString)
        {
            switch (inString)
            {
                case ("Active patron"):
                    return EPatronStatus.ActivePatron;
                case ("Former patron"):
                    return EPatronStatus.FormerPatron;
                case ("Declined patron"):
                    return EPatronStatus.DeclinedPatron;
                default: 
                    return null;
            }
        }
    }
}
