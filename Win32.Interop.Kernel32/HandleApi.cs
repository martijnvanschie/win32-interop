using System;
using System.Runtime.InteropServices;

namespace Win32.Interop.Kernel32
{
    public static partial class HandleApi
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);
    }
}
