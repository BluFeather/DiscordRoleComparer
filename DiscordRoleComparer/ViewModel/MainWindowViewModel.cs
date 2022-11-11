using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DiscordRoleComparer
{
    public class MainWindowViewModel : IDiscordRoleComparerViewModelInterface
    {
        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private MainWindow mainWindow;

        public void FindRoleMismatches_Clicked()
        {
            throw new NotImplementedException();
        }

        public void ManageRoles_Clicked()
        {
            throw new NotImplementedException();
        }

        public async void PullDiscordRoles_Clicked()
        {
            var facade = new DiscordFacade();
            List<GuildData> result = await facade.AsyncPullGuildData(mainWindow.TokenTextBox.Text);
        }

        public void SaveOutputLog_Clicked()
        {
            throw new NotImplementedException();
        }

        public void SetPatreonCSVFile_Clicked()
        {
            throw new NotImplementedException();
        }
    }
}
