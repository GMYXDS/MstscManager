using Newtonsoft.Json.Linq;
using Sunny.UI;
using System.Text.RegularExpressions;

namespace MstscManager.Controls {
    public partial class add_mstsc : UIPage {
        public add_mstsc() {
            InitializeComponent();
        }
        public Dictionary<string, string> get_config() {
            Dictionary<string, string> config = new Dictionary<string, string>();
            string screen_mode = (string)(uiComboBox1.SelectedItem == null ? uiComboBox1.Text : uiComboBox1.SelectedItem);
            config.Add("screen_mode_name", screen_mode);
            if (screen_mode.Trim() == "全屏") {
                config.Add("screen_mode", "2");
                config.Add("screen_width", "1024");
                config.Add("screen_height", "768");
            } else {
                config.Add("screen_mode", "1");
                string[] arr = screen_mode.Split(new char[] {' ',}, StringSplitOptions.RemoveEmptyEntries);
                config.Add("screen_width", arr[0]);
                config.Add("screen_height", arr[2]);
            }

            if (uiCheckBox3.Checked) config.Add("use_multimon", "1"); else config.Add("use_multimon", "0");

            string session_bpp = (string)(uiComboBox3.SelectedItem == null ? uiComboBox3.Text : uiComboBox3.SelectedItem);
            Regex regex = new Regex(@"(\d+)");
            Match match = regex.Match(session_bpp);
            config.Add("session_bpp", match.Groups[1].Value.ToString());

            if (uiCheckBox4.Checked) config.Add("displayconnectionbar", "1"); else config.Add("displayconnectionbar", "0");

            string audiomode = (string)(uiComboBox4.SelectedItem == null ? uiComboBox4.Text : uiComboBox4.SelectedItem);
            config.Add("audiomode_name", audiomode);
            if (audiomode== "在远程计算机上播放") config.Add("audiomode", "1");
            else if(audiomode== "不要播放") config.Add("audiomode", "2");
            else if(audiomode == "在此计算机上播放") config.Add("audiomode", "0");
            else config.Add("audiomode", "0");

            string audiocapturemode = (string)(uiComboBox5.SelectedItem == null ? uiComboBox5.Text : uiComboBox5.SelectedItem);
            config.Add("audiocapturemode_name", audiocapturemode);
            if (audiocapturemode == "从此计算机进行录制") config.Add("audiocapturemode", "1");
            else if (audiocapturemode == "不录制") config.Add("audiocapturemode", "0");
            else config.Add("audiocapturemode", "0");

            string keyboardhook = (string)(uiComboBox6.SelectedItem == null ? uiComboBox6.Text : uiComboBox6.SelectedItem);
            config.Add("keyboardhook_name", keyboardhook);
            if (keyboardhook == "在这台计算机上") config.Add("keyboardhook", "1");
            else if (keyboardhook == "在远程计算机上") config.Add("keyboardhook", "3");
            else if (keyboardhook == "仅在全屏显示时") config.Add("keyboardhook", "2");
            else config.Add("keyboardhook", "2");

            if (uiCheckBox5.Checked) config.Add("autoreconnection", "1"); else config.Add("autoreconnection", "0");
            if (uiCheckBox8.Checked) config.Add("disable_wallpaper", "0"); else config.Add("disable_wallpaper", "1");
            if (uiCheckBox7.Checked) config.Add("compression", "1"); else config.Add("compression", "0");
            if (uiCheckBox1.Checked) config.Add("p_prompt", "1"); else config.Add("p_prompt", "0");

            if (uiCheckBox6.Checked) config.Add("redirectclipboard", "1"); else config.Add("redirectclipboard", "0");
            if (uiCheckBox9.Checked) config.Add("redirectprinters", "1"); else config.Add("redirectprinters", "0");
            if (uiCheckBox2.Checked) config.Add("redirectsmartcards", "1"); else config.Add("redirectsmartcards", "0");
            if (uiCheckBox10.Checked) config.Add("redirectcomports", "1"); else config.Add("redirectcomports", "0");
            if (uiCheckBox11.Checked) config.Add("redirectposdevices", "1"); else config.Add("redirectposdevices", "0");
            if (uiCheckBox12.Checked) config.Add("camerastoredirect", "*"); else config.Add("camerastoredirect", " ");
            if (uiCheckBox13.Checked) config.Add("drivestoredirect", "*"); else config.Add("drivestoredirect", " ");

            if(uiRadioButton2.Checked) config.Add("p_admin_mode", "1"); else config.Add("p_admin_mode", "0");

            return config;
        }
        public void set_config(JObject csobj) {
            uiComboBox1.Text  = csobj["screen_mode_name"].ToString();
            uiCheckBox3.Checked = csobj["use_multimon"].ToString()=="1"?true:false;

            string session_bpp = csobj["session_bpp"].ToString();
            if(session_bpp == "15") { uiComboBox3.Text = "增强色(15位)"; }
            else if(session_bpp == "16") { uiComboBox3.Text = "增强色(16位)"; } 
            else if(session_bpp == "24") { uiComboBox3.Text = "真彩色(24位)"; }
            else if (session_bpp == "32") { uiComboBox3.Text = "最高质量(32位)"; }

            uiCheckBox4.Checked = csobj["displayconnectionbar"].ToString() == "1" ? true : false;
            uiComboBox4.Text = csobj["audiomode_name"].ToString();
            uiComboBox5.Text = csobj["audiocapturemode_name"].ToString();
            uiComboBox6.Text = csobj["keyboardhook_name"].ToString();

            uiCheckBox5.Checked = csobj["autoreconnection"].ToString() == "1" ? true : false;
            uiCheckBox8.Checked = csobj["disable_wallpaper"].ToString() == "0" ? true : false;
            uiCheckBox7.Checked = csobj["compression"].ToString() == "1" ? true : false;
            uiCheckBox1.Checked = csobj["p_prompt"].ToString() == "1" ? true : false;

            uiCheckBox6.Checked = csobj["redirectclipboard"].ToString() == "1" ? true : false;
            uiCheckBox9.Checked = csobj["redirectprinters"].ToString() == "1" ? true : false;
            uiCheckBox2.Checked = csobj["redirectsmartcards"].ToString() == "1" ? true : false;
            uiCheckBox10.Checked = csobj["redirectcomports"].ToString() == "1" ? true : false;
            uiCheckBox11.Checked = csobj["redirectposdevices"].ToString() == "1" ? true : false;
            uiCheckBox12.Checked = csobj["camerastoredirect"].ToString() == "*" ? true : false;
            uiCheckBox13.Checked = csobj["drivestoredirect"].ToString() == "*" ? true : false;

            uiRadioButton2.Checked = csobj["p_admin_mode"].ToString() == "1" ? true : false;
            uiRadioButton1.Checked = csobj["p_admin_mode"].ToString() == "0" ? true : false;

        }

        private void add_mstsc_ReceiveParams(object sender, UIPageParamsArgs e) {
            JObject csobj = e.Value as JObject;
            set_config(csobj);
        }
    }
}
