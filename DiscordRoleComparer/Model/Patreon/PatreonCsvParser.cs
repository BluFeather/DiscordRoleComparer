using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordRoleComparer.Model.Patreon
{
    public static class PatreonCsvParser
    {
        public static PatreonCsvData ParsePatreonCsvFile(FileInfo csvFile)
        {
            List<PatronInfo> result = new List<PatronInfo>();
            Dictionary<string, PatronInfo> subscriberDictionary = new Dictionary<string, PatronInfo>();

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
                    EPatronStatus? patronStatus = columns[4].ParseAsPatronStatus();
                    double.TryParse(columns[6], out double lifetimeAmount);
                    string tier = columns[9];
                    DateTime.TryParse(columns[18], out DateTime lastChargeDate);
                    if (string.IsNullOrWhiteSpace(discord)) continue;

                    PatronInfo patreonSubscriber = new PatronInfo(discord, patronStatus, lifetimeAmount, tier, lastChargeDate);
                    if (subscriberDictionary.TryGetValue(patreonSubscriber.Discord, out PatronInfo existingSubscriber))
                    {
                        patreonSubscriber.TryCombinePatronInfo(existingSubscriber);
                        result.Remove(existingSubscriber);
                    }
                    subscriberDictionary.TryAdd(patreonSubscriber.Discord, patreonSubscriber);
                    result.Add(patreonSubscriber);
                }
            }

            return new PatreonCsvData(result);
        }
    }
}
