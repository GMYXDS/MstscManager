using Sunny.UI;
using MstscManager.Utils;

namespace MstscManager.Controls {
    public partial class About : UIPage {
        string now_version = "1.1";
        string update_url = "";
        public About() {
            InitializeComponent();
            uiLine1.Text = "当前版本 v" + now_version;
            try {
                check_update();
            } catch (Exception) {  }
        }

        public async void check_update() {
            //to do 请求服务器
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var url = "https://gmyxds.fun/soft/mstscmanager/update.html";
            var result = await client.GetStringAsync(url);
            //Console.WriteLine(result);
            string [] arr = result.Split("-");
            string new_version = arr[0];
            if (Convert.ToDouble(new_version) > Convert.ToDouble(now_version)) {
                uiButton1.ShowTips = true;
                update_url = arr[1];
            }
        }

        private void uiButton1_Click(object sender, EventArgs e) {
            if (update_url == "") {
                ShowSuccessTip("暂无更新！");
                return;
            } else {
                ShowSuccessTip("正在打开更新下载页面！");
                common_tools.RunApp3("cmd.exe", "/C start " + update_url);
            }
        }
    }
}
