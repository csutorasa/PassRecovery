using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PassRecovery.BLL.NSS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Providers
{
    public class FirefoxDataProvider : IDataProvider
    {
        public string Source
        {
            get
            {
                return "Firefox";
            }
        }

        private readonly NSSAPI nssapi;

        public FirefoxDataProvider()
        {
            try
            {
                nssapi = new NSSAPI(GetFirefoxDirectory());
            }
            catch
            {
                throw new InvalidDataProviderException(Source);
            }
        }

        public string GetProfilesDirectory()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Mozilla", "Firefox", "Profiles");
        }

        public IEnumerable<LoginData> GetLogins()
        {
            var loginData = new List<LoginData>();
            foreach (var profile in GetProfiles())
            {
                try
                {
                    loginData.AddRange(GetLogins(profile));
                }
                catch { }
            }
            return loginData;
        }

        public IEnumerable<LoginData> GetLogins(Profile profile)
        {
            var logins = new List<LoginData>();
            var profileDirectory = new DirectoryInfo(profile.Path);
            var loginsPath = new FileInfo(Path.Combine(profileDirectory.FullName, "logins.json"));
            if (loginsPath.Exists)
            {
                JObject loginsData = JObject.Parse(File.ReadAllText(loginsPath.FullName));
                try
                {
                    nssapi.LoadProfile(profileDirectory);
                }
                catch
                {
                    throw new ProfileNotFoundException(profile);
                }
                foreach (var x in loginsData["logins"])
                {
                    string url = x["hostname"].ToString();
                    string username = x["encryptedUsername"].ToString();
                    string password = x["encryptedPassword"].ToString();
                    logins.Add(createData(url, username, password));
                }
                return logins;
            }
            else
            {
                throw new ProfileNotFoundException(profile);
            }
        }

        public IEnumerable<Profile> GetProfiles()
        {
            var profiles = new List<Profile>();
            string firefoxDataDirectory = Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Mozilla", "Firefox");
            var profilesFile = new FileInfo(Path.Combine(firefoxDataDirectory, "profiles.ini"));
            if(profilesFile.Exists)
            {
                var lines = File.ReadAllLines(profilesFile.FullName);
                bool profileReadState = false;
                var data = new Dictionary<string, string>();
                foreach (var line in lines)
                {
                    if(!profileReadState && line.Length > 2 && line[0] == '[' && line[line.Length - 1] == ']' && line.Substring(1, line.Length - 2).StartsWith("Profile"))
                    {
                        profileReadState = true;
                    }
                    else if(profileReadState)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            string name;
                            string path;
                            string isRelative;
                            if (data.TryGetValue("Name", out name) && data.TryGetValue("Path", out path) && data.TryGetValue("IsRelative", out isRelative))
                            {
                                profiles.Add(new Profile
                                {
                                    DisplayName = name,
                                    Path = new DirectoryInfo(isRelative == "1" ? Path.Combine(firefoxDataDirectory, path) : path).FullName,
                                    Source = Source
                                });
                            }
                            profileReadState = false;
                        }
                        else
                        {
                            int index = line.IndexOf("=");
                            if (index >= 0)
                            {
                                data.Add(line.Substring(0, index), line.Substring(index + 1));
                            }
                        }
                    }
                }
            }
            return profiles;
        }

        private DirectoryInfo GetFirefoxDirectory()
        {
            return new DirectoryInfo(Path.Combine(Environment.GetEnvironmentVariable("programfiles"), "Mozilla Firefox"));
        }

        private IEnumerable<DirectoryInfo> GetProfilesPaths()
        {
            return Directory.GetDirectories(GetProfilesDirectory())
                .Select(profilePath => new DirectoryInfo(profilePath));
        }

        private FileInfo GetLoginsPath(DirectoryInfo profileDirectory)
        {
            return new FileInfo(Path.Combine(profileDirectory.FullName, "logins.json"));
        }

        private LoginData createData(string url, string username, string password)
        {
            return new LoginData
            {
                Url = url,
                Username = nssapi.Decrypt(username) ?? "",
                Password = nssapi.Decrypt(password) ?? "",
                Source = Source
            };
        }
    }
}
