using Newtonsoft.Json.Linq;
using Sunny.UI;

namespace MstscManager.Controls {
    public partial class Add_radmin : UIPage {
        public Add_radmin() {
            InitializeComponent();
        }
        public Dictionary<string, string> get_config() {
            Dictionary<string, string> config = new Dictionary<string, string>();
            string sec_connect_mode = (string)(uiComboBox1.SelectedItem == null ? uiComboBox1.Text : uiComboBox1.SelectedItem);
            if (sec_connect_mode == "完全控制") config.Add("sec_connect_mode", "完全控制");
            else if (sec_connect_mode == "仅限查看") config.Add("sec_connect_mode", "仅限查看");
            else if (sec_connect_mode == "Telnet") config.Add("sec_connect_mode", "telnet");
            else if (sec_connect_mode == "文件传送") config.Add("sec_connect_mode", "文件传送");
            else if (sec_connect_mode == "关机") config.Add("sec_connect_mode", "关机");
            else if (sec_connect_mode == "聊天") config.Add("sec_connect_mode", "聊天");
            else if (sec_connect_mode == "语音聊天") config.Add("sec_connect_mode", "语音聊天");
            else if (sec_connect_mode == "传送讯息") config.Add("sec_connect_mode", "传送讯息");
            else config.Add("sec_connect_mode", "完全控制");

            if (uiCheckBox1.Checked) config.Add("encrypt", "1"); else config.Add("encrypt", "0");
            if (uiCheckBox2.Checked) config.Add("fullscreen", "1"); else config.Add("fullscreen", "0");
            if (uiCheckBox3.Checked) config.Add("nofullkbcontrol", "1"); else config.Add("nofullkbcontrol", "0");

            string color_mode = (string)(uiComboBox2.SelectedItem == null ? uiComboBox2.Text : uiComboBox2.SelectedItem);
            config.Add("color_mode", color_mode);

            string updates = uiTextBox1.Text == "" ? "30" : uiTextBox1.Text;
            config.Add("updates", updates);

            return config;
        }
        public void set_config(JObject csobj) {
            uiComboBox1.Text = csobj["sec_connect_mode"].ToString();
            uiCheckBox1.Checked = csobj["encrypt"].ToString() == "1" ? true : false;
            uiCheckBox2.Checked = csobj["fullscreen"].ToString() == "1" ? true : false;
            uiCheckBox3.Checked = csobj["nofullkbcontrol"].ToString() == "1" ? true : false;
            uiComboBox2.Text = csobj["color_mode"].ToString();
            uiTextBox1.Text = csobj["updates"].ToString();
        }

        private void Add_radmin_ReceiveParams(object sender, UIPageParamsArgs e) {
            JObject csobj = e.Value as JObject;
            set_config(csobj);
        }
    }
}

