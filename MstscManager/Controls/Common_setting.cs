using Microsoft.Win32;
using MstscManager.Utils;
using Sunny.UI;
using System.IO;

namespace MstscManager.Controls {
    public partial class Common_setting : UIPage {
        public Common_setting() {
            InitializeComponent();
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e) {
            //RegistryKey rkey = Registry.CurrentUser;
            //RegistryKey key = rkey.CreateSubKey(@"SOFTWARE\MstscManager");
            //key = rkey.OpenSubKey(@"SOFTWARE\MstscManager", true);
            if ((sender as UICheckBox).Checked) {
                //to do 设置启动true;
                //key.SetValue("is_open_with_mm", "1");
                DbInihelper.SetIniData(Share.iniconfig_action, "is_open_with_mm", "1", Share.iniconfig_path);
                Share.fm.set_mm_status("1");
            } else {
                //to do 设置启动false;
                //key.SetValue("is_open_with_mm", "0");
                DbInihelper.SetIniData(Share.iniconfig_action, "is_open_with_mm", "0", Share.iniconfig_path);
                Share.fm.set_mm_status("0");
            }
            //key.Close();
            //rkey.Close();
        }

        private void Common_setting_Load(object sender, EventArgs e) {
            if (Share.fm.is_open_with_mm == "1") uiCheckBox1.Checked = true;
            if (Share.fm.is_hide_behind == "1") uiCheckBox2.Checked = true;
            uiTextBox1.Text = Share.fm.db_path;
        }
        //更换db位置
        private void uiButton1_Click(object sender, EventArgs e) {
            string dir = "";
            if (DirEx.SelectDirEx("选择数据库保存的位置", ref dir)) {
                string new_db_path = Path.Combine(dir, "MstscManager.db");
                string old_path = Share.fm.db_path;
                new_db_path = new_db_path.Replace(System.Environment.CurrentDirectory + "\\", "");
                //File.Move(old_path, new_db_path);
                File.Copy(old_path, new_db_path);
                //common_tools.RunApp2("explorer.exe",  " "+ dir);
                //common_tools.RunApp2("explorer.exe", " " + Path.GetDirectoryName(old_path));
                //RegistryKey rkey = Registry.CurrentUser;
                //RegistryKey key = rkey.CreateSubKey(@"SOFTWARE\MstscManager");
                //key = rkey.OpenSubKey(@"SOFTWARE\MstscManager", true);
                //key.SetValue("db_path", new_db_path);
                //key.SetValue("old_db_path", old_path);
                //key.Close();rkey.Close();
                DbInihelper.SetIniData(Share.iniconfig_action, "db_path", new_db_path, Share.iniconfig_path);
                DbInihelper.SetIniData(Share.iniconfig_action, "old_db_path", old_path, Share.iniconfig_path);
                Share.fm.set_db_path(new_db_path);
                //UIMessageDialog.ShowMessageDialog("请手动关闭【MSTSC管理器】后，手动将【MstscManager.db】文件移动到你选择的文件夹！", "提示",false,Style,false);
                ShowSuccessTip("为了软件了稳定，请重新启动！");
                Thread.Sleep(1000);
                System.Environment.Exit(0);
            }
        }

        private void uiCheckBox2_CheckedChanged(object sender, EventArgs e) {
            if ((sender as UICheckBox).Checked) {
                DbInihelper.SetIniData(Share.iniconfig_action, "is_hide_behind", "1", Share.iniconfig_path);
                Share.fm.set_hide_behind("1");
            } else {
                DbInihelper.SetIniData(Share.iniconfig_action, "is_hide_behind", "0", Share.iniconfig_path);
                Share.fm.set_hide_behind("0");
            }
        }
    }
}
