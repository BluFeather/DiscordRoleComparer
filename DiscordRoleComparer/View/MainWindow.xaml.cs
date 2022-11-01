using System.Windows;

namespace DiscordRoleComparer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DiscordRoleComparerViewModel(this);
        }

        protected IDiscordRoleComparerViewModelInterface ViewModel { get { return DataContext as IDiscordRoleComparerViewModelInterface; } }

        private void SetPatreonCSVFile_Clicked(object sender, RoutedEventArgs e) => ViewModel?.SetPatreonCSVFile_Clicked();

        private void PullDiscordRoles_Clicked(object sender, RoutedEventArgs e) => ViewModel?.PullDiscordRoles_Clicked();

        private void SaveOutputLog_Clicked(object sender, RoutedEventArgs e) => ViewModel?.SaveOutputLog_Clicked();

        private void FindRoleMismatches_Clicked(object sender, RoutedEventArgs e) => ViewModel?.FindRoleMismatches_Clicked();

        private void ManageRoles_Clicked(object sender, RoutedEventArgs e) => ViewModel?.ManageRoles_Clicked();
    }
}
