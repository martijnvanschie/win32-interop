using System;
using System.Runtime.InteropServices;
using System.Text;
using static Win32.Interop.Advapi32.SecurityBaseApi;

namespace Win32.Interop.Secur32
{
    public static partial class NtSecApi
    {
        [DllImport("secur32.dll", SetLastError = false)]
        public static extern uint LsaConnectUntrusted([Out] out IntPtr lsaHandle);

        [DllImport("secur32.dll", SetLastError = false)]
        public static extern uint LsaLookupAuthenticationPackage([In] IntPtr lsaHandle, [In] ref LSA_STRING packageName, [Out] out UInt32 authenticationPackage);

        [DllImport("secur32.dll", SetLastError = false)]
        public static extern uint LsaDeregisterLogonProcess([In] IntPtr lsaHandle);

        [DllImport("secur32.dll", SetLastError = false, ExactSpelling = true)]
        public static extern uint LsaEnumerateLogonSessions(out uint LogonSessionCount, out IntPtr LogonSessionList);

		/// <summary>
		/// TODO: Change return uint and SubStatus to typed object
		/// </summary>
		/// <param name="LsaHandle"></param>
		/// <param name="OriginName"></param>
		/// <param name="LogonType"></param>
		/// <param name="AuthenticationPackage"></param>
		/// <param name="AuthenticationInformation"></param>
		/// <param name="AuthenticationInformationLength"></param>
		/// <param name="LocalGroups"></param>
		/// <param name="SourceContext"></param>
		/// <param name="ProfileBuffer"></param>
		/// <param name="ProfileBufferLength"></param>
		/// <param name="LogonId"></param>
		/// <param name="Token"></param>
		/// <param name="Quotas"></param>
		/// <param name="SubStatus"></param>
		/// <returns></returns>
        [DllImport("secur32.dll", SetLastError = false)]
        public static extern /*WinStatusCodes */ uint LsaLogonUser(
                    [In] IntPtr LsaHandle,
                    [In] ref LSA_STRING OriginName,
                    [In] SECURITY_LOGON_TYPE LogonType,
                    [In] UInt32 AuthenticationPackage,
                    [In] IntPtr AuthenticationInformation,
                    [In] UInt32 AuthenticationInformationLength,
                    [In] PTOKEN_GROUPS LocalGroups,
                    [In] ref TOKEN_SOURCE SourceContext,
                    [Out] /*PVOID*/ out IntPtr ProfileBuffer,
                    [Out] out UInt32 ProfileBufferLength,
                    [Out] out Int64 LogonId,
                    [Out] out IntPtr Token,
                    [Out] out QUOTA_LIMITS Quotas,
                    [Out] out /*WinStatusCodes */ uint SubStatus
                    );
    }

    public static partial class NtSecApi
    {
		[StructLayout(LayoutKind.Sequential)]
		public struct KERB_INTERACTIVE_LOGON
		{
			/// <summary>KERB_LOGON_SUBMIT_TYPE value identifying the type of logon request being made. This member must be set to <c>KerbInteractiveLogon</c>.</summary>
			public KERB_LOGON_SUBMIT_TYPE MessageType;

			/// <summary>UNICODE_STRING specifying the name of the target logon domain.</summary>
			public UNICODE_STRING LogonDomainName;

			/// <summary>UNICODE_STRING specifying the user name.</summary>
			public UNICODE_STRING UserName;

			/// <summary>
			/// UNICODE_STRING specifying the user password. When you have finished using the password, remove the sensitive information from
			/// memory by calling SecureZeroMemory. For more information on protecting the password, see Handling Passwords.
			/// </summary>
			public UNICODE_STRING Password;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct UNICODE_STRING : IDisposable
		{
			public ushort Length;
			public ushort MaximumLength;
			private IntPtr buffer;

			public UNICODE_STRING(string s)
			{
				Length = (ushort)(s.Length * 2);
				MaximumLength = (ushort)(Length + 2);
				buffer = Marshal.StringToHGlobalUni(s);
			}

			public void Dispose()
			{
				Marshal.FreeHGlobal(buffer);
				buffer = IntPtr.Zero;
			}

			public override string ToString()
			{
				return Marshal.PtrToStringUni(buffer);
			}
		}

		public enum KERB_LOGON_SUBMIT_TYPE
		{
			/// <summary>Perform an interactive logon.</summary>
			KerbInteractiveLogon = 2,
			/// <summary>Logon using a smart card.</summary>
			KerbSmartCardLogon = 6,
			/// <summary>Unlock a workstation.</summary>
			KerbWorkstationUnlockLogon,
			/// <summary>Unlock a workstation using a smart card.</summary>
			KerbSmartCardUnlockLogon,
			/// <summary>Logon using a proxy server.</summary>
			KerbProxyLogon,
			/// <summary>Logon using a valid Kerberos ticket as a credential.</summary>
			KerbTicketLogon,
			/// <summary>Unlock a workstation by using a Kerberos ticket.</summary>
			KerbTicketUnlockLogon,
			/// <summary>Perform a service for user logon.</summary>
			KerbS4ULogon,
			/// <summary>Logon interactively using a certificate stored on a smart card.</summary>
			KerbCertificateLogon,
			/// <summary>Perform a service for user logon using a certificate stored on a smart card.</summary>
			KerbCertificateS4ULogon,
			/// <summary>Unlock a workstation using a certificate stored on a smart card.</summary>
			KerbCertificateUnlockLogon,
			/// <summary />
			KerbNoElevationLogon = 83,
			/// <summary />
			KerbLuidLogon,
		}

