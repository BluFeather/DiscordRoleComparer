using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;

namespace DiscordRoleComparer
{
    public static class PatreonCsvParser
    {
        public static List<PatreonSubscriber> ParsePatreonCsvFile(FileInfo csvFile)
        {
            List<PatreonSubscriber> result = new List<PatreonSubscriber>();
            
            using (TextFieldParser parser = new TextFieldParser(csvFile.FullName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] columns = parser.ReadFields();

                    string discordHandle = columns[3];
                    bool activePatron = columns[4] == "Active patron";
                    SubscriberRole? subscriberRole = SubscriberRoleHelperFunctions.ParseSubscriberRole(columns[9]);
                    double.TryParse(columns[6], out double lifetimeAmount);
                    
                    if (string.IsNullOrWhiteSpace(discordHandle) == false)
                    {
                        if (discordHandle == "Discord") continue;
                        result.Add(new PatreonSubscriber(discordHandle, activePatron, subscriberRole, lifetimeAmount));
                    }
                }
            }

            return result;
        }
    }
}
