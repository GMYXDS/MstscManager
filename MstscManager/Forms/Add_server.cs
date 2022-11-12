using Sunny.UI;
using MstscManager.Controls;
using Microsoft.Data.Sqlite;
using MstscManager.Utils;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;

namespace MstscManager.Forms {
    public partial class Add_server : UIForm {

        DataTable dt = new DataTable();
        string select_user_id = "-1";
        bool is_update = false;
        string target_id =  "-1";
        string update_pass = "";

        public Add_server() {
            InitializeComponent();
            init();
            only_add_server();
        }
        public void only_add_server() {
            uiSymbolButton1.Visible = false;
            uiSymbolButton3.Visible = false;
            uiTextBox4.Width = 199;
            uiTextBox5.Width = 199;
        }
        public Add_server(string connect_setting, string target_id) {
            InitializeComponent();
            this.is_update = true;
            this.target_id = target_id;

            JObject csobj = JsonConvert.DeserializeObject<JObject>(connect_setting);

            init();
            uiHeaderButton1.Selected = false;
            string connect_type = csobj["connect_type"].ToString();

            //base 初始化
            uiTextBox1.Text = csobj["server_name"].ToString();
            uiComboBox2.Text = csobj["group_name"].ToString();
            uiTextBox3.Text = csobj["ip"].ToString();
            uiTextBox2.Text = csobj["port"].ToString();
            string user_id = csobj["user_id"].ToString().Trim();
            if (user_id != "-1") {//有账户体系
                select_user_id = user_id;
                uiComboDataGridView1.Text = "下拉选择用户";
                uiComboDataGridView1.BringToFront();
                //uiTextBox4.Watermark = "下拉选择用户";
                uiTextBox5.Watermark = "使用账户体系:id:"+ user_id.ToString();
                uiTextBox5.Enabled = false;
                togole_index += 1;
            } else {
                uiTextBox4.Text = csobj["user_name"].ToString();
                //uiTextBox5.Text = csobj["user_pass"].ToString();
                update_pass = csobj["user_pass"].ToString();
                if (update_pass != "")uiTextBox5.Text = "*********";
            }
            string end_date = csobj["end_date"].ToString();
            if (end_date != "") {
                uiDatePicker1.Text = end_date;
                uiCheckBox2.Checked = false;
            }
            uiTextBox6.Text = csobj["mark_text"].ToString();
            string connect_string = csobj["connect_string"].ToString();
            if (connect_string.Trim() != "") {
                uiCheckBox1.Checked = true;
                uiTextBox7.Text = connect_string;
                uiTabControl1.Enabled = false;
            }

            //个性配置初始化
            if (connect_type.IndexOf("RDP") != -1) {
                SelectPage(1001);
                uiHeaderButton1.Selected = true;
                SendParamToPage(1001, csobj);
            } else if (connect_type.IndexOf("Putty") != -1) {
                SelectPage(1002);
                uiHeaderButton3.Selected = true;
                SendParamToPage(1002, csobj);
            } else if (connect_type.IndexOf("Xshell") != -1) {
                SelectPage(1003);
                uiHeaderButton2.Selected = true;
                //(target as Add_xshell).set_config(csobj);
                SendParamToPage(1003, csobj);
            } else if (connect_type.IndexOf("Xftp") != -1) {
                SelectPage(1004);
                uiHeaderButton4.Selected = true;
                SendParamToPage(1004, csobj);
            } else if (connect_type.IndexOf("Radmin") != -1) {
                SelectPage(1005);
                uiHeaderButton5.Selected = true;
                SendParamToPage(1005, csobj);
            } else if (connect_type.IndexOf("VNC") != -1) {
                SelectPage(1006);
                uiHeaderButton6.Selected = true;
                SendParamToPage(1006, csobj);
            } else if (connect_type.IndexOf("Winscp") != -1) {
                SelectPage(1007);
                uiHeaderButton7.Selected = true;
                SendParamToPage(1007, csobj);
            } else if (connect_type.IndexOf("SecureCrt") != -1) {
                SelectPage(1008);
                uiHeaderButton8.Selected = true;
                SendParamToPage(1008, csobj);
            } else if (connect_type.IndexOf("Mobaxterm") != -1) {
                SelectPage(1009);
                uiHeaderButton9.Selected = true;
                SendParamToPage(1009, csobj);
            } else if (connect_type.IndexOf("Todesk") != -1) {
                SelectPage(10010);
                uiHeaderButton10.Selected = true;
                SendParamToPage(10010, csobj);
            }
        }
        public void init() {
            //初始化页面额外参数框
            int pageIndex = 1000;
            AddPage(new add_mstsc(), ++pageIndex);
            AddPage(new Add_puty(), ++pageIndex);
            AddPage(new Add_xshell(), ++pageIndex);
            AddPage(new Add_xftp(), ++pageIndex);
            AddPage(new Add_radmin(), ++pageIndex);
            AddPage(new Add_vnc(), ++pageIndex);
            AddPage(new Add_winscp(), ++pageIndex);
            AddPage(new Add_securecrt(), ++pageIndex);
            AddPage(new Add_mobaxterm(), ++pageIndex);
            AddPage(new Add_todesk(), ++pageIndex);
            uiHeaderButton1.Selected = true;
            uiDatePicker1.Text = "";

            //初始化分组信息
            uiComboBox2.Items.Add("全部分类");
            SqliteDataReader reader = DbSqlHelper.ExecuteReader("select * from Group_setting");
            while (reader.Read()) {
                string group_name = (string)reader["group_name"];
                uiComboBox2.Items.Add(group_name);
            }
            //uiComboBox2.Text = "全部分类";
            uiComboBox2.Text = Share.now_group_name;
            reader.Close();

            //初始化用户下拉选择框
            init_user_select();
        }
        public void init_user_select() {
            dt.Columns.Add("Column1", typeof(string));
            dt.Columns.Add("Column2", typeof(string));
            dt.Columns.Add("Column3", typeof(string));

            SqliteDataReader reader = DbSqlHelper.ExecuteReader("select * from User_setting");
            while (reader.Read()) {
                dt.Rows.Add(reader["user_name"], reader["mark_text"], reader["id"]);
            }
            reader.Close();

            uiComboDataGridView1.DataGridView.Init();
            uiComboDataGridView1.ItemSize = new System.Drawing.Size(360, 240);
            uiComboDataGridView1.DataGridView.AddColumn("用户名", "Column1");
            uiComboDataGridView1.DataGridView.AddColumn("备注", "Column2");
            uiComboDataGridView1.DataGridView.AddColumn("id", "Column3");
            //uiComboDataGridView1.DataGridView.Columns[2].Visible = false;
            uiComboDataGridView1.DataGridView.ReadOnly = true;
            uiComboDataGridView1.SelectIndexChange += UiComboDataGridView1_SelectIndexChange;
            uiComboDataGridView1.ShowFilter = true;
            uiComboDataGridView1.DataGridView.DataSource = dt;
            //uiComboDataGridView1.FilterColumnName = "Column1"; //不设置则全部列过滤
        }
        public void set_port(int port) {
            if (port == 0) {
                this.uiTextBox2.Text = "";
                return;
            }
            this.uiTextBox2.Text = Convert.ToString(port);
        }
        private void uiSymbolButton2_Click(object sender, EventArgs e) {
            UIMessageDialog.ShowMessageDialog(
                "自定义使用规则：\n" +
                "使用自定义可以自己定义连接工具后面的命令行参数，但是上面的额外信息配置框将会全部失效。\n" +
                "同时你可以引入设置的基本信息作为变量：\n" +
                "用户名:{username}\n" +
                "密码：{password}\n" +
                "ip地址：{ip}\n" +
                "端口：{port}\n" +
                "比如连接ssh的时候，自定义参数可以写成： {username}:{password}@{ip}:{port}\n" +
                "具体的参数需要自己上网查询对应手册使用，不建议萌新使用这个", "提示", false, Style, false, true);
        }
        //rdp
        private void uiHeaderButton1_Click(object sender, EventArgs e) {
            SelectPage(1001);
            set_port(3389);
        }
        //putty
        private void uiHeaderButton3_Click(object sender, EventArgs e) {
            SelectPage(1002);
            set_port(22);
        }
        //xshell
        private void uiHeaderButton2_Click(object sender, EventArgs e) {
            SelectPage(1003);
            set_port(22);
        }
        //xftp
        private void uiHeaderButton4_Click(object sender, EventArgs e) {
            SelectPage(1004);
            set_port(22);
        }
        //radmin
        private void uiHeaderButton5_Click(object sender, EventArgs e) {
            SelectPage(1005);
            set_port(4899);
        }
        //vnc
        private void uiHeaderButton6_Click(object sender, EventArgs e) {
            SelectPage(1006);
            set_port(5900);
        }
        //winscp
        private void uiHeaderButton7_Click(object sender, EventArgs e) {
            SelectPage(1007);
            set_port(22);
        }
        //securecrt
        private void uiHeaderButton8_Click(object sender, EventArgs e) {
            SelectPage(1008);
            set_port(22);
        }
        //mobaxterm
        private void uiHeaderButton9_Click(object sender, EventArgs e) {
            SelectPage(1009);
            set_port(22);
        }
        //todesk
        private void uiHeaderButton10_Click(object sender, EventArgs e) {
            SelectPage(1010);
            set_port(0);
        }

