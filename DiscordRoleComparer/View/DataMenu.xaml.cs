using DiscordRoleComparer.ViewModel;
using System.Windows;

namespace DiscordRoleComparer
{
    public partial class DataMenu : Window
    {
        public DataMenu()
        {
            InitializeComponent();
            DataContext = new DataMenuViewModel(this);
        }

        private DataMenuViewModel DataMenuViewModel { get { return DataContext as DataMenuViewModel; } }
    }
}
