using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.DPAPI
{
    public class DPAPI
    {
        public byte[] Decrypt(byte[] input, byte[] entropy)
        {
            string s;
            return Decrypt(input, entropy, out s);
        }

        public byte[] Decrypt(byte[] input, byte[] entropy, out string description)
        {
            DataBlob inputBlob = new DataBlob(input);
            description = string.Empty;
            DataBlob entropyBlob = new DataBlob(entropy);
            CryptProtectPrompt prompt = CryptProtectPrompt.Create();
            int flags = (int)CryptProptectInitalizationFlags.UiForbidden;
            DataBlob outputBlob = new DataBlob();

            try
            {
                bool success = CryptUnprotectData(ref inputBlob, ref description, ref entropyBlob, IntPtr.Zero, ref prompt, flags, ref outputBlob);
                if (!success)
                {
                    int errCode = Marshal.GetLastWin32Error();
                    throw new Exception("CryptUnprotectData() failed.", new Win32Exception(errCode));
                }
                byte[] outputBytes = new byte[outputBlob.Length];
                Marshal.Copy(outputBlob.Data, outputBytes, 0, outputBlob.Length);
                return outputBytes;
            }
            finally
            {
                if (outputBlob.Data != IntPtr.Zero)
                    Marshal.FreeHGlobal(outputBlob.Data);
                if (inputBlob.Data != IntPtr.Zero)
                    Marshal.FreeHGlobal(inputBlob.Data);
                if (entropyBlob.Data != IntPtr.Zero)
                    Marshal.FreeHGlobal(entropyBlob.Data);
            }
        }

        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool CryptUnprotectData(ref DataBlob input, ref string description, ref DataBlob entropy, IntPtr reserved, ref CryptProtectPrompt prompt, int dwFlags, ref DataBlob output);
    }
}