        private void Add_server_Load(object sender, EventArgs e) {
            //add_instance = this;
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e) {
            if ((sender as UICheckBox).Checked) {
                uiTabControl1.Enabled = false;
                uiTextBox7.Enabled = true;
            } else {
                uiTabControl1.Enabled = true;
                uiTextBox7.Enabled = false;
            }
        }
        //保存配置文件 server
        private void uiButton1_Click(object sender, EventArgs e) {
            Dictionary<string, string> config = new Dictionary<string, string>();
            Dictionary<string, string> config_extra = new Dictionary<string, string>();

            //获取base信息
            string server_name = this.uiTextBox1.Text.Trim();
            config.Add("server_name", server_name);
            string group_name = (string)(uiComboBox2.SelectedItem == null ? uiComboBox2.Text : uiComboBox2.SelectedItem);
            config.Add("group_name", group_name);
            config.Add("group_id", "-1");
            config.Add("connect_type", "");
            string connect_type = "";
            string ip = this.uiTextBox3.Text.Trim();
            config.Add("ip", ip);
            string port = this.uiTextBox2.Text.Trim();
            config.Add("port", port);
            config.Add("user_name", this.uiTextBox4.Text);
            if (this.uiTextBox5.Text=="*********") {
                config.Add("user_pass",update_pass);
            } else {
                config.Add("user_pass", this.uiTextBox5.Text);
            }
            if (uiCheckBox2.Checked) config.Add("end_date", "");
            else config.Add("end_date", this.uiDatePicker1.Text);
            config.Add("mark_text", this.uiTextBox6.Text);
            config.Add("user_id", "-1");
            config.Add("connect_setting", "");
            config.Add("connect_string", "");


            //check 参数
            if (server_name == "") { ShowWarningTip("<服务器名称>是必须的！"); return; }
            if (ip == "") { ShowWarningTip("<服务器ip>是必须的！"); return; }
            if (port == "") { ShowWarningTip("<服务器端口>是必须的！"); return; }

            //获取保存类型
            //不同类型保存不同的参数集
            //参数集序列化到数据库
            int index = this.uiTabControl1.SelectedIndex;
            Control target = uiTabControl1.SelectedTab.Controls[0];
            if (index == 0) {
                connect_type = "RDP";
                config_extra = (target as add_mstsc).get_config();
            } else if (index == 1) {
                connect_type = "Putty";
                config_extra = (target as Add_puty).get_config();
            } else if (index == 2) {
                connect_type = "Xshell";
                config_extra = (target as Add_xshell).get_config();
            } else if (index == 3) {
                connect_type = "Xftp";
                config_extra = (target as Add_xftp).get_config();
            } else if (index == 4) {
                connect_type = "Radmin";
                config_extra = (target as Add_radmin).get_config();
            } else if (index == 5) {
                connect_type = "VNC";
                config_extra = (target as Add_vnc).get_config();
            } else if (index == 6) {
                connect_type = "Winscp";
                config_extra = (target as Add_winscp).get_config();
            } else if (index == 7) {
                connect_type = "SecureCrt";
                config_extra = (target as Add_securecrt).get_config();
            } else if(index==8) {
                connect_type = "Mobaxterm";
                config_extra = (target as Add_mobaxterm).get_config();
            } else if(index == 9) {
                connect_type = "Todesk";
            }
            if (uiCheckBox1.Checked) {
                config["connect_string"] = uiTextBox7.Text;
                connect_type += "-自定义";
            } else {
                if (config_extra.ContainsKey("sec_connect_mode")) {
                    connect_type += "-" + config_extra["sec_connect_mode"];
                }
            }

            config["connect_type"] = connect_type;
            config = MergeDictionary(config, config_extra);

            //数据存到数据库
            //获取分组id
            if (group_name != "全部分类") {
                SqliteDataReader reader = DbSqlHelper.ExecuteReader("select * from Group_setting where group_name = ?", group_name);
                int group_id = -1;
                bool flag = reader.Read();
                if (flag) {
                    group_id = Convert.ToInt32(reader["id"]);
                }
                reader.Close();
                config["group_id"] = group_id.ToString();
            }
            //获取user_id
            //Console.WriteLine(select_user_id);
            if (uiComboDataGridView1.Text != "" && uiTextBox5.Enabled == false) {
                if (select_user_id != "-1" && select_user_id != "") config["user_id"] = select_user_id;
            }

            //输出看看
            //foreach (KeyValuePair<string, string> item in config) {
            //    Console.WriteLine(item.Key + " : " + item.Value);
            //}
            //序列化config
            config["connect_setting"] = JsonConvert.SerializeObject(config);
            string notice_msg = "数据添加成功！";
            if (!is_update) {
                //保存数据库
                DbSqlHelper.ExecuteNonQuery2("INSERT INTO Server_setting(server_name,group_id,connect_type,ip,port,user_name,user_pass,end_date,mark_text,user_id,connect_setting,connect_string) VALUES (?,?,?,?,?,?,?,?,?,?,?,?);",
                config["server_name"], config["group_id"], config["connect_type"], config["ip"], config["port"], config["user_name"], config["user_pass"], config["end_date"], config["mark_text"], config["user_id"], config["connect_setting"], config["connect_string"]);
                //ShowInfoNotifier("数据添加成功！");
            } else {
                //更新
                //保存数据库
                DbSqlHelper.ExecuteNonQuery2("update Server_setting set server_name = ?,group_id = ?,connect_type = ?,ip = ?,port = ?,user_name = ?,user_pass = ?,end_date = ?,mark_text = ?,user_id = ?,connect_setting = ?,connect_string = ? where id = ?",
                config["server_name"], config["group_id"], config["connect_type"], config["ip"], config["port"], config["user_name"], config["user_pass"], config["end_date"], config["mark_text"], config["user_id"], config["connect_setting"], config["connect_string"],target_id);
                notice_msg = "数据更新成功！";
            }

            //清除数据？
            //刷新主form
            Share.fm.refresh_server_table();
            UIMessageDialog.ShowMessageDialog(notice_msg, "提示", false, Style, false);
        }

