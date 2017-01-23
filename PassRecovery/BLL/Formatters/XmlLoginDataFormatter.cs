using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PassRecovery.BLL.Formatters
{
    public class XmlLoginDataFormatter : ILoginDataFormatter
    {
        public string Format
        {
            get
            {
                return "xml";
            }
        }

        public void Print(IEnumerable<LoginData> logins, StreamWriter sw)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<LoginData>));
            serializer.Serialize(sw, logins);
        }
    }
}
