using System;
using System.Runtime.InteropServices;

namespace Win32.Interop.Advapi32
{
    public static partial class NtSecApi
    {
        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/win32/api/ntsecapi/nf-ntsecapi-lsantstatustowinerror
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern uint LsaNtStatusToWinError(uint status);
    }
}
