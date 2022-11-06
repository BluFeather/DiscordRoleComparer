using System;
using System.ComponentModel;
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

            RoleRulesComboBox = new ComboBox() { ItemsSource = DiscordMember.UniqueRoles, SelectedIndex = 0 };
            UiElement.Children.Add(RoleRulesComboBox);

            RulesComboBox = new ComboBox() { ItemsSource = Enum.GetValues(typeof(Rules)), SelectedIndex = 0 };
            UiElement.Children.Add(RulesComboBox);

            TiersComboBox = new ComboBox() { ItemsSource = PatreonSubscriber.UniqueTiers, SelectedIndex = 0 };
            UiElement.Children.Add(TiersComboBox);

            DeleteRuleButton = new Button() { Content = "Delete Rule" };
            DeleteRuleButton.Click += OnDeleteRuleButtonClicked;
            UiElement.Children.Add(DeleteRuleButton);
        }

        #region UI Elements
        public StackPanel UiElement { get; }

        ComboBox RoleRulesComboBox;

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
