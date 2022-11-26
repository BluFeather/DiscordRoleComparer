using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace DiscordRoleComparer
{
    public class RoleRuleElement : RoleRule, INotifyPropertyChanged
    {
        public RoleRuleElement(string roleName, ulong roleID) : base(roleName, roleID)
        {
            GenerateDisplayElement();
        }

        private ERoleRequirementTypes _roleRequirementType = ERoleRequirementTypes.Unmanaged;

        public ERoleRequirementTypes RoleRequirementType
        {
            get
            {
                return _roleRequirementType;
            }
            set
            {
                _roleRequirementType = value;
                OnPropertyChanged();
            }
        }

        public ListBoxItem DisplayElement { get; private set; }

        private void GenerateDisplayElement()
        {
            DisplayElement = new ListBoxItem();
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
