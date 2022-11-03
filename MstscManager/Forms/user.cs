using Sunny.UI;
using MstscManager.Utils;
using Microsoft.Data.Sqlite;

namespace MstscManager.Forms {
    public partial class user : UIForm {
        public user() {
            InitializeComponent();
        }

        //添加
        private void uiButton1_Click(object sender, EventArgs e) {
            string user_name = uiTextBox1.Text;
            string user_pass = uiTextBox2.Text;
            string mark_text = uiTextBox3.Text;
            if (user_name == "") { ShowWarningTip("<用户名>是必须的！"); return; }
            if (user_pass == "") { ShowWarningTip("<密码>是必须的！"); return; }
            DbSqlHelper.ExecuteNonQuery2("insert into User_setting (user_name,user_pass,mark_text) values (?,?,?)", user_name, user_pass, mark_text);
            //ShowInfoNotifier("数据添加成功！");
            ShowSuccessTip("数据添加成功");
            //刷新界面？
            //uiDataGridView1.Rows.Add(user_name, user_pass, mark_text);
            update_table();
        }

        //删除
        private void uiButton2_Click(object sender, EventArgs e) {
            if (ShowAskDialog("确定要删除当前选中的账号吗？删除之后，如果有关联的服务器将失效！", false)) {
                string id = uiDataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                //Console.WriteLine(id);
                DbSqlHelper.ExecuteNonQuery2("delete from User_setting where id = ?", id);
                uiDataGridView1.Rows.Remove(uiDataGridView1.SelectedRows[0]);
                ShowSuccessTip("数据删除成功");
            }
        }

        private void user_Load(object sender, EventArgs e) {
            string value = "";
            if (this.InputPasswordDialog(ref value, true, "请输入管理员密码", true)) {
                //ShowInfoDialog(value);
                if (common_tools.md5(value.ToString()) == Share.fm.password) {
                    real_init();
                } else {
                    ShowErrorTip("密码错误！");
                    this.Close();
                }
            } else {
                this.Close();
            }
        }
        private void real_init() {

            //数据初始化
            UIDataGridView view = uiDataGridView1;
            //base.Init();

            view.AutoGenerateColumns = false;
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            view.AllowUserToResizeRows = false;
            view.AllowUserToResizeColumns = true;
            view.AllowUserToAddRows = false;
            view.AllowUserToDeleteRows = false;
            view.CellBorderStyle = DataGridViewCellBorderStyle.None;
            view.MultiSelect = false;
            view.RowHeadersVisible = false;
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            update_table();
        }
        private void update_table() {
            UIDataGridView view = uiDataGridView1;
            view.Rows.Clear();
            SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from User_setting");
            while (reader.Read()) {
                view.Rows.Add(reader["user_name"], reader["user_pass"], reader["mark_text"], reader["id"]);
            }
            reader.Close();
        }

        private void uiButton3_Click(object sender, EventArgs e) {
            Console.WriteLine(uiDataGridView1.SelectedRows.Count);
            string id = uiDataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string user_name = uiTextBox1.Text;
            string user_pass = uiTextBox2.Text;
            string mark_text = uiTextBox3.Text;
            if (user_name == "") { ShowWarningTip("<用户名>是必须的！"); return; }
            if (user_pass == "") { ShowWarningTip("<密码>是必须的！"); return; }
            DbSqlHelper.ExecuteNonQuery2("update User_setting set user_name = ?,user_pass = ?,mark_text = ? where id = ? ", user_name, user_pass, mark_text,id);
            ShowSuccessTip("数据更新成功");
            update_table();
        }

        private void uiDataGridView1_MouseClick(object sender, MouseEventArgs e) {
            //Console.WriteLine(uiDataGridView1.SelectedRows[0].Cells[3].Value.ToString());
            uiTextBox1.Text = uiDataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            uiTextBox2.Text = uiDataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            uiTextBox3.Text = uiDataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void uiButton4_Click(object sender, EventArgs e) {
            uiTextBox1.Text = "";
            uiTextBox2.Text = "";
            uiTextBox3.Text = ""; 
        }
    }
}
