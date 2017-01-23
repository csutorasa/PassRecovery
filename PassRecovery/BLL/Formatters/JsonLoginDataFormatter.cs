using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Formatters
{
    public class JsonLoginDataFormatter : ILoginDataFormatter
    {
        public string Format
        {
            get
            {
                return "json";
            }
        }

        public void Print(IEnumerable<LoginData> logins, StreamWriter sw)
        {
            var serializer = JsonSerializer.Create();
            serializer.Serialize(sw, logins);
        }
    }
}
