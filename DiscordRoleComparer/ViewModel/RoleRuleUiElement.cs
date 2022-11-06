using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace DiscordRoleComparer
{
    public class RoleRuleUiElement : StackPanel, INotifyPropertyChanged
    {
        public RoleRuleUiElement()
        {
            Orientation = Orientation.Horizontal;

            RoleRulesComboBox = new ComboBox() { ItemsSource = RoleRules, SelectedIndex = 0 };
            Children.Add(RoleRulesComboBox);

            RulesComboBox = new ComboBox() { ItemsSource = Enum.GetValues(typeof(Rules)), SelectedIndex = 0 };
            Children.Add(RulesComboBox);

            TiersComboBox = new ComboBox() { ItemsSource = Tiers, SelectedIndex = 0 };
            Children.Add(TiersComboBox);

            Children.Add(new Button() { Content = "Delete Rule" });
        }

        private RoleRule roleRule = new RoleRule();

        private static HashSet<string> roleRules = new HashSet<string>();
        public HashSet<string> RoleRules
        {
            get { return roleRules; }
            set
            {
                roleRules = value;
                OnPropertyChanged();
            }
        }

        private Rules? selectedRules;
        public Rules? SelectedRule
        {
            get { return selectedRules; }
            set
            {
                selectedRules = value;
                OnPropertyChanged();
            }
        }

        private static HashSet<string> tiers = new HashSet<string>();
        public HashSet<string> Tiers
        {
            get { return tiers; }
            set
            {
                tiers = value;
                OnPropertyChanged();
            }
        }

        #region UI Elements
        ComboBox RoleRulesComboBox;

        ComboBox RulesComboBox;

        ComboBox TiersComboBox;
        #endregion

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
