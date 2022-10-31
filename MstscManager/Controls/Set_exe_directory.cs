using Microsoft.Data.Sqlite;
using MstscManager.Utils;
using Sunny.UI;

namespace MstscManager.Controls {
    public partial class Set_exe_directory : UIPage {
        public Set_exe_directory() {
            InitializeComponent();
            init();
        }
        private void init() {
            SqliteDataReader reader = DbSqlHelper.ExecuteReader("select * from Commom_setting");
            while (reader.Read()) {
                string key = reader["key"].ToString();
                if(key == "putty_exe_path") uiTextBox1.Text = reader["val"].ToString();
                else if(key == "xshell_exe_path") uiTextBox2.Text = reader["val"].ToString();
                else if(key == "xftp_exe_path") uiTextBox3.Text = reader["val"].ToString();
                else if(key == "radmin_exe_path") uiTextBox4.Text = reader["val"].ToString();
                else if(key == "vnc_exe_path") uiTextBox5.Text = reader["val"].ToString();
                else if(key == "winscp_exe_path") uiTextBox6.Text = reader["val"].ToString();
                else if(key == "securecrt_exe_path") uiTextBox7.Text = reader["val"].ToString();
                else if(key == "mobaxterm_exe_path") uiTextBox8.Text = reader["val"].ToString();
                else if(key == "todesk_exe_path") uiTextBox9.Text = reader["val"].ToString();
            }
            reader.Close();
        }
        private string show_dialog() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择对应的exe文件";
            //ofd.Multiselect = true;
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            ofd.Filter = "可执行文件|*.exe|所有文件|*.*";
            ofd.ShowDialog();
            return ofd.FileName;
        }
        private void set_or_update_path(string key,UITextBox button) {
            string path = show_dialog();
            if (path == "") return;
            path = path.Replace(System.Environment.CurrentDirectory+"\\", "");
            //Console.WriteLine(path);
            if (button.Text != "") {//update
                DbSqlHelper.ExecuteNonQuery("update Commom_setting set val = ? where key = ?", path, key);
            } else {//insert
                DbSqlHelper.ExecuteNonQuery("INSERT INTO Commom_setting (key,val) VALUES (?,?);", key, path);
            }
            button.Text = path;
        }
        private void uiButton1_Click(object sender, EventArgs e) {
            set_or_update_path("putty_exe_path", uiTextBox1);
        }

        private void uiButton2_Click(object sender, EventArgs e) {
            set_or_update_path("xshell_exe_path", uiTextBox2);
        }

        private void uiButton3_Click(object sender, EventArgs e) {
            set_or_update_path("xftp_exe_path", uiTextBox3);
        }

        private void uiButton4_Click(object sender, EventArgs e) {
            set_or_update_path("radmin_exe_path", uiTextBox4);
        }

        private void uiButton5_Click(object sender, EventArgs e) {
            set_or_update_path("vnc_exe_path", uiTextBox5);
        }

        private void uiButton6_Click(object sender, EventArgs e) {
            set_or_update_path("winscp_exe_path", uiTextBox6);
        }

        private void uiButton7_Click(object sender, EventArgs e) {
            set_or_update_path("securecrt_exe_path", uiTextBox7);
        }

        private void uiButton8_Click(object sender, EventArgs e) {
            set_or_update_path("mobaxterm_exe_path", uiTextBox8);
        }

        private void uiButton9_Click(object sender, EventArgs e) {
            set_or_update_path("todesk_exe_path", uiTextBox9);
        }
    }
}
