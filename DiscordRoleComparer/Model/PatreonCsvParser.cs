using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;

namespace DiscordRoleComparer
{
    public static class PatreonCsvParser
    {
        public static Dictionary<string, SubscriberRole> ParsePatreonCsvFile(FileInfo csvFile)
        {
            Dictionary<string, SubscriberRole> userPatreonRoles = new Dictionary<string, SubscriberRole>();

            using (TextFieldParser parser = new TextFieldParser(csvFile.FullName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] columns = parser.ReadFields();
                    string username = columns[3];
                    SubscriberRole? subscriberRole = SubscriberRoleHelperFunctions.ParseSubscriberRole(columns[9]);
                    if (!string.IsNullOrEmpty(username) && subscriberRole != null)
                    {
                        userPatreonRoles.Add(username, subscriberRole.GetValueOrDefault());
                    }
                }
            }

            return userPatreonRoles;
        }
    }
}
