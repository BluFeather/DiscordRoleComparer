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

        public override async void PullDiscordGuilds()
        {
            DiscordFacade discordFacade = new DiscordFacade();
            GuildDatas = await discordFacade.AsyncPullGuildData(DataMenuView.TokenTextBox.Text);
        }

        public override void ParseCsvFile()
        {
            FileInfo csvFile = AskForCsvFile();
            if (csvFile == null) return;
            PatreonSubscribers = PatreonCsvParser.ParsePatreonCsvFile(csvFile);
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
    }
}
