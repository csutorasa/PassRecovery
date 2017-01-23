using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Providers
{
    public interface IDataProvider
    {
        /// <summary>
        /// Gets the source of the login data.
        /// </summary>
        string Source { get; }
        string GetProfilesDirectory();
        IEnumerable<Profile> GetProfiles();
        IEnumerable<LoginData> GetLogins();
        IEnumerable<LoginData> GetLogins(Profile profile);
    }
}
