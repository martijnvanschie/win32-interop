using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32.Interop.CredUi
{
    public static partial class WinCred
    {
		/// <summary>
		/// Specifies how the credential should be packed. This can be a combination of the following flags.
		/// </summary>
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

		[Flags]
		public enum CredentialFlag
		{
			CREDUIWIN_GENERIC = 0x1,
			CREDUIWIN_CHECKBOX = 0x2,
			CREDUIWIN_AUTHPACKAGE_ONLY = 0x10,
			CREDUIWIN_IN_CRED_ONLY = 0x20,
			CREDUIWIN_ENUMERATE_ADMINS = 0x100,
			CREDUIWIN_ENUMERATE_CURRENT_USER = 0x200,
			CREDUIWIN_SECURE_PROMPT = 0x1000,
			CREDUIWIN_PACK_32_WOW = 0x10000000,
		}

		/// <summary>
		/// The CredPackAuthenticationBuffer function converts a string user name and password into an authentication buffer.
		/// </summary>
		/// <param name="dwFlags">Specifies how the credential should be packed. This can be a combination of the following flags.</param>
		/// <param name="pszUserName">A pointer to a null-terminated string that specifies the user name to be converted.</param>
		/// <param name="pszPassword">A pointer to a null-terminated string that specifies the password to be converted.</param>
		/// <param name="pPackedCredentials">A pointer to an array of bytes that, on output, receives the packed authentication buffer. This parameter can be NULL to receive the required buffer size in the pcbPackedCredentials parameter.</param>
		/// <param name="pcbPackedCredentials">A pointer to a DWORD value that specifies the size, in bytes, of the pPackedCredentials buffer. On output, if the buffer is not of sufficient size, specifies the required size, in bytes, of the pPackedCredentials buffer.</param>
		/// <returns></returns>
		/// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/api/wincred/nf-wincred-credpackauthenticationbuffera"/>
		[DllImport("credui.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        //[PInvokeData("wincred.h", MSDNShortId = "aa374802")]
        public static extern bool CredPackAuthenticationBuffer(CredPackFlags dwFlags, string pszUserName, string pszPassword, IntPtr pPackedCredentials, ref int pcbPackedCredentials);

		[DllImport("credui.dll", CharSet = CharSet.Auto)]
		public static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO uiInfo,int authError,ref uint authPackage,IntPtr InAuthBuffer,uint InAuthBufferSize,out IntPtr refOutAuthBuffer,out uint refOutAuthBufferSize,	ref bool fSave,CredentialFlag flags);

		[DllImport("credui.dll", CharSet = CharSet.Auto)]
		public static extern bool CredUnPackAuthenticationBuffer(CredPackFlags dwFlags,
																   IntPtr pAuthBuffer,
																   uint cbAuthBuffer,
																   StringBuilder pszUserName,
																   ref int pcchMaxUserName,
																   StringBuilder pszDomainName,
																   ref int pcchMaxDomainame,
																   StringBuilder pszPassword,
																   ref int pcchMaxPassword);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct CREDUI_INFO
		{
			public int cbSize;
			public IntPtr hwndParent;
			public string pszMessageText;
			public string pszCaptionText;
			public IntPtr hbmBanner;
		}
	}
}
