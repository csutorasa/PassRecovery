using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.NSS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SECItem
    {
        public int Type;
        public int Data;
        public int Len;
    }
}
