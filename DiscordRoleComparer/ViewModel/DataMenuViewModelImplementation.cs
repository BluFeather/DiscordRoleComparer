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
        }

        DataMenu DataMenuView;

        #region View Input Events
        public override async void PullDiscordGuilds()
        {
#if DEBUG
            FileInfo jsonFile = AskForJsonFile();
            if (jsonFile == null) return;

            string jsonString = File.ReadAllText(jsonFile.FullName);
            GuildData result = JsonConvert.DeserializeObject<GuildData>(jsonString);
            GuildDatas = new List<GuildData>() { result };
#else
            DiscordFacade discordFacade = new DiscordFacade();
            GuildDatas = await discordFacade.AsyncPullGuildData(DataMenuView.TokenTextBox.Text);
#endif
        }

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

        public override void CreateDiscordRoleEdits()
        {
            List<DiscordMemberEdits> discordMemberEdits = CreateDiscordMemberEditsList();
            foreach (var discordMemberEdit in discordMemberEdits)
            {
                Debug.WriteLine($"{discordMemberEdit.discordMember.SummarizeAsString()} && {discordMemberEdit.PatreonSubscriber.SummarizeAsString()}");
            }
        }
#endregion

        protected List<DiscordMemberEdits> CreateDiscordMemberEditsList()
        {
            GuildData selectedGuildData = GuildDatas[DataMenuView.GuildsDropdown.SelectedIndex];

            Dictionary<string, PatreonSubscriber> patreonSubscribersList = new Dictionary<string, PatreonSubscriber>();
            foreach (PatreonSubscriber patreonSubscriber in PatreonSubscribers)
            {
                if (patreonSubscribersList.ContainsKey(patreonSubscriber.Discord))
                {
                    patreonSubscribersList[patreonSubscriber.Discord].CombineIfDiscordsMatch(patreonSubscriber);
                    continue;
                }
                patreonSubscribersList.Add(patreonSubscriber.Discord, patreonSubscriber);
            }

            var discordMemberEdits = new List<DiscordMemberEdits>();
            foreach (DiscordMember discordMember in selectedGuildData.Members)
            {
                patreonSubscribersList.TryGetValue(discordMember.Username, out PatreonSubscriber patreonSubscriber);
                if (patreonSubscriber == null)
                {
                    Debug.WriteLine($"{discordMember.Username} was not found in the Patreon Subscribers CSV!");
                    continue;
                }
                discordMemberEdits.Add(new DiscordMemberEdits(discordMember, patreonSubscriber));
            }
            return discordMemberEdits;
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
        #endregion
    }
}
