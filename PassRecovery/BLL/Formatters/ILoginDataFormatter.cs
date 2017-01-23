using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Formatters
{
    public interface ILoginDataFormatter
    {
        /// <summary>
        /// Gets the name of the format
        /// </summary>
        string Format { get; }
        /// <summary>
        /// Writes formatted login data into the stream.
        /// </summary>
        /// <param name="logins">Input data</param>
        /// <param name="sw">Stream to write</param>
        void Print(IEnumerable<LoginData> logins, StreamWriter sw);
    }
}
