using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static Win32.Interop.Advapi32.NtSecApi;
using static Win32.Interop.Advapi32.SecurityBaseApi;
using static Win32.Interop.Secur32.NtSecApi;

namespace UnitTests.Secur32.NtSecApi
{
    [TestClass]
    public class LsaLogonUserTests
    {
        [TestMethod]
        public void LsaLogonUser_Success()
        {
            var connectStatus = LsaConnectUntrusted(out var lsaHande);
            var lsaString = new LSA_STRING("Kerberos");
            var lsaStatus = LsaLookupAuthenticationPackage(lsaHande, ref lsaString, out var authenticationPackage);
            
            AllocateLocallyUniqueId(out var srcLuid);
            var tokenSource = new TOKEN_SOURCE { SourceName = "foobar12".ToCharArray(), SourceIdentifier = srcLuid };
            
            var lsaOriginName = new LSA_STRING("Kerberos");

            var kerb = new KERB_INTERACTIVE_LOGON()
            {
                MessageType = KERB_LOGON_SUBMIT_TYPE.KerbInteractiveLogon,
                LogonDomainName = new UNICODE_STRING("eu"),
                UserName = new UNICODE_STRING("martijn"),
                Password = new UNICODE_STRING("Unisys!1")
            };

            IntPtr info = (IntPtr)1024;// Marshal.SizeOf(kerb);
            Marshal.StructureToPtr(kerb, info, false);

            PTOKEN_GROUPS groups = new PTOKEN_GROUPS() { GroupCount = 0 };
            IntPtr profileBuffer = IntPtr.Zero;
            UInt32 profileBufferLength = 0;
            Int64 logonId;
            IntPtr token = IntPtr.Zero;
            UInt32 subStatus;
            QUOTA_LIMITS quotas;

            var logon = LsaLogonUser(lsaHande, ref lsaOriginName, SECURITY_LOGON_TYPE.Interactive, authenticationPackage, info, 1024, groups,
                ref tokenSource, out profileBuffer, out profileBufferLength, out logonId, out token, out quotas, out subStatus);
        }
    }

    [TestClass]
    public class LsaLookupAuthenticationPackageTests
    {
        [TestMethod]
        public void LsaLookupAuthenticationPackage_Negotiate_Success()
        {
            var connectStatus = LsaConnectUntrusted(out var lsaHande);

            var lsaString = new LSA_STRING("Negotiate");
            var lsaStatus = LsaLookupAuthenticationPackage(lsaHande, ref lsaString, out var authenticationPackage);

            var winErrorCode = LsaNtStatusToWinError(lsaStatus);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The operation completed successfully.");
        }

        [TestMethod]
        public void LsaLookupAuthenticationPackage_Kerberos_Success()
        {
            var connectStatus = LsaConnectUntrusted(out var lsaHande);

            var lsaString = new LSA_STRING("Kerberos");
            var lsaStatus = LsaLookupAuthenticationPackage(lsaHande, ref lsaString, out var authenticationPackage);

            var winErrorCode = LsaNtStatusToWinError(lsaStatus);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The operation completed successfully.");
        }

        [TestMethod]
        public void LsaLookupAuthenticationPackage_MSV1_0_Success()
        {
            var connectStatus = LsaConnectUntrusted(out var lsaHande);

            var lsaString = new LSA_STRING("MICROSOFT_AUTHENTICATION_PACKAGE_V1_0");
            var lsaStatus = LsaLookupAuthenticationPackage(lsaHande, ref lsaString, out var authenticationPackage);

            var winErrorCode = LsaNtStatusToWinError(lsaStatus);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The operation completed successfully.");
        }

        [TestMethod]
        public void LsaLookupAuthenticationPackage_Packages_Unknown()
        {
            var connectStatus = LsaConnectUntrusted(out var lsaHande);

            var lsaString = new LSA_STRING("unknown");
            var lsaStatus = LsaLookupAuthenticationPackage(lsaHande, ref lsaString, out var authenticationPackage);

            var winErrorCode = LsaNtStatusToWinError(lsaStatus);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "A specified authentication package is unknown.");
        }

        [TestMethod]
        public void LsaLookupAuthenticationPackage_Invalid_Handle()
        {
            var lsaString = new LSA_STRING("Negotiate");
            var lsaStatus = LsaLookupAuthenticationPackage(IntPtr.Zero, ref lsaString, out var authenticationPackage);

            var winErrorCode = LsaNtStatusToWinError(lsaStatus);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "An unexpected network error occurred.");
        }
    }



    [TestClass]
    public class LsaConnectUntrustedTests
    {
        [TestMethod]
        public void LsaConnectUntrusted_Success()
        {
            var connectStatus = LsaConnectUntrusted(out var lsaHande);
            var winErrorCode = LsaNtStatusToWinError(connectStatus);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The operation completed successfully.");

            var deregisterStatus = LsaDeregisterLogonProcess(lsaHande);
            winErrorCode = LsaNtStatusToWinError(deregisterStatus);
            win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The operation completed successfully.");

            Assert.IsTrue(true);
        }
    }

    [TestClass]
    public class LsaDeregisterLogonProcessTests
    {
        [TestMethod]
        public void LsaDeregisterLogonProcess_Success()
        {
            var connectStatus = LsaConnectUntrusted(out var lsaHande);
            var winErrorCode = LsaNtStatusToWinError(connectStatus);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The operation completed successfully.");

            var deregisterStatus = LsaDeregisterLogonProcess(lsaHande);
            winErrorCode = LsaNtStatusToWinError(deregisterStatus);
            win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The operation completed successfully.");

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void LsaDeregisterLogonProcess_InvalidHandle()
        {
            var deregisterStatus = LsaDeregisterLogonProcess(IntPtr.Zero);
            var winErrorCode = LsaNtStatusToWinError(deregisterStatus);
            var win32Exception = new Win32Exception((int)winErrorCode);
            Assert.AreEqual(win32Exception.Message, "The handle is invalid.");
        }
    }
}
