using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.NSS
{
    public sealed class NSSAPI
    {
        private static IntPtr nssModule = IntPtr.Zero;

        /// <summary>
        /// Loads the required DLLs from the given directory
        /// </summary>
        /// <param name="nssDlls">Directory of the DLLs</param>
        public static void LoadDlls(DirectoryInfo nssDlls)
        {
            if (nssModule == IntPtr.Zero)
            {
                LoadLibrary(nssDlls.FullName + "\\msvcp120.dll");
                LoadLibrary(nssDlls.FullName + "\\msvcr120.dll");
                LoadLibrary(nssDlls.FullName + "\\mozglue.dll");
                nssModule = LoadLibrary(nssDlls.FullName + "\\nss3.dll");
            }
        }

        public NSSAPI(DirectoryInfo nssDlls)
        {
            LoadDlls(nssDlls);
        }

        /// <summary>
        /// Inializes NSS
        /// </summary>
        /// <param name="nssProfile"></param>
        public void LoadProfile(DirectoryInfo nssProfile)
        {
            NSS_Init(nssProfile.FullName);
            long keySlot = PK11_GetInternalKeySlot();
            PK11_Authenticate(keySlot, true, 0);
        }

        public string Decrypt(string encryptedText)
        {
            StringBuilder sb = new StringBuilder(encryptedText);
            int hi2 = NSSBase64_DecodeBuffer(IntPtr.Zero, IntPtr.Zero, sb, sb.Length);
            SECItem tSecDec = new SECItem();
            SECItem item = (SECItem)Marshal.PtrToStructure(new IntPtr(hi2), typeof(SECItem));
            if (PK11SDR_Decrypt(ref item, ref tSecDec, 0) == 0)
            {
                if (tSecDec.Len != 0)
                {
                    byte[] bvRet = new byte[tSecDec.Len];
                    Marshal.Copy(new IntPtr(tSecDec.Data), bvRet, 0, tSecDec.Len);
                    return Encoding.UTF8.GetString(bvRet);
                }
            }
            return null;
        }

        #region DLL calls
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void NSS_InitPtr(string configdir);
        private static void NSS_Init(string configdir)
        {
            IntPtr pProc = GetProcAddress(nssModule, "NSS_Init");
            NSS_InitPtr ptr = (NSS_InitPtr)Marshal.GetDelegateForFunctionPointer(pProc, typeof(NSS_InitPtr));
            ptr(configdir);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int PK11SDR_DecryptPtr(ref SECItem data, ref SECItem result, int cx);
        private static int PK11SDR_Decrypt(ref SECItem data, ref SECItem result, int cx)
        {
            IntPtr pProc = GetProcAddress(nssModule, "PK11SDR_Decrypt");
            PK11SDR_DecryptPtr ptr = (PK11SDR_DecryptPtr)Marshal.GetDelegateForFunctionPointer(pProc, typeof(PK11SDR_DecryptPtr));
            return ptr(ref data, ref result, cx);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long PK11_GetInternalKeySlotPtr();
        private static long PK11_GetInternalKeySlot()
        {
            IntPtr pProc = GetProcAddress(nssModule, "PK11_GetInternalKeySlot");
            PK11_GetInternalKeySlotPtr ptr = (PK11_GetInternalKeySlotPtr)Marshal.GetDelegateForFunctionPointer(pProc, typeof(PK11_GetInternalKeySlotPtr));
            return ptr();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long PK11_AuthenticatePtr(long slot, bool loadCerts, long wincx);
        private static long PK11_Authenticate(long slot, bool loadCerts, long wincx)
        {
            IntPtr pProc = GetProcAddress(nssModule, "PK11_Authenticate");
            PK11_AuthenticatePtr ptr = (PK11_AuthenticatePtr)Marshal.GetDelegateForFunctionPointer(pProc, typeof(PK11_AuthenticatePtr));
            return ptr(slot, loadCerts, wincx);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int NSSBase64_DecodeBufferPtr(IntPtr arenaOpt, IntPtr outItemOpt, StringBuilder inStr, int inLen);
        private static int NSSBase64_DecodeBuffer(IntPtr arenaOpt, IntPtr outItemOpt, StringBuilder inStr, int inLen)
        {
            IntPtr pProc = GetProcAddress(nssModule, "NSSBase64_DecodeBuffer");
            NSSBase64_DecodeBufferPtr ptr = (NSSBase64_DecodeBufferPtr)Marshal.GetDelegateForFunctionPointer(pProc, typeof(NSSBase64_DecodeBufferPtr));
            return ptr(arenaOpt, outItemOpt, inStr, inLen);
        }
        #endregion
    }
}
