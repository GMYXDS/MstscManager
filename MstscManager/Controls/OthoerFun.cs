using MstscManager.Forms;
using MstscManager.Utils;
using Sunny.UI;
namespace MstscManager.Controls {
    public partial class OthoerFun :  UIUserControl{
        public OthoerFun() {
            InitializeComponent();
        }

        //用户管理
        private void uiButton1_Click(object sender, EventArgs e) {
            //to do 密码验证
            Share.fm.auto_close_oth_func();
            var user = new user();
            user.Show();
        }

        //一键导入本机mstsc
        private void uiButton5_Click(object sender, EventArgs e) {
            Share.fm.auto_close_oth_func();
            Share.fm.simple_import();
        }

        private void uiButton2_Click(object sender, EventArgs e) {
            Share.fm.auto_close_oth_func();
            Share.fm.ping_now_items();
        }
    }
}
