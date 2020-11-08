using System;
using System.Runtime.InteropServices;
using Win32.Interop.CredUi;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");



            var pszUserName = "test";
            var pszPassword = "test2";

            var pcbPackedCredentials = 1024;
            var pPackedCredentials = Marshal.AllocCoTaskMem(pcbPackedCredentials);

            var oke = WinCred.CredPackAuthenticationBuffer(WinCred.CredPackFlags.CRED_PACK_PROTECTED_CREDENTIALS, pszUserName, pszPassword, pPackedCredentials, ref pcbPackedCredentials);
            //var error = Marshal.GetLastWin32Error();
            //var win32Exception = new Win32Exception((int)error);

        }
    }
}
