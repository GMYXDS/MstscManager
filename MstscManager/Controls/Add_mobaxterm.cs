using MstscManager.Forms;
using MstscManager.Utils;
using Newtonsoft.Json.Linq;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MstscManager.Controls {
    public partial class Add_mobaxterm : UIPage {
        public Add_mobaxterm() {
            InitializeComponent();
        }

        private void uiRadioButton1_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(22);
        }

        private void uiRadioButton2_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(0);
        }
        public Dictionary<string, string> get_config() {
            Dictionary<string, string> config = new Dictionary<string, string>();
            if (uiRadioButton1.Checked) config.Add("sec_connect_mode", "ssh");
            if (uiRadioButton2.Checked) config.Add("sec_connect_mode", "telnet");
            if (uiCheckBox1.Checked) config.Add("is_ssh_rsa", "1"); else config.Add("is_ssh_rsa", "0");
            config.Add("ssh_rsa_path", uiTextBox1.Text);
            return config;
        }
        public void set_config(JObject csobj) {
            uiRadioButton1.Checked = false;
            uiRadioButton1.Checked = csobj["sec_connect_mode"].ToString() == "ssh" ? true : false;
            uiRadioButton2.Checked = csobj["sec_connect_mode"].ToString() == "telnet" ? true : false;
            uiRadioButton2.Checked = csobj["is_ssh_rsa"].ToString() == "1" ? true : false;
            if (csobj["is_ssh_rsa"].ToString() == "1") {
                uiTextBox1.Enabled = true;
                uiButton1.Enabled = true;
                uiCheckBox1.Checked = true;
                uiRadioButton1.Checked = true;
                uiRadioButton2.Checked = false;
            }
            uiTextBox1.Text = csobj["ssh_rsa_path"].ToString();
        }
        private void Add_mobaxterm_ReceiveParams(object sender, UIPageParamsArgs e) {
            JObject csobj = e.Value as JObject;
            set_config(csobj);
        }
        private void uiLinkLabel2_Click(object sender, EventArgs e) {
            common_tools.RunApp3("cmd.exe", "/C start " + uiLinkLabel2.Text.ToString());
        }
        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e) {
            if ((sender as UICheckBox).Checked) {
                uiRadioButton1.Checked = true;
                uiRadioButton2.Checked = false;
                uiTextBox1.Enabled = true;
                uiButton1.Enabled = true;
            } else {
                uiTextBox1.Text = "";
                uiTextBox1.Enabled = false;
                uiButton1.Enabled = false;
            }
        }
        private string show_dialog() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择对应的rsa.ppk文件";
            //ofd.Multiselect = true;
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            ofd.Filter = "所有文件|*.*";
            ofd.ShowDialog();
            return ofd.FileName;
        }
        private void uiButton1_Click(object sender, EventArgs e) {
            string path = show_dialog();
            if (path == "") return;
            path = path.Replace(System.Environment.CurrentDirectory + "\\", "");
            //Console.WriteLine(path);
            uiTextBox1.Text = path;
        }
    }
}
