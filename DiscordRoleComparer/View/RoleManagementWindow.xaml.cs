using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DiscordRoleComparer
{
    /// <summary>
    /// Interaction logic for RoleManagementWindow.xaml
    /// </summary>
    public partial class RoleManagementWindow : Window
    {
        public RoleManagementWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = new RuleDefinitionsViewModel(this, OnWindowClosing);
            this.mainWindow = mainWindow;
            RulesList.Items.Add(new RoleRuleUiElement("Equus Minor", Rules.Patreon_Tier_Is, "Equus Minor (Early Access)").UiElement);
        }

        private MainWindow mainWindow;

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            mainWindow.IsEnabled = true;
        }

        protected IRuleDefinitionsViewModelInterface ViewModel { get { return DataContext as IRuleDefinitionsViewModelInterface;} }
    }
}
