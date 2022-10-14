using MstscManager.Forms;
using MstscManager.Utils;

namespace MstscManager.Controls {
    public partial class OthoerFun : UserControl {
        public OthoerFun() {
            InitializeComponent();
        }

        //用户管理
        private void uiButton1_Click(object sender, EventArgs e) {
            //to do 密码验证
            var user = new user();
            user.Show();
        }

        //一键导入本机mstsc
        private void uiButton5_Click(object sender, EventArgs e) {
            Share.fm.simple_import();
        }
    }
}
