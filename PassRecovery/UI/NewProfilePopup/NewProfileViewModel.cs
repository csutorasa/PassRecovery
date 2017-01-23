using PassRecovery.BLL;
using PassRecovery.BLL.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.UI.NewProfilePopup
{
    public class NewProfileViewModel
    {
        private readonly NewProfileModel model = new NewProfileModel();
        private readonly ClassFinder classFinder = new ClassFinder();

        public NewProfileModel Model { get { return model; } }

        public NewProfileViewModel(IEnumerable<IDataProvider> providers)
        {
            foreach (var provider in providers)
            {
                Model.Providers.Add(provider);
            }
            Model.SelectedProvider = Model.Providers.First();
        }

        public Profile Add()
        {
            return new Profile
            {
                DisplayName = Model.SelectedPath.Split('\\').Last(),
                Path = Model.SelectedPath,
                Source = Model.SelectedProvider.Source
            };
        }

        public void OpenFileDialog()
        {
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowser.SelectedPath = Model.SelectedProvider.GetProfilesDirectory();
            var result = folderBrowser.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Model.SelectedPath = folderBrowser.SelectedPath;
            }
        }
    }
}
