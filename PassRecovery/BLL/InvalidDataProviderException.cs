using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL
{
    public class InvalidDataProviderException : Exception
    {
        public InvalidDataProviderException(string source) : base($"Cannot create {source} type data provider!") { }
    }
}
