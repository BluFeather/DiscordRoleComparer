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

                    if (columns.Length != 27)
                    {
                        throw new FileFormatException($"\"{csvFile.Name}\" does not contain the expected number of columns and could not be parsed! \nPlease ensure the provided file is correct and is coming from Patreon.\nIf the file is correct, please let the developer know. Patreon may have changed the structure of their CSV files which means this program is out of date.");
                    }
                    string discordHandle = columns[3];
                    bool activePatron = columns[4] == "Active patron";
                    string subscriberRole = columns[9];
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
