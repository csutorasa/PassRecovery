using PassRecovery.BLL.Formatters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.UI.ExportPopup
{
    public class ExportPopupModel : ModelBase
    {
        private readonly ObservableCollection<ILoginDataFormatter> formatters = new ObservableCollection<ILoginDataFormatter>();
        public ObservableCollection<ILoginDataFormatter> Formatters { get { return formatters; } }

        private ILoginDataFormatter selectedFormatter;
        public ILoginDataFormatter SelectedFormatter
        {
            get
            {
                return selectedFormatter;
            }
            set
            {
                if (value != selectedFormatter)
                {
                    selectedFormatter = value;
                    OnPropertyChanged(nameof(SelectedFormatter));
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
        private bool openAfter = true;
        public bool OpenAfter
        {
            get
            {
                return openAfter;
            }
            set
            {
                if (value != openAfter)
                {
                    openAfter = value;
                    OnPropertyChanged(nameof(OpenAfter));
                }
            }
        }
    }
}
