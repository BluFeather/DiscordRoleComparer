using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DiscordRoleComparer
{
    public abstract class DataMenuViewModel : INotifyPropertyChanged
    {
        public DataMenuViewModel() { }

        #region View Accessed Properties
        private PatreonCsvResult _patreonSubscribers = null;

        public PatreonCsvResult PatreonSubscribers
        {
            get
            {
                return _patreonSubscribers;
            }
            protected set
            {
                _patreonSubscribers = value;
                OnPropertyChanged();
            }
        }

        private HashSet<string> _guildNames = new HashSet<string>();

        public HashSet<string> GuildNames
        {
            get 
            {
                return _guildNames;
            }
            protected set
            {
                _guildNames = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region View Input Events
        public abstract void ParseCsvFile();

        public abstract void PullDiscordGuilds();

        public abstract void CreateDiscordRoleEdits();
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
