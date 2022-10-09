using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WInApiWinBottomLib.Model
{
    public class WinButtom
    {
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOACTIVATE = 0x0010;
        const UInt32 SWP_NOZORDER = 0x0004;
        const int WM_ACTIVATEAPP = 0x001C;
        const int WM_ACTIVATE = 0x0006;
        const int WM_SETFOCUS = 0x0007;
        const int WM_WINDOWPOSCHANGING = 0x0046;
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        public IntPtr HWD
        {
            get;set;
        } = new IntPtr();

        public Window MainWindow
        {
            get; set;
        }
        public void SetButtom()
        {
            if(HWD == IntPtr.Zero || MainWindow == null)
                return;

            WinApi.SetWindowPos(HWD, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
        }
        public void SerEvent()
        {

            MainWindow.Loaded += (o, e) =>
            {  
                HwndSource src = HwndSource.FromHwnd(HWD);
                src.AddHook(new HwndSourceHook(WndProc));

                WinApi.SetWindowPos(HWD, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

            };
            MainWindow.MouseEnter += (o, e) =>
            {
                
               WinApi.SetWindowPos(HWD, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
            };
            MainWindow.Closing += (o, e) =>
            {
                HwndSource src = HwndSource.FromHwnd(HWD);
                src.RemoveHook(new HwndSourceHook(this.WndProc));
            };
        }
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SETFOCUS)
            {
                
                WinApi.SetWindowPos(HWD, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                handled = true;
            }
            return IntPtr.Zero;
        }
        public WinButtom( Window window )
        {
            if (window == null)
                return;
            HWD = new WindowInteropHelper(window).Handle;
            MainWindow = window;
        }
    }
}
