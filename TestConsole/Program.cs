using System;
using System.Runtime.InteropServices;
using System.Text;
//using Win32.Interop.CredUi;
using static Win32.Interop.CredUi.WinCred;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting test console for Win32 interop");

            DisplayCredUi();
        }

        private static void DisplayCredUi()
        {
            var uiInfo = new CREDUI_INFO()
            {
                pszCaptionText = "Enter credentials",
                pszMessageText = "Please use test credentials als they will be logged to the console when succesful."
            };

            uiInfo.cbSize = Marshal.SizeOf(uiInfo);

            uint authPackage = 0;

            var save = false;

            var result = CredUIPromptForWindowsCredentials(
                ref uiInfo,
                0,
                ref authPackage,
                IntPtr.Zero,
                0,
                out IntPtr outCredBuffer,
                out uint outCredSize,
                ref save,
                0
            );

            var usernameBuf = new StringBuilder(100);
            var passwordBuf = new StringBuilder(100);
            var domainBuf = new StringBuilder(100);

            var maxUserName = 100;
            var maxDomain = 100;
            var maxPassword = 100;

            if (CredUnPackAuthenticationBuffer(CredPackFlags.CRED_PACK_PROTECTED_CREDENTIALS, outCredBuffer, outCredSize, usernameBuf, ref maxUserName,
                                   domainBuf, ref maxDomain, passwordBuf, ref maxPassword))
            {

                var UserName = usernameBuf.ToString();
                var Password = passwordBuf.ToString();
                var Domain = domainBuf.ToString();

                Console.WriteLine($"Username=[{UserName}], Password=[{Password}], Domain=[{Domain}]");
            }
        }
    }
}
