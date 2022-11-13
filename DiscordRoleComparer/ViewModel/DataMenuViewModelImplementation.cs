using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DiscordRoleComparer
{
    public class DataMenuViewModelImplementation : DataMenuViewModel
    {
        public DataMenuViewModelImplementation(DataMenu dataMenu)
        {
            DataMenuView = dataMenu;
            saveData = SaveDataHandler.LoadOrCreateSaveDataFromDisk();
        }

        DataMenu DataMenuView;

        SaveData saveData;

        #region View Input Events
        public override void ParseCsvFile()
        {
            FileInfo csvFile = AskForCsvFile();
            if (csvFile == null) return;
            try
            {
                PatreonSubscribers = PatreonCsvParser.ParsePatreonCsvFile(csvFile);
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.Message);
            }
        }

        public override async void PullDiscordGuilds()
        {
            GuildData guilds = LoadGuildDataFromDisk();
            GuildNames.Add(guilds.Name);
            AddGuildMembersToKnownUsersDatabase(guilds);
            SaveDataHandler.WriteSaveDataToDisk(saveData);
        }

        public override void CreateDiscordRoleEdits()
        {
            throw new NotImplementedException();
        }
        #endregion


        private void AddGuildMembersToKnownUsersDatabase(List<GuildData> guildData)
        {
            foreach (var guild in guildData)
            {
                AddGuildMembersToKnownUsersDatabase(guild);
            }
        }

        private void AddGuildMembersToKnownUsersDatabase(GuildData guildData)
        {
            foreach (var member in guildData.Members)
            {
                if (saveData.DiscordMemberAliases.ContainsKey(member.UserID))
                {
                    HashSet<string> aliases = saveData.DiscordMemberAliases[member.UserID];
                    aliases.Add(member.Username);
                    saveData.DiscordMemberAliases[member.UserID] = aliases;
                    continue;
                }
                saveData.DiscordMemberAliases.Add(member.UserID, new HashSet<string>() { member.Username });
            }
        }

        #region I/O
        private FileInfo AskForCsvFile()
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog() { Filter = "CSV | *.csv"};
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return new FileInfo(openFileDialog.FileName);
            }
            return null;
        }

        private FileInfo AskForJsonFile()
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog() { Filter = "json | *.json" };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return new FileInfo(openFileDialog.FileName);
            }
            return null;
        }

        private void GuildDataToJson(GuildData guildData)
        {
            string jsonString = JsonConvert.SerializeObject(guildData, Formatting.Indented);
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog() { Filter = "json | *.json" };
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, jsonString);
            }
        }

        private GuildData LoadGuildDataFromDisk()
        {
            FileInfo jsonFile = AskForJsonFile();
            if (jsonFile == null) return null;

            string jsonString = File.ReadAllText(jsonFile.FullName);
            return JsonConvert.DeserializeObject<GuildData>(jsonString);
        }
        #endregion
    }
}