		/// <summary>The QUOTA_LIMITS structure describes the amount of system resources available to a user.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct QUOTA_LIMITS
		{
			/// <summary>
			/// Specifies the amount of paged pool memory assigned to the user. The paged pool is an area of system memory (physical memory
			/// used by the operating
			/// system) for objects that can be written to disk when they are not being used.
			/// <para>
			/// The value set in this member is not enforced by the LSA. You should set this member to 0, which causes the default value to
			/// be used.
			/// </para>
			/// </summary>
			public uint PagedPoolLimit;

			/// <summary>
			/// Specifies the amount of nonpaged pool memory assigned to the user. The nonpaged pool is an area of system memory for objects
			/// that cannot be written to disk but must remain in physical memory as long as they are allocated.
			/// <para>
			/// The value set in this member is not enforced by the LSA. You should set this member to 0, which causes the default value to
			/// be used.
			/// </para>
			/// </summary>
			public uint NonPagedPoolLimit;

			/// <summary>
			/// Specifies the minimum set size assigned to the user. The "working set" of a process is the set of memory pages currently
			/// visible to the process in physical RAM memory. These pages are present in memory when the application is running and
			/// available for an application to use without triggering a page fault.
			/// <para>
			/// The value set in this member is not enforced by the LSA. You should set this member to NULL, which causes the default value
			/// to be used.
			/// </para>
			/// </summary>
			public uint MinimumWorkingSetSize;

			/// <summary>
			/// Specifies the maximum set size assigned to the user.
			/// <para>
			/// The value set in this member is not enforced by the LSA. You should set this member to 0, which causes the default value to
			/// be used.
			/// </para>
			/// </summary>
			public uint MaximumWorkingSetSize;

			/// <summary>
			/// Specifies the maximum size, in bytes, of the paging file, which is a reserved space on disk that backs up committed physical
			/// memory on the computer.
			/// <para>
			/// The value set in this member is not enforced by the LSA. You should set this member to 0, which causes the default value to
			/// be used.
			/// </para>
			/// </summary>
			public uint PagefileLimit;

			/// <summary>
			/// Indicates the maximum amount of time the process can run.
			/// <para>
			/// The value set in this member is not enforced by the LSA. You should set this member to NULL, which causes the default value
			/// to be used.
			/// </para>
			/// </summary>
			public long TimeLimit;
		}

		public struct PTOKEN_GROUPS
        {
            public uint GroupCount;
            
            [MarshalAs(UnmanagedType.ByValArray)]
            public SID_AND_ATTRIBUTES[] Groups;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public uint Attributes;
        }

		/// <summary>The TOKEN_SOURCE structure identifies the source of an access token.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct TOKEN_SOURCE
		{
			private const int TOKEN_SOURCE_LENGTH = 8;

			/// <summary>
			/// Specifies an 8-byte character string used to identify the source of an access token. This is used to distinguish between such
			/// sources as Session Manager, LAN Manager, and RPC Server. A string, rather than a constant, is used to identify the source so
			/// users and developers can make extensions to the system, such as by adding other networks, that act as the source of access tokens.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = TOKEN_SOURCE_LENGTH)]
			public char[] SourceName;

			/// <summary>
			/// Specifies a locally unique identifier (LUID) provided by the source component named by the SourceName member. This value aids
			/// the source component in relating context blocks, such as session-control structures, to the token. This value is typically,
			/// but not necessarily, an LUID.
			/// </summary>
			public UInt64 SourceIdentifier;
		}

		/// <summary>The <c>SECURITY_LOGON_TYPE</c> enumeration indicates the type of logon requested by a logon process.</summary>
		// https://docs.microsoft.com/en-us/windows/desktop/api/ntsecapi/ne-ntsecapi-_security_logon_type typedef enum _SECURITY_LOGON_TYPE {
		// UndefinedLogonType, Interactive, Network, Batch, Service, Proxy, Unlock, NetworkCleartext, NewCredentials, RemoteInteractive,
		// CachedInteractive, CachedRemoteInteractive, CachedUnlock } SECURITY_LOGON_TYPE, *PSECURITY_LOGON_TYPE;
		public enum SECURITY_LOGON_TYPE
		{
			/// <summary>The undefined logon type</summary>
			UndefinedLogonType = 0,

			/// <summary>The security principal is logging on interactively.</summary>
			Interactive = 2,

			/// <summary>The security principal is logging using a network.</summary>
			Network,

			/// <summary>The logon is for a batch process.</summary>
			Batch,

			/// <summary>The logon is for a service account.</summary>
			Service,

			/// <summary>Not supported.</summary>
			Proxy,

			/// <summary>The logon is an attempt to unlock a workstation.</summary>
			Unlock,

			/// <summary>The logon is a network logon with plaintext credentials.</summary>
			NetworkCleartext,

			/// <summary>
			/// Allows the caller to clone its current token and specify new credentials for outbound connections. The new logon session has
			/// the same local identity but uses different credentials for other network connections.
			/// </summary>
			NewCredentials,

			/// <summary>A terminal server session that is both remote and interactive.</summary>
			RemoteInteractive,

			/// <summary>Attempt to use the cached credentials without going out across the network.</summary>
			CachedInteractive,

			/// <summary>Same as RemoteInteractive, except used internally for auditing purposes.</summary>
			CachedRemoteInteractive,

			/// <summary>The logon is an attempt to unlock a workstation.</summary>
			CachedUnlock,
		}
	}
}
