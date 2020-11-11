using System;
using System.Runtime.InteropServices;

namespace Win32.Interop.Advapi32
{
    public static partial class ProcessThreadsApi
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public  static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

    }
}
