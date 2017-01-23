using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Providers
{
    public abstract class ChromeOperaAbstractProvider : IDataProvider
    {
        public abstract string Source
        {
            get;
        }

        private readonly DPAPI.DPAPI dpapi = new DPAPI.DPAPI();

        public IEnumerable<LoginData> GetLogins()
        {
            var loginData = new List<LoginData>();
            foreach (var profile in GetProfiles())
            {
                loginData.AddRange(GetLogins(profile));
            }
            return loginData;
        }

        public IEnumerable<LoginData> GetLogins(Profile profile)
        {
            var dbPath = new FileInfo(Path.Combine(profile.Path, "Login Data"));
            if (dbPath.Exists)
            {
                FileInfo tempDbPath = BackupDatabase(dbPath);
                var logins = ReadLogins(tempDbPath);
                RemoveDatabase(tempDbPath);
                return logins;
            }
            else
            {
                throw new ProfileNotFoundException(profile);
            }
        }

        public abstract string GetProfilesDirectory();

        public abstract IEnumerable<Profile> GetProfiles();

        protected FileInfo GetLoginDB(Profile profile)
        {
            return new FileInfo(Path.Combine(profile.Path, "Login Data"));
        }

        private FileInfo BackupDatabase(FileInfo dbPath)
        {
            string newpath = Path.Combine(Environment.CurrentDirectory, "Login Data.tmp");
            dbPath.CopyTo(newpath, true);
            return new FileInfo(newpath);
        }

        private void RemoveDatabase(FileInfo dbPath)
        {
            if (dbPath.Exists)
            {
                dbPath.Delete();
            }
        }

        private IEnumerable<LoginData> ReadLogins(FileInfo dbPath)
        {
            List<LoginData> logins = new List<LoginData>();
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = dbPath.FullName,
                Version = 3,
                ReadOnly = true
            };
            using (var db = new SQLiteConnection(builder.ToString()))
            {
                db.Open();
                using (var command = db.CreateCommand())
                {
                    command.CommandText = "SELECT action_url, username_value, password_value FROM logins";
                    using (var reader = command.ExecuteReader())
                    {
                        byte[] passwordBytes = new byte[4096];
                        while (reader.Read())
                        {
                            string url = reader.GetString(0);
                            string username = reader.GetString(1);
                            long length = reader.GetBytes(2, 0, passwordBytes, 0, passwordBytes.Length);
                            byte[] password = new byte[length];
                            Array.Copy(passwordBytes, password, password.Length);
                            logins.Add(createData(url, username, password));
                        }
                    }
                }
            }
            return logins;
        }

        private LoginData createData(string url, string username, byte[] password)
        {
            return new LoginData
            {
                Url = url,
                Username = username,
                Password = Encoding.UTF8.GetString(dpapi.Decrypt(password, null)),
                Source = Source
            };
        }
    }
}
