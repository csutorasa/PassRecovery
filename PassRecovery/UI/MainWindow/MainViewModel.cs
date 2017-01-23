using PassRecovery.BLL;
using PassRecovery.BLL.Providers;
using PassRecovery.UI.ExportPopup;
using PassRecovery.UI.NewProfilePopup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PassRecovery.UI.MainWindow
{
    public class MainViewModel
    {
        private readonly MainModel model = new MainModel();
        private readonly ClassFinder classFinder = new ClassFinder();
        private readonly IEnumerable<IDataProvider> providers;

        public MainModel Model { get { return model; } }
        public MainViewModel()
        {
            providers = classFinder.GetSubClasses(typeof(IDataProvider))
                .Select(loginDataProviderType => (IDataProvider)classFinder.CreateInstance(loginDataProviderType));
            RefreshProfiles();
        }

        public void AddProfile()
        {
            var newProfilePopup = new NewProfilePopup.NewProfilePopup(new NewProfileViewModel(providers));
            newProfilePopup.ShowDialog();
            if(newProfilePopup.Profile != null)
            {
                Model.Profiles.Add(newProfilePopup.Profile);
            }
        }

        public void OpenExportPopup(bool onlySelected)
        {
            IEnumerable<LoginData> data;
            if(onlySelected)
            {
                data = Model.LoginData;
            }
            else
            {
                data = Model.Profiles.SelectMany(profile =>
                {
                    var provider = providers.First(p => p.Source == profile.Source);
                    return provider.GetLogins(profile);
                });
            }
            new ExportPopup.ExportPopup(new ExportPopupViewModel(data)).ShowDialog();
        }

        public void RefreshProfiles()
        {
            Model.Profiles.Clear();
            foreach (var provider in providers)
            {
                foreach (var profile in provider.GetProfiles())
                {
                    Model.Profiles.Add(profile);
                }
            }
        }

        public IEnumerable<ProfileNotFoundException> ItemsSelected(IEnumerable<Profile> profiles)
        {
            Model.LoginData.Clear();
            var errors = new List<ProfileNotFoundException>();
            foreach (var selectedProfile in profiles.ToArray())
            {
                if (selectedProfile != null)
                {
                    try
                    {
                        var provider = providers.First(x => x.Source == selectedProfile.Source);
                        foreach (var login in provider.GetLogins(selectedProfile))
                        {
                            Model.LoginData.Add(login);
                        }
                    }
                    catch (ProfileNotFoundException ex)
                    {
                        Model.Profiles.Remove(selectedProfile);
                        errors.Add(ex);
                    }
                }
            }
            return errors;
        }
    }
}
