using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DiscordRoleComparer
{
    public class RuleDefinitionsViewModel : IRuleDefinitionsViewModelInterface, INotifyPropertyChanged
    {
        public RuleDefinitionsViewModel(RoleManagementWindow roleManagementWindow, CancelEventHandler windowClosingCallback)
        {
            this.roleManagementWindow = roleManagementWindow;
            roleManagementWindow.Closing += windowClosingCallback;
        }

        private RoleManagementWindow roleManagementWindow;

        public HashSet<string> DiscordRoles
        {
            get
            {
                HashSet<string> output = new HashSet<string>();
                output.Add(RoleManagement.DiscordRoles.Count <= 0 ? "(No Discord Roles)" : "(Select Discord Role)");
                output.UnionWith(RoleManagement.DiscordRoles);
                return output;
            }
            set
            {
                RoleManagement.DiscordRoles = value;
                OnPropertyChanged();
            }
        }

        public HashSet<string> PatreonTiers
        {
            get
            {
                HashSet<string> output = new HashSet<string>();
                output.Add(RoleManagement.PatreonTiers.Count <= 0 ? "(No Patreon Tiers)" : "(Select Patreon Tier)");
                output.UnionWith(RoleManagement.PatreonTiers);
                return output;
            }
            set
            {
                RoleManagement.PatreonTiers = value;
                OnPropertyChanged();
            }
        }

        private HashSet<string> _ruleTypes = new HashSet<string>()
        {
            "(Select Conditional)",
            "If Tier Is",
            "If Lifetime Donation Is Or Higher"
        };
        public HashSet<string> RuleTypes
        {
            get
            {
                return _ruleTypes;
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
