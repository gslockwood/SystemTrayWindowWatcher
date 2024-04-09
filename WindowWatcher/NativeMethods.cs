using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowWatcher
{
    public class NativeMethods
    {
        public static uint SWP_NOSIZE = 0x0001;
        public static uint SWP_NOZORDER = 0x0004;
        public static uint SWP_SHOWWINDOW = 0x0040;

        public static uint GW_HWNDNEXT = 0x0002;
        public static uint GW_HWNDPREV = 0x0003;
        public static uint GW_HWNDLAST = 0x0001;
        public static uint GW_HWNDFIRST = 0x0000;

        public static uint SW_MINIMIZE = 0x0006;


        //[DllImport( "user32.dll" )] public static extern bool GetCursorPos( out LPPOINT lpPoint );
        [DllImport( "user32.dll" )] public static extern bool SetForegroundWindow( IntPtr hWnd );
        [DllImport( "user32.dll" )] public static extern IntPtr GetForegroundWindow();
        [DllImport( "user32.dll", EntryPoint = "SetWindowPos" )] public static extern IntPtr SetWindowPos( IntPtr hWnd, int hWndInsertAfter, int x, int Y, uint cx, uint cy, uint wFlags );
        [DllImport( "user32.dll" )] public static extern long GetWindowRect( IntPtr hWnd, ref Rectangle lpRect );
        [DllImport( "user32.dll", SetLastError = true )] public static extern IntPtr GetWindow( IntPtr hWnd, uint uCmd );
        [DllImport( "user32.dll", CharSet = CharSet.Auto )] public static extern int GetWindowText( IntPtr hWnd, StringBuilder lpString, int nMaxCount );
        [DllImport( "user32.dll", SetLastError = true )] public static extern uint GetWindowThreadProcessId( IntPtr hWnd, out uint processId );
        [DllImport( "user32.dll" )] public static extern bool ShowWindow( IntPtr hWnd, uint nCmdShow );
        [DllImport( "Kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true )] public static extern IntPtr GetConsoleWindow();
        [DllImport( "user32.dll" )] public static extern IntPtr GetTopWindow( IntPtr hWnd );
        //[DllImport( "user32.dll", EntryPoint = "GetWindow" )] public static extern IntPtr GetNextWindow( IntPtr hWnd, uint wCmd );
        [DllImport( "user32.dll" )] public static extern bool EnumThreadWindows( int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam );
        [DllImport( "user32.dll" )] public static extern bool IsWindowVisible( IntPtr hWnd );
        [DllImport( "user32.dll", EntryPoint = "GetDesktopWindow" )] public static extern IntPtr GetDesktopWindow();
        [DllImport( "user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto )] public static extern IntPtr FindWindowEx( IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow );


        public delegate bool EnumThreadDelegate( IntPtr hWnd, IntPtr lParam );
        //
        //
        //
        public void MinimizeConsoleWindow()
        {
            NativeMethods.ShowWindow( NativeMethods.GetConsoleWindow(), NativeMethods.SW_MINIMIZE );
        }

        public static bool GetWindowZOrder( IntPtr hwnd, out int zOrder )
        {
            const uint GW_HWNDPREV = 3;
            const uint GW_HWNDLAST = 1;

            var lowestHwnd = NativeMethods.GetWindow( hwnd, GW_HWNDLAST );

            var z = 0;
            var hwndTmp = lowestHwnd;
            while( hwndTmp != IntPtr.Zero )
            {
                if( hwnd == hwndTmp )
                {
                    zOrder = z;
                    return true;
                }

                hwndTmp = NativeMethods.GetWindow( hwndTmp, GW_HWNDPREV );
                z++;
            }

            zOrder = int.MinValue;
            return false;
        }
    }


}
