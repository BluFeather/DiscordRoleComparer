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

        List<GuildData> guilds = new List<GuildData>();

        List<ChangeListItem> ChangeListItems = new List<ChangeListItem>();

        private GuildData SelectedGuild
        {
            get
            {
                return guilds.Count > 0 ? guilds[0] : null;
            }
        }

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
            guilds.Add(LoadGuildDataFromDisk());
            GuildNames.Add(guilds[0]?.Name);
            AddGuildMembersToKnownUsersDatabase(guilds);
            SaveDataHandler.WriteSaveDataToDisk(saveData);
        }

        public override void CreateDiscordRoleEdits()
        {
            if (PatreonSubscribers.Count <= 0 || guilds.Count <= 0) return;
            ChangeListItems.Clear();

            foreach (PatreonSubscriber subscriber in PatreonSubscribers)
            {
                ChangeListItem changeListItem = new ChangeListItem()
                {
                    Username = subscriber.Discord
                };

                if (saveData.DiscordMemberIDs.TryGetValue(subscriber.Discord, out ulong discordID))
                {
                    var result = SelectedGuild.TryFindMemberByID(discordID, out DiscordMember discordMember);
                    changeListItem.DiscordID = discordID;
                    changeListItem.ExistingRoles = discordMember.RoleIDs;
                }

                ChangeListItems.Add(changeListItem);
            }
            foreach (ChangeListItem item in ChangeListItems)
            {
                Debug.WriteLine(item.ToString());
            }
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
                saveData.DiscordMemberIDs.TryAdd(member.Username, member.UserID);
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