        public Dictionary<string, string> MergeDictionary(Dictionary<string, string> first, Dictionary<string, string> second) {
            if (first == null) first = new Dictionary<string, string>();
            if (second == null) return first;
            foreach (var item in second) {
                if (!first.ContainsKey(item.Key))
                    first.Add(item.Key, item.Value);
            }
            return first;
        }

        private void uiCheckBox2_CheckedChanged(object sender, EventArgs e) {
            if ((sender as UICheckBox).Checked) {
                uiDatePicker1.Text = "";
                uiDatePicker1.Enabled = false;
            } else {
                uiDatePicker1.Enabled = true;
            }
        }
        int togole_index = 0;
        //选择用户
        private void uiSymbolButton1_Click(object sender, EventArgs e) {
            if (togole_index == 0) {
                uiComboDataGridView1.BringToFront();
                uiTextBox4.Text = "";
                uiTextBox5.Text = "";
                uiTextBox5.Enabled = false;
                togole_index += 1;
            } else {
                uiComboDataGridView1.SendToBack();
                uiTextBox5.Enabled = true;
                togole_index -= 1;
                select_user_id = "-1";
            }
        }
        private void UiComboDataGridView1_SelectIndexChange(object sender, int index) {
            uiComboDataGridView1.Text = dt.Rows[index][0].ToString();
            select_user_id = dt.Rows[index][2].ToString();
        }
        private void uiComboDataGridView1_ValueChanged(object sender, object value) {
            uiComboDataGridView1.Text = "";
            if (value != null && value is DataGridViewRow) {
                DataGridViewRow row = (DataGridViewRow)value;
                uiComboDataGridView1.Text = row.Cells[0].Value.ToString();
                select_user_id = row.Cells[2].Value.ToString();
            }
        }
        private void uiComboDataGridView1_SelectIndexChange_1(object sender, int index) {
            uiComboDataGridView1.Text = dt.Rows[index][0].ToString();
            select_user_id = dt.Rows[index][2].ToString();
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e) {
            //是否使用账户体系
            //弹出密码框
            string value = "";
            if (this.InputPasswordDialog(ref value, true, "请输入管理员密码", true)) {
                if (common_tools.md5(value.ToString()) == Share.fm.password) {
                    if (select_user_id.ToString() != "-1") {
                        ShowInfoTip("请到账户管理里面查看该id的密码！");
                        return;
                    } else {
                        uiTextBox5.Text = update_pass;
                    }
                    return;
                } else {
                    ShowErrorTip("密码错误！");
                    return;
                }
            }
        }

    }
}
