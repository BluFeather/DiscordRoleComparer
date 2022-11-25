using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiscordRoleComparer
{
    public static class PatreonCsvParser
    {
        public static PatreonCsvResult ParsePatreonCsvFile(FileInfo csvFile)
        {
            List<PatreonSubscriber> result = new List<PatreonSubscriber>();
            Dictionary<string, PatreonSubscriber> subscriberDictionary = new Dictionary<string, PatreonSubscriber>();

            using (TextFieldParser parser = new TextFieldParser(csvFile.FullName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                string[] firstRow = parser.ReadFields();
                if (firstRow.Length != 27 || firstRow[3] != "Discord" || firstRow[4] != "Patron Status" || firstRow[6] != "Lifetime Amount" || firstRow[9] != "Tier" || firstRow[18] != "Last Charge Date")
                {
                    throw new FileFormatException($"\"{csvFile.Name}\" does not contain the expected number of columns and could not be parsed! \nPlease ensure the provided file is correct and is coming from Patreon.\nIf the file is correct, please let the developer know. Patreon may have changed the structure of their CSV files which means this program is out of date.");
                }

                while (!parser.EndOfData)
                {
                    string[] columns = parser.ReadFields();

                    string discord = columns[3];
                    string patronStatus = columns[4];
                    double.TryParse(columns[6], out double lifetimeAmount);
                    string tier = columns[9];
                    DateTime.TryParse(columns[18], out DateTime lastChargeDate);
                    if (string.IsNullOrWhiteSpace(discord)) continue;

                    PatreonSubscriber patreonSubscriber = new PatreonSubscriber(discord, patronStatus, lifetimeAmount, tier, lastChargeDate);
                    if (subscriberDictionary.TryGetValue(patreonSubscriber.Discord, out PatreonSubscriber existingSubscriber))
                    {
                        patreonSubscriber.CombineIfDiscordsMatch(existingSubscriber);
                        result.Remove(existingSubscriber);
                    }
                    subscriberDictionary.TryAdd(patreonSubscriber.Discord, patreonSubscriber);
                    result.Add(patreonSubscriber);
                }
            }

            return new PatreonCsvResult(result);
        }
    }
}
