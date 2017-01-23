using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.UI
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string nameofProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameofProperty));
        }
    }
}
