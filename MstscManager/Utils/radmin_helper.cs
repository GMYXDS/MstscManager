using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MstscManager.Utils {
    public static class radmin_helper {
        #region Windows API 调用
        public static int WM_CHAR = 0x102;
        public static int WM_CLICK = 0x00F5;
        public static int WM_GETTEXT = 0x000D;
        public static int WM_SETTEXT = 0x000C;

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll")]
        public static extern int AnyPopup();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int EnumThreadWindows(IntPtr dwThreadId, CallBack lpfn, int lParam);

        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessageA(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        public delegate bool CallBack(IntPtr hwnd, int lParam);

        public static void SendChar(IntPtr hand, char ch, int SleepTime) {
            PostMessage(hand, WM_CHAR, ch, 0);
            System.Threading.Thread.Sleep(SleepTime);
        }
        #endregion
        public static void auto_password(string ip,string user,string pass) {
            #region 钩子过程
            IntPtr mainWnd = FindWindow(null, "Radmin 安全性: "+ip);
            Thread.Sleep(1000);
            mainWnd = FindWindow(null, "Radmin 安全性: " + ip);
            for (int i = 0; i < 30; i++)        //最大查找3秒
            {
                if (mainWnd != IntPtr.Zero) {
                    Trace.WriteLine(System.DateTime.UtcNow.ToString() + "找到！");
                    break;
                }
                Thread.Sleep(100);
            }
            if (mainWnd == IntPtr.Zero) {//3秒内找不到就退出。
                Console.WriteLine("没找到窗口");
                mainWnd = FindWindow(null, "Windows 安全性：" + ip);
                for (int i = 0; i < 30; i++)        //最大查找3秒
                {
                    if (mainWnd != IntPtr.Zero) {
                        Trace.WriteLine(System.DateTime.UtcNow.ToString() + "找到！");
                        break;
                    }
                    Thread.Sleep(100);
                }
                if (mainWnd == IntPtr.Zero) {//3秒内找不到就退出。
                    return;
                }
            }        

            List<IntPtr> listWnd = new List<IntPtr>();
            IntPtr hwnd_edit = IntPtr.Zero;
            int edit_id = 0;        //第几个编辑框
            do {
                hwnd_edit = FindWindowEx(mainWnd, hwnd_edit, "Edit", null);
                char[] UserChar = user.ToCharArray();
                char[] passChar = pass.ToCharArray();
                if (edit_id <= 0) {
                    foreach (char ch in UserChar) {
                        SendChar(hwnd_edit, ch, 5);
                    }
                } else {
                    foreach (char ch in passChar) {
                        SendChar(hwnd_edit, ch, 5);
                    }
                }
                edit_id++;       //输完后，编辑框序号变1
            } while (!hwnd_edit.Equals(IntPtr.Zero));
            IntPtr hwnd_button = FindWindowEx(mainWnd, new IntPtr(0), null, "确定");
            if (hwnd_button != IntPtr.Zero) {
                Trace.WriteLine(System.DateTime.UtcNow.ToString() + "找到按钮！");
                SendMessage(hwnd_button, WM_CLICK, mainWnd, "0");
            }
            #endregion
        }
    }
}
