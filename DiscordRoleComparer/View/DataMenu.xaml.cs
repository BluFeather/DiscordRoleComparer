using System.Windows;

namespace DiscordRoleComparer
{
    public partial class DataMenu : Window
    {
        public DataMenu()
        {
            InitializeComponent();
            DataContext = new DataMenuViewModelImplementation(this);
        }

        protected DataMenuViewModel ViewModel { get { return DataContext as DataMenuViewModel; } }

        private void ParseCsvFile_Clicked(object sender, RoutedEventArgs e) => ViewModel?.ParseCsvFile();

        private void PullDiscordGuilds_Clicked(object sender, RoutedEventArgs e) => ViewModel?.PullDiscordGuilds();
    }
}
