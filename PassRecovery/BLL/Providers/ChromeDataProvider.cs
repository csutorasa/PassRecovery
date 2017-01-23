using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Providers
{
    public class ChromeDataProvider : ChromeOperaAbstractProvider
    {
        public override string Source
        {
            get
            {
                return "Chrome";
            }
        }

        public override string GetProfilesDirectory()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "Google", "Chrome", "User Data");
        }

        public override IEnumerable<Profile> GetProfiles()
        {
            List<Profile> profiles = new List<Profile>();
            DirectoryInfo profileDirectory;
            string profilesDirectory = GetProfilesDirectory();
            Profile profile;
            profileDirectory = new DirectoryInfo(Path.Combine(profilesDirectory, "Default"));
            if (profileDirectory.Exists)
            {
                profile = new Profile
                {
                    Path = profileDirectory.FullName,
                    DisplayName = "Default",
                    Source = Source
                };
                if (GetLoginDB(profile).Exists)
                {
                    profiles.Add(profile);
                }
            }
            for (int i = 1; i < 1000; i++)
            {
                profileDirectory = new DirectoryInfo(Path.Combine(profilesDirectory, "Profile " + i));
                if (profileDirectory.Exists)
                {
                    profile = new Profile
                    {
                        Path = profileDirectory.FullName,
                        DisplayName = "Profile " + i,
                        Source = Source
                    };
                    if (GetLoginDB(profile).Exists)
                    {
                        profiles.Add(profile);
                    }
                }
                else
                {
                    break;
                }
            }
            return profiles;
        }
    }
}
