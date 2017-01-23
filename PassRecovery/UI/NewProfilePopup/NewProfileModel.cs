using PassRecovery.BLL.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.UI.NewProfilePopup
{
    public class NewProfileModel : ModelBase
    {
        private readonly ObservableCollection<IDataProvider> providers = new ObservableCollection<IDataProvider>();
        public ObservableCollection<IDataProvider> Providers { get { return providers; } }

        private IDataProvider selectedProvider;
        public IDataProvider SelectedProvider
        {
            get
            {
                return selectedProvider;
            }
            set
            {
                if (value != selectedProvider)
                {
                    selectedProvider = value;
                    OnPropertyChanged(nameof(SelectedProvider));
                    SelectedPath = null;
                }
            }
        }

        private string selectedPath;
        public string SelectedPath
        {
            get
            {
                return selectedPath;
            }
            set
            {
                if (value != selectedPath)
                {
                    selectedPath = value;
                    OnPropertyChanged(nameof(SelectedPath));
                }
            }
        }
    }
}
