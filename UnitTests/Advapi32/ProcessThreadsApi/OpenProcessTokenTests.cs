using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using static Win32.Interop.Advapi32.ProcessThreadsApi;
using static Win32.Interop.Kernel32.HandleApi;

namespace UnitTests.Advapi32.ProcessThreadsApi
{
    [TestClass]
    public class OpenProcessTokenTests
    {
        [TestMethod]
        public void OpenProcessToken_Success()
        {
            IntPtr processHandle = IntPtr.Zero;
            
            try
            {
                var process = Process.GetCurrentProcess();
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                string user = wi.Name;
                Assert.IsFalse(string.IsNullOrEmpty(wi.Name));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }

        }
    }
}
