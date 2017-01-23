using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Providers
{
    public class OperaDataProvider : ChromeOperaAbstractProvider
    {
        public override string Source
        {
            get
            {
                return "Opera";
            }
        }

        public override string GetProfilesDirectory()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Opera Software", "Opera Stable");
        }

        public override IEnumerable<Profile> GetProfiles()
        {
            Profile profile = new Profile
            {
                DisplayName = "Opera",
                Path = GetProfilesDirectory(),
                Source = Source
            };
            if (GetLoginDB(profile).Exists)
            {
                return new Profile[] { profile };
            }
            return new Profile[0];
        }
    }
}
