using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DiscordRoleComparer
{
    public abstract class DataMenuViewModel : INotifyPropertyChanged
    {
        public DataMenuViewModel() { }

        #region View Accessed Properties
        private List<PatreonSubscriber> _patreonSubscribers = new List<PatreonSubscriber>();

        public List<PatreonSubscriber> PatreonSubscribers
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

        private List<GuildData> _guildDatas = new List<GuildData>();

        public List<GuildData> GuildDatas
        {
            get 
            {
                return _guildDatas;
            }
            protected set
            {
                _guildDatas = value;
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
