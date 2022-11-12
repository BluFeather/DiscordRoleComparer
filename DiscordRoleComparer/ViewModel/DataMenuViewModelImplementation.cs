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
            PatreonSubscribers = PatreonCsvParser.ParsePatreonCsvFile(csvFile);
        }

        public override void CreateDiscordRoleEdits()
        {
            List<DiscordMemberEdits> discordMemberEdits = CreateDiscordMemberEditsList();

            foreach (var item in discordMemberEdits)
            {
                Debug.WriteLine($"{item.discordMember.SummarizeAsString()} | {item.PatreonSubscriber.SummarizeAsString()}");
            }
        }
#endregion

        protected List<DiscordMemberEdits> CreateDiscordMemberEditsList()
        {
            GuildData selectedGuildData = GuildDatas[DataMenuView.GuildsDropdown.SelectedIndex];

            Dictionary<string, PatreonSubscriber> patreonSubscribersList = new Dictionary<string, PatreonSubscriber>();
            foreach (PatreonSubscriber patreonSubscriber in PatreonSubscribers)
            {
                patreonSubscribersList.Add(patreonSubscriber.Discord, patreonSubscriber);
            }

            var discordMemberEdits = new List<DiscordMemberEdits>();
            foreach (DiscordMember discordMember in selectedGuildData.Members)
            {
                patreonSubscribersList.TryGetValue(discordMember.Username, out PatreonSubscriber patreonSubscriber);
                discordMemberEdits.Add(new DiscordMemberEdits(discordMember, patreonSubscriber));
            }
            return discordMemberEdits;
        }

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
    }
}
