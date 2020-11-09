using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Win32.Interop.COM.Winerror;
using static Win32.Interop.Advapi32.NtSecApi;

namespace UnitTests.Advapi32.NtSecApi
{
    [TestClass]
    public class LsaNtStatusToWinErrorTests
    {
        [TestMethod]
        public void LsaNtStatusToWinErrorSuccess()
        {
            var winErrorCode = LsaNtStatusToWinError(HRESULT.S_OK);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The operation completed successfully.");
        }

        [TestMethod]
        public void LsaNtStatusToWinErrorErrorWait2()
        {
            var winErrorCode = LsaNtStatusToWinError(2);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "ERROR_WAIT_2");
        }
    }
}
