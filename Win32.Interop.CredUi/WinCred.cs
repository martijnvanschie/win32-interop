using System;
using System.Runtime.InteropServices;

namespace Win32.Interop.CredUi
{
    public static partial class WinCred
    {
		[Flags]
		public enum CredPackFlags
		{
			/// <summary>Encrypts the credential so that it can only be decrypted by processes in the caller's logon session.</summary>
			CRED_PACK_PROTECTED_CREDENTIALS = 0x1,

			/// <summary>Encrypts the credential in a WOW buffer.</summary>
			CRED_PACK_WOW_BUFFER = 0x2,

			/// <summary>Encrypts the credential in a CRED_GENERIC buffer.</summary>
			CRED_PACK_GENERIC_CREDENTIALS = 0x4,

			/// <summary>
			/// Encrypts the credential of an online identity into a SEC_WINNT_AUTH_IDENTITY_EX2 structure.If CRED_PACK_GENERIC_CREDENTIALS
			/// and CRED_PACK_ID_PROVIDER_CREDENTIALS are not set, encrypts the credentials in a KERB_INTERACTIVE_LOGON buffer.
			/// <para><c>Windows 7, Windows Server 2008 R2, Windows Vista, Windows Server 2008:</c> This value is not supported.</para>
			/// </summary>
			CRED_PACK_ID_PROVIDER_CREDENTIALS = 0x8
		}

		[DllImport("credui.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        //[PInvokeData("wincred.h", MSDNShortId = "aa374802")]
        public static extern bool CredPackAuthenticationBuffer(CredPackFlags dwFlags, string pszUserName, string pszPassword, IntPtr pPackedCredentials, ref int pcbPackedCredentials);

    }
}
