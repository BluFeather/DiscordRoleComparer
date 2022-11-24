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

            foreach (var discordMember in SelectedGuild.Members)
            {
                ChangeListItem changeListItem = new ChangeListItem()
                {
                    DiscordID = discordMember.UserID,
                    DiscordUsername = discordMember.Username,
                    ExistingRoles = discordMember.RoleIDs
                };

                if (saveData.DiscordMemberAliases.TryGetValue(discordMember.UserID, out HashSet<string> knownUsernames))
                {
                    foreach (string username in knownUsernames)
                    {
                        if (PatreonSubscribers.TryFindPatronByUsername(username, out PatreonSubscriber patreonSubscriber))
                        {
                            changeListItem.PatreonSubscriberData = patreonSubscriber;
                            break;
                        }
                    }
                }

                ChangeListItems.Add(changeListItem);
            }

            // Hacky Comparison Logic Here
            foreach (var item in SelectedGuild.Roles)
            {
                Debug.WriteLine($"{item.Key} = {item.Value}\n");
            }

            foreach (ChangeListItem item in ChangeListItems)
            {
                if (!ExplicitRuleSet.MemberFoundInCsv(item))
                {
                    Debug.WriteLine($"{item.DiscordUsername} was not found in CSV. Skipping!");
                    continue;
                }

                if (ExplicitRuleSet.MemberRemainsUnedited(item))
                {
                    Debug.WriteLine($"{item.DiscordUsername} has spent $60 or more overall and will not be edited.");
                    continue;
                }

                if (!ExplicitRuleSet.MemberIsStillSubscribed(item))
                {
                    Debug.WriteLine($"{item.DiscordUsername} is no longer subscribed and should have their roles removed.");
                    continue;
                }

                if (ExplicitRuleSet.MemberNeedsEquusMinor(item))
                {
                    const ulong equusMinorRoleID = 559794815223726102;
                    if (item.PatreonSubscriberData.Tier == "Equus Minor (Early Access)")
                    {
                        if (item.ExistingRoles.Contains(equusMinorRoleID))
                        {
                            Debug.WriteLine($"{item.DiscordUsername} is already assigned Equus Minor.");
                        }
                        else
                        {
                            Debug.WriteLine($"{item.DiscordUsername} needs to be assigned Equus Minor!");
                        }
                    }
                }

                if (ExplicitRuleSet.MemberNeedsEquusMagnus(item))
                {
                    const ulong equusMagnusRoleID = 559795290748747806;
                    if (item.PatreonSubscriberData.Tier == "Equus Magnus")
                    {
                        if (item.ExistingRoles.Contains(equusMagnusRoleID))
                        {
                            Debug.WriteLine($"{item.DiscordUsername} is already assigned Equus Magnus.");
                        }
                        else
                        {
                            Debug.WriteLine($"{item.DiscordUsername} needs to be assigned Equus Magnus!");
                        }
                    }
                }

                if (ExplicitRuleSet.MemberNeedsEquusMinimi(item))
                {
                    const ulong equusMinimiRoleID = 559798726789562370;
                    if (item.PatreonSubscriberData.Tier == "Equus Minimi")
                    {
                        if (item.ExistingRoles.Contains(equusMinimiRoleID))
                        {
                            Debug.WriteLine($"{item.DiscordUsername} is already assigned Equus Minimi.");
                        }
                        else
                        {
                            Debug.WriteLine($"{item.DiscordUsername} needs to be assigned Equus Minimi!");
                        }
                    }
                }

                if (ExplicitRuleSet.MemberNeedsEquusMaximus(item))
                {
                    const ulong equusMaximusRoleID = 559795531195482154;
                    if (item.PatreonSubscriberData.Tier == "Equus Maximus")
                    {
                        if (item.ExistingRoles.Contains(equusMaximusRoleID))
                        {
                            Debug.WriteLine($"{item.DiscordUsername} is already assigned Equus Maximus.");
                        }
                        else
                        {
                            Debug.WriteLine($"{item.DiscordUsername} needs to be assigned Equus Maximus!");
                        }
                    }
                }
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
                saveData.AddDiscordAliasToSaveData(member.UserID, member.Username);
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

        #region Role Rules
        private List<RoleRule> RoleRules = new List<RoleRule>();

        private List<RoleRule> CreateRoleRules(GuildData guildData)
        {
            List<RoleRule> roleRules = new List<RoleRule>();
            foreach (KeyValuePair<ulong, string> role in guildData.Roles)
            {
                var roleRule = new RoleRule(role.Value, role.Key);
                roleRules.Add(roleRule);
            }
            return roleRules;
        }

        private HashSet<string> SetTierNamesForRoleRules(PatreonCsvResult patreonCsvResult)
        {
            return PatreonCsvResult.TierNames;
        }
        #endregion
    }
}
