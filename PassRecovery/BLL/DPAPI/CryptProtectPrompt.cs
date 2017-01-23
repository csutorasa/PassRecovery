using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.DPAPI
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CryptProtectPrompt
    {
        public int Length;
        public int DwPromptFlags;
        public IntPtr HwndApp;
        public string PromptText;
        public static CryptProtectPrompt Create()
        {
            CryptProtectPrompt cryptProtectPrompt = new CryptProtectPrompt();
            cryptProtectPrompt.Length = Marshal.SizeOf(typeof(CryptProtectPrompt));
            cryptProtectPrompt.DwPromptFlags = 0;
            cryptProtectPrompt.HwndApp = IntPtr.Zero;
            cryptProtectPrompt.PromptText = null;
            return cryptProtectPrompt;
        }
    }
    public enum CryptProptectInitalizationFlags
    {
        UiForbidden = 0x1,
        LocalMachine = 0x4
    }
}
