using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordRoleComparer.ViewModel
{
    public class DataMenuViewModel
    {
        public DataMenuViewModel(DataMenu dataMenu)
        {
            View = dataMenu;
        }

        private DataMenu View { get; }
    }
}
