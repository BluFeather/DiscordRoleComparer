using Discord;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DiscordRoleComparer
{
    public class DiscordRoleComparerViewModel : IDiscordRoleComparerViewModelInterface, INotifyPropertyChanged
    {
        public DiscordRoleComparerViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private MainWindow mainWindow;

        #region Data Sets
        List<PatreonSubscriber> PatreonSubscriberRoles = new List<PatreonSubscriber>();

        List<OLDDiscordMember> DiscordMembers = new List<OLDDiscordMember>();
        #endregion

        #region Manage Roles
        public void ManageRoles_Clicked()
        {
            RoleManagementWindow roleManagementWindow = new RoleManagementWindow(mainWindow);
            roleManagementWindow.Show();
            mainWindow.IsEnabled = false;
        }
        #endregion

        #region Discord User Role Repository
        private DiscordRoleRepository discordRoleRepository;

        public void PullDiscordRoles_Clicked()
        {
            ClearLogMessages();
            discordRoleRepository = new DiscordRoleRepository();
            discordRoleRepository.Log += DiscordBotLog;
            PassthroughBotMessages = true;
            string token = mainWindow.TokenTextBox.Text;
            if (string.IsNullOrEmpty(token))
            {
                LogMessage("No token provided!");
                return;
            }
            PullDiscordRolesButtonEnabled = false;
            discordRoleRepository.PullDiscordPatreonRoles(token, OnDiscordMembersPulled, OnDiscordRolesPulled);
        }

        private void OnDiscordRolesPulled(object sender, HashSet<string> discordServerRoles)
        {
            OLDDiscordMember.UniqueRoles = discordServerRoles;
            LogMessage("");
            LogMessage($"{OLDDiscordMember.UniqueRoles.Count} Unique Roles Found.");
            foreach (var role in OLDDiscordMember.UniqueRoles)
            {
                LogMessage($"Role: {role}");
            }
            RoleManagement.DiscordRoles = OLDDiscordMember.UniqueRoles;
        }

        private void OnDiscordMembersPulled(object sender, List<OLDDiscordMember> discordMembersList)
        {
            DisableDiscordBotLog();
            DiscordMembers = discordMembersList;
            LogMessage($"Discord User Roles Retrieved.");
            LogMessage("");

            foreach (var discordMember in discordMembersList)
            {
                LogMessage($"{discordMember.Handle} is {string.Join(", ", discordMember.Roles)}");
            }
            PullDiscordRolesButtonEnabled = true;
            UpdateButtonEnabledStates();
        }

        bool PassthroughBotMessages = true;

        private Task DisableDiscordBotLog()
        {
            PassthroughBotMessages = false;
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                ClearLogMessages();
            }
            else
            {
                dispatcher.BeginInvoke(new Action(() => ClearLogMessages()));
            }
            return Task.CompletedTask;
        }

        private Task DiscordBotLog(LogMessage arg)
        {
            if (PassthroughBotMessages == false) return Task.CompletedTask;
            LogMessage(arg.Message);
            return Task.CompletedTask;
        }
        #endregion

        #region Patreon CSV Parser
        public void SetPatreonCSVFile_Clicked()
        {
            var csvFile = GetCsvFromUser();

            DisableDiscordBotLog();
            ClearLogMessages();
            if (csvFile == null)
            {
                LogMessage($"No CSV file was selected.");
                return;
            }

            try
            {
                PatreonSubscriberRoles = PatreonCsvParser.ParsePatreonCsvFile(csvFile);
                UpdateButtonEnabledStates();
            }
            catch (Exception exception)
            {
                LogMessage(exception.Message);
                return;
            }

            LogMessage($"{csvFile.Name} Opened.");
            LogMessage("");
            foreach (var subscriber in PatreonSubscriberRoles)
            {
                LogMessage($"{subscriber.Discord}  |  {subscriber.Tier}  |  {subscriber.LifetimeAmount}");
            }
            RoleManagement.PatreonTiers = PatreonSubscriber.UniqueTiers;
        }

        protected FileInfo GetCsvFromUser()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv) | *.csv";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return new FileInfo(dlg.FileName);
            }
            return null;
        }
        #endregion

        #region Find Role Mismatches
        public void FindRoleMismatches_Clicked()
        {
            var comparer = new RoleComparer(PatreonSubscriberRoles, DiscordMembers);
        }
        #endregion

        #region Output Logging
        public void LogMessage(string message)
        {
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                LogToOutput(message);
            }
            else
            {
                dispatcher.BeginInvoke(new Action(() => LogToOutput(message)));
            }
        }

        public void ClearLogMessages()
        {
            mainWindow.LoggingListBox.Items.Clear();
        }

        public void SaveOutputLog_Clicked()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt) | *.txt"
            };

            if (saveFileDialog.ShowDialog() != true) return;

            var writer = new StreamWriter(saveFileDialog.FileName);

            foreach (var item in mainWindow.LoggingListBox.Items)
            {
                if (item is ListBoxItem listBoxItem)
                {
                    writer.WriteLine(listBoxItem.Content);
                }
            }
            writer.Close();
        }

        public int GetNumberOfOutputMessages()
        {
            return mainWindow.LoggingListBox.Items.Count;
        }

        private void LogToOutput(string message)
        {
            ListBoxItem listBoxItem = new ListBoxItem() { Content = message };
            mainWindow.LoggingListBox.Items.Add(listBoxItem);
        }
        #endregion

        #region UI Element States
        private bool _findRoleMismatchesButtonEnabled = false;

        public bool FindRoleMismatchesButtonEnabled
        {
            get
            {
                return _findRoleMismatchesButtonEnabled;
            }
            set
            {
                _findRoleMismatchesButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _pullDiscordRolesButtonEnabled = true;

        public bool PullDiscordRolesButtonEnabled
        {
            get
            {
                return _pullDiscordRolesButtonEnabled;
            }
            set
            {
                _pullDiscordRolesButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private void UpdateButtonEnabledStates()
        {
            FindRoleMismatchesButtonEnabled = PatreonSubscriberRoles.Count > 0 && DiscordMembers.Count > 0;
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
