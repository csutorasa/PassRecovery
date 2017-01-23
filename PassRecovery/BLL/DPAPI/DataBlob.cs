using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.DPAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DataBlob
    {
        public int Length;
        public IntPtr Data;
        public DataBlob(byte[] data)
        {
            if (data == null)
                data = new byte[0];

            Data = Marshal.AllocHGlobal(data.Length);

            if (Data == IntPtr.Zero)
                throw new Exception("Unable to allocate data buffer for BLOB structure.");

            Length = data.Length;
            Marshal.Copy(data, 0, Data, data.Length);
        }
    }
}
