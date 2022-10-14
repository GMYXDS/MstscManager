using Newtonsoft.Json.Linq;
using Sunny.UI;

namespace MstscManager.Controls {
    public partial class Add_vnc : UIPage {
        public Add_vnc() {
            InitializeComponent();
        }
        public Dictionary<string, string> get_config() {
            Dictionary<string, string> config = new Dictionary<string, string>();
            if (uiRadioButton1.Checked) config.Add("sec_connect_mode", "tightvnc");
            if (uiRadioButton2.Checked) config.Add("sec_connect_mode", "realvnc");
            if (uiRadioButton3.Checked) config.Add("sec_connect_mode", "ultravnc");

            if (uiCheckBox1.Checked) config.Add("fullscreen", "1"); else config.Add("fullscreen", "0");
            if (uiCheckBox2.Checked) config.Add("autoreconnect", "1"); else config.Add("autoreconnect", "0");
            if (uiCheckBox3.Checked) config.Add("viewonly", "1"); else config.Add("viewonly", "0");

            return config;
        }
        public void set_config(JObject csobj) {
            string sec_connect_mode = csobj["sec_connect_mode"].ToString();
            uiRadioButton3.Checked = false;
            if (sec_connect_mode == "tightvnc") {
                uiRadioButton1.Checked = true;
            } else if (sec_connect_mode == "realvnc") {
                uiRadioButton2.Checked = true;
            } else if (sec_connect_mode == "ultravnc") {
                uiRadioButton3.Checked = true;
            }

            uiCheckBox1.Checked = csobj["fullscreen"].ToString() == "1" ? true : false;
            uiCheckBox2.Checked = csobj["autoreconnect"].ToString() == "1" ? true : false;
            uiCheckBox3.Checked = csobj["viewonly"].ToString() == "1" ? true : false;
        }

        private void Add_vnc_ReceiveParams(object sender, UIPageParamsArgs e) {
            JObject csobj = e.Value as JObject;
            set_config(csobj);
        }
    }
}
