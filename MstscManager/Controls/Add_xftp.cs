using MstscManager.Forms;
using Newtonsoft.Json.Linq;
using Sunny.UI;

namespace MstscManager.Controls {
    public partial class Add_xftp : UIPage {
        public Add_xftp() {
            InitializeComponent();
        }

        private void uiRadioButton1_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(22);
        }

        private void uiRadioButton2_Click(object sender, EventArgs e) {
            Add_server top_form = this.Parent.Parent.Parent.Parent as Add_server;
            top_form.set_port(21);
        }
        public Dictionary<string, string> get_config() {
            Dictionary<string, string> config = new Dictionary<string, string>();
            if (uiRadioButton1.Checked) config.Add("sec_connect_mode", "sftp");
            if (uiRadioButton2.Checked) config.Add("sec_connect_mode", "ftp");
            return config;
        }
        public void set_config(JObject csobj) {
            uiRadioButton1.Checked = false;
            uiRadioButton1.Checked = csobj["sec_connect_mode"].ToString() == "sftp" ? true : false;
            uiRadioButton2.Checked = csobj["sec_connect_mode"].ToString() == "ftp" ? true : false;
        }

        private void Add_xftp_ReceiveParams(object sender, UIPageParamsArgs e) {
            JObject csobj = e.Value as JObject;
            set_config(csobj);
        }
    }
}
