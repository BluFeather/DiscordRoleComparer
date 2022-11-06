using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace DiscordRoleComparer
{
    public class RoleRuleUiElement : RoleRule, INotifyPropertyChanged
    {
        public RoleRuleUiElement()
        {
            UiElement = new StackPanel() { Orientation = Orientation.Horizontal };

            RolesComboBox = new ComboBox() { ItemsSource = RolesDropdownOptions, SelectedIndex = 0 };
            RolesComboBox.SelectionChanged += RoleSelectionChanged;
            UiElement.Children.Add(RolesComboBox);

            RulesComboBox = new ComboBox() { ItemsSource = Enum.GetValues(typeof(Rules)), SelectedIndex = 0 };
            RulesComboBox.SelectionChanged += RuleSelectionChanged;
            UiElement.Children.Add(RulesComboBox);

            TiersComboBox = new ComboBox() { ItemsSource = TiersDropdownOptions, SelectedIndex = 0 };
            TiersComboBox.SelectionChanged += TiersSelectionChanged;
            UiElement.Children.Add(TiersComboBox);

            DeleteRuleButton = new Button() { Content = "Delete Rule" };
            DeleteRuleButton.Click += OnDeleteRuleButtonClicked;
            UiElement.Children.Add(DeleteRuleButton);
        }

        private void RoleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedRole = (sender as ComboBox)?.SelectedItem.ToString();
        }

        private void RuleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ruleSelection = (sender as ComboBox)?.SelectedItem.ToString();
            Enum.TryParse(ruleSelection, out Rules SelectedRule);
        }

        private void TiersSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedTier = (sender as ComboBox)?.SelectedItem.ToString();
        }

        #region Properties
        private HashSet<string> TiersDropdownOptions
        {
            get
            {
                
                if (PatreonSubscriber.UniqueTiers.Count > 0)
                {
                    var result = new HashSet<string>() { "(Select Patreon Tier)" };
                    result.UnionWith(PatreonSubscriber.UniqueTiers);
                    return result;
                }
                else
                {
                    return new HashSet<string>() { "(No Patreon Tiers Available)" };
                }
            }
        }

        private HashSet<string> RolesDropdownOptions
        {
            get
            {

                if (DiscordMember.UniqueRoles.Count > 0)
                {
                    var result = new HashSet<string>() { "(Select Discord Role)" };
                    result.UnionWith(DiscordMember.UniqueRoles);
                    return result;
                }
                else
                {
                    return new HashSet<string>() { "(No Discord Roles Available)" };
                }
            }
        }
        #endregion

        #region UI Elements
        public StackPanel UiElement { get; }

        ComboBox RolesComboBox;

        ComboBox RulesComboBox;

        ComboBox TiersComboBox;

        Button DeleteRuleButton;
        #endregion

        #region Events
        public EventHandler DeleteRuleClicked;

        private void OnDeleteRuleButtonClicked(object sender, RoutedEventArgs e)
        {
            DeleteRuleClicked?.Invoke(this, e);
        }
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
