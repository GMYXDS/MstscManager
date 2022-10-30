using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MstscManager.Utils {
    public static class common_tools {
        public static Point Compute_absolute_point(Control obj) {
            int x = obj.Location.X;
            int y = obj.Location.Y;
            while (!(obj.Parent is FMain)) {
                x += obj.Parent.Location.X;
                y += obj.Parent.Location.Y;
                obj = obj.Parent;
            }
            return new Point(x, y);
        }
        public static void RunApp(string path_str, string arg) {
            Thread thread = new Thread(() => {
                Process pro = new Process();
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.CreateNoWindow = false;
                pro.StartInfo.FileName = @path_str;
                pro.StartInfo.Arguments = @arg;
                //pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                pro.StartInfo.WorkingDirectory = Path.GetDirectoryName(@path_str);
                pro.Start();
            });
            thread.Start();
            Thread.Sleep(200);
        }
        public static void RunApp2(string Path, string arg) {
            Thread thread = new Thread(() => {
                Process pro = new Process();
                pro.StartInfo.UseShellExecute = true;
                pro.StartInfo.CreateNoWindow = false;
                pro.StartInfo.FileName = Path;
                pro.StartInfo.Arguments = arg;
                //pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //pro.StartInfo.WorkingDirectory = Path;
                pro.Start();
            });
            thread.Start();
            Thread.Sleep(200);
        }
        public static void RunApp3(string Path, string arg) {
            Thread thread = new Thread(() => {
                Process pro = new Process();
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.CreateNoWindow = false;
                pro.StartInfo.FileName = @Path;
                pro.StartInfo.Arguments = arg;
                //pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //pro.StartInfo.WorkingDirectory = Path;
                pro.Start();
            });
            thread.Start();
            Thread.Sleep(200);
        }
        public static string pinghost(string host) {
            try {
                Ping p1 = new Ping();
                PingReply reply = p1.Send(host); //发送主机名或Ip地址
                string status = "失败";
                if (reply.Status == IPStatus.Success) {
                    //status = reply.Options.Ttl.ToString();
                    status = reply.RoundtripTime.ToString()+"ms";
                } else if (reply.Status == IPStatus.TimedOut) {
                    status = "超时";
                }
                return status;
            } catch (Exception) {
                return "超时";
            }
        }
        public static string md5(string str) {
            MD5 md5 = MD5.Create();
            byte[] buffer = Encoding.Default.GetBytes(str);
            byte[] MD5Buffer = md5.ComputeHash(buffer);
            string strNew = "";
            for (int i = 0; i < MD5Buffer.Length; i++) {
                strNew += MD5Buffer[i].ToString("x2");
            }
            return strNew;
        }
    }
}
