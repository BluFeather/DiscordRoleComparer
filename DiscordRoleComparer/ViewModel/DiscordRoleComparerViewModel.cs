using Discord;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        List<DiscordMember> DiscordUserRoles = new List<DiscordMember>();
        #endregion

        #region Manage Roles
        public void ManageRoles_Clicked()
        {
            RoleManagementWindow roleManagementWindow = new RoleManagementWindow();
            roleManagementWindow.Show();
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
            discordRoleRepository.PullDiscordPatreonRoles(token, OnDiscordRolesPulled);
        }

        private void OnDiscordRolesPulled(object sender, List<DiscordMember> discordUserRoles)
        {
            DisableDiscordBotLog();
            DiscordUserRoles = discordUserRoles;
            LogMessage($"Discord User Roles Retrieved.");
            LogMessage("");

            foreach (var discordUser in discordUserRoles)
            {
                LogMessage($"{discordUser.Handle} is {string.Join(", ", discordUser.Roles)}");
            }
            PullDiscordRolesButtonEnabled = true;

            LogMessage("");
            LogMessage($"{DiscordMember.UniqueRoles.Count} Unique Roles Found.");
            foreach (var role in DiscordMember.UniqueRoles)
            {
                LogMessage($"Role: {role}");
            }
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
                LogMessage($"\"{csvFile.Name}\" could not be parsed!");
                LogMessage($"Is {csvFile.Name} the correct file?");
                LogMessage("");
                LogMessage($"Exception Message: {exception.Message}");
                return;
            }

            LogMessage($"{csvFile.Name} Opened.");
            LogMessage("");
            foreach (var subscriber in PatreonSubscriberRoles)
            {
                LogMessage($"{subscriber.DiscordHandle}  |  {subscriber.ActivePatron}  |  {subscriber.Tier}  |  {subscriber.LifetimeAmount}");
            }
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
            var comparer = new RoleComparer(PatreonSubscriberRoles, DiscordUserRoles);
            /*
            try
            {
                ClearLogMessages();
                LogMessage($"Number of Patreon Subscribers: {PatreonSubscriberRoles.Count}");
                LogMessage($"Number of Discord Roles Assigned: {DiscordSubscriberRoles.Count}");
                LogMessage("");

                var unsubscribedUsers = FindDiscordUsersWithRoleButNotSubscribed();
                foreach (var user in unsubscribedUsers)
                {
                    LogMessage($"{user.Key} is not subscribed! (Expected: NONE  |  Actual: {string.Join(", ", user.Value)})");
                    DiscordSubscriberRoles.Remove(user.Key);
                }

                var multipleSubscriberRoles = FindDiscordUsersWithMultipleSubscriberRoles();
                foreach (var subscriber in multipleSubscriberRoles)
                {
                    LogMessage($"{subscriber.Key} has more than one Subscriber Role! (Expected: {PatreonSubscriberRoles[subscriber.Key]}  |  Actual: {string.Join(", ", subscriber.Value)})");
                }

                var incorrectSubscriberRole = FindDiscordUsersWithIncorrectSubscriberRole();
                foreach (var subscriber in incorrectSubscriberRole)
                {
                    LogMessage($"{subscriber.Key} has the incorrect Subscriber Role! (Expected: {PatreonSubscriberRoles[subscriber.Key]}  |  Actual: {string.Join(", ", subscriber.Value)})");
                }

                if (GetNumberOfOutputMessages() == 3)
                {
                    LogMessage("No Discord Role Issues Found.");
                }
            }
            catch (Exception exception)
            {
                LogMessage("An Exception was encountered within FindRoleMismatches_Clicked()!");
                LogMessage($"Exception Message: {exception.Message}");
            }
            */
        }
        /*
        private Dictionary<string, List<SubscriberRole>> FindDiscordUsersWithRoleButNotSubscribed()
        {
            var result = new Dictionary<string, List<SubscriberRole>>();
            foreach (var discordUser in DiscordSubscriberRoles)
            {
                try
                {
                    if (!PatreonSubscriberRoles.ContainsKey(discordUser.Key))
                    {
                        result.Add(discordUser.Key, discordUser.Value);
                    }
                }
                catch (Exception exception)
                {
                    LogMessage($"An exception was thrown when comparing {discordUser.Key}!");
                    LogMessage($"Exception Message: {exception.Message}");
                }
            }
            return result;
        }

        private Dictionary<string, List<SubscriberRole>> FindDiscordUsersWithMultipleSubscriberRoles()
        {
            var result = new Dictionary<string, List<SubscriberRole>>();
            foreach (var discordUser in DiscordSubscriberRoles)
            {
                try
                {
                    string discordUsername = discordUser.Key;
                    List<SubscriberRole> subscriberRoles = discordUser.Value;
                    if (subscriberRoles.Count > 1)
                    {
                        result.Add(discordUsername, subscriberRoles);
                    }
                }
                catch (Exception exception)
                {
                    LogMessage($"An exception was thrown when comparing {discordUser.Key}!");
                    LogMessage($"Exception Message: {exception.Message}");
                }
            }
            return result;
        }

        private Dictionary<string, List<SubscriberRole>> FindDiscordUsersWithIncorrectSubscriberRole()
        {
            var result = new Dictionary<string, List<SubscriberRole>>();
            foreach (var discordUser in DiscordSubscriberRoles)
            {
                try
                {
                    string discordUsername = discordUser.Key;
                    List<SubscriberRole> subscriberRoles = discordUser.Value;
                    if (subscriberRoles.Count == 1 && PatreonSubscriberRoles.ContainsKey(discordUsername))
                    {
                        SubscriberRole? expectedRole = PatreonSubscriberRoles[discordUsername];
                        if (subscriberRoles[0] != expectedRole.GetValueOrDefault())
                        {
                            result.Add(discordUsername, subscriberRoles);
                        }
                    }
                }
                catch (Exception exception)
                {
                    LogMessage($"An exception was thrown when comparing {discordUser.Key}!");
                    LogMessage($"Exception Message: {exception.Message}");
                }
            }
            return result;
        }
        */
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
            FindRoleMismatchesButtonEnabled = PatreonSubscriberRoles.Count > 0 && DiscordUserRoles.Count > 0;
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
