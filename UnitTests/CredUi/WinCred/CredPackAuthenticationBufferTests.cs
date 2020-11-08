using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Win32.Interop.CredUi;

namespace UnitTests.CredUi.WinCredTests
{
    [TestClass]
    public class CredPackAuthenticationBufferTests
    {
        [TestMethod]
        public void CredPackAuthenticationBufferOke()
        {
            var pszUserName = "test";
            var pszPassword = "test2";

            var pcbPackedCredentials = 1024;
            var pPackedCredentials = Marshal.AllocCoTaskMem(pcbPackedCredentials);

            var oke = WinCred.CredPackAuthenticationBuffer(WinCred.CredPackFlags.CRED_PACK_PROTECTED_CREDENTIALS, pszUserName, pszPassword, pPackedCredentials, ref pcbPackedCredentials);
            Assert.IsTrue(oke);
        }

        [TestMethod]
        public void CredPackAuthenticationBufferInvalidBuffersize()
        {
            var pszUserName = "test";
            var pszPassword = "test2";

            var pcbPackedCredentials = 0;
            var pPackedCredentials = Marshal.AllocCoTaskMem(pcbPackedCredentials);

            var oke = WinCred.CredPackAuthenticationBuffer(WinCred.CredPackFlags.CRED_PACK_PROTECTED_CREDENTIALS, pszUserName, pszPassword, pPackedCredentials, ref pcbPackedCredentials);
            Assert.IsFalse(oke);

            var error = Marshal.GetLastWin32Error();
            Assert.AreEqual(error, 122);

            var win32Exception = new Win32Exception((int)error);
            Assert.AreEqual(win32Exception.Message, "The data area passed to a system call is too small.");
        }
    }
}
