using PassRecovery.BLL.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Formatters
{
    public class CsvLoginDataFormatter : ILoginDataFormatter
    {
        public string Format
        {
            get
            {
                return "csv";
            }
        }

        public void Print(IEnumerable<LoginData> logins, StreamWriter sw)
        {
            var template = new CsvTemplate();
            template.Logins = logins;
            sw.Write(template.TransformText());
        }
    }
}
