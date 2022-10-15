using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordRoleComparer
{
    public static class SubscriberRoleHelperFunctions
    {
        public static SubscriberRole? ParseSubscriberRole(string roleString)
        {
            switch (roleString)
            {
                case "Equus Maximus":
                    { return SubscriberRole.Equus_Maximus; }
                case "Equus Magnus":
                    { return SubscriberRole.Equus_Magnus; }
                case "Equus Minor":
                    { return SubscriberRole.Equus_Minor; }
                case "Equus Minor (Early Access)":
                    { return SubscriberRole.Equus_Minor; }
                case "Equus Minimi":
                    { return SubscriberRole.Equus_Minimi; }
                default: return null;
            }
        }

        public static List<SubscriberRole> ParseSubscriberRoles(List<string> discordRolesList)
        {
            List<SubscriberRole> subscriberRoles = new List<SubscriberRole>();
            foreach (var role in discordRolesList)
            {
                SubscriberRole? subscriberRole = ParseSubscriberRole(role);
                if (subscriberRole != null)
                {
                    subscriberRoles.Add(subscriberRole.GetValueOrDefault());
                }
            }
            return subscriberRoles;
        }
    }
}
