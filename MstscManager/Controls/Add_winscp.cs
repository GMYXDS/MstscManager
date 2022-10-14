using MstscManager.Forms;
using Newtonsoft.Json.Linq;
using Sunny.UI;

namespace MstscManager.Controls {
    public partial class Add_winscp : UIPage {
        public Add_winscp() {
            InitializeComponent();
        }

        private void uiRadioButton1_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(22);
        }

        private void uiRadioButton2_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(22);
        }

        private void uiRadioButton3_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(21);
        }

        private void uiRadioButton4_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(0);
        }

        private void uiRadioButton5_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(0);
        }

        public Dictionary<string, string> get_config() {
            Dictionary<string, string> config = new Dictionary<string, string>();
            if (uiRadioButton1.Checked) config.Add("sec_connect_mode", "sftp");
            if (uiRadioButton2.Checked) config.Add("sec_connect_mode", "scp");
            if (uiRadioButton3.Checked) config.Add("sec_connect_mode", "ftp");
            if (uiRadioButton4.Checked) config.Add("sec_connect_mode", "http");
            if (uiRadioButton5.Checked) config.Add("sec_connect_mode", "https");

            config.Add("dav_address", uiTextBox1.Text);

            return config;
        }
        public void set_config(JObject csobj) {
            uiRadioButton1.Checked = false;
            uiRadioButton1.Checked = csobj["sec_connect_mode"].ToString() == "sftp" ? true : false;
            uiRadioButton2.Checked = csobj["sec_connect_mode"].ToString() == "scp" ? true : false;
            uiRadioButton3.Checked = csobj["sec_connect_mode"].ToString() == "ftp" ? true : false;
            uiRadioButton4.Checked = csobj["sec_connect_mode"].ToString() == "http" ? true : false;
            uiRadioButton5.Checked = csobj["sec_connect_mode"].ToString() == "https" ? true : false;
            uiTextBox1.Text = csobj["dav_address"].ToString();
        }

        private void Add_winscp_ReceiveParams(object sender, UIPageParamsArgs e) {
            JObject csobj = e.Value as JObject;
            set_config(csobj);
        }
    }
}
