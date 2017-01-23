using PassRecovery.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.UI.MainWindow
{
    public class MainModel : ModelBase
    {
        private readonly ObservableCollection<Profile> profiles = new ObservableCollection<Profile>();
        private readonly ObservableCollection<LoginData> loginData = new ObservableCollection<LoginData>();

        public ObservableCollection<Profile> Profiles { get { return profiles; } }
        public ObservableCollection<LoginData> LoginData { get { return loginData; } }
    }
}
