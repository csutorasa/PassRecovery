using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL
{
    public class ProfileNotFoundException : Exception
    {
        public ProfileNotFoundException(Profile profile) : base($"Cannot find {profile.Source} type profile data in '{profile.Path}'!") { }
    }
}
