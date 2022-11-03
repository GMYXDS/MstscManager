using Sunny.UI;
using MstscManager.Forms;
using MstscManager.Controls;
using MstscManager.Utils;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static System.Environment;
using System.Data.Common;
using System.Text;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace MstscManager {
    public partial class FMain : UIForm {
        //public string db_path = System.Environment.GetFolderPath(SpecialFolder.LocalApplicationData) + "\\MstscManager.db";
        //public string db_path = System.Environment.CurrentDirectory + "\\data\\MstscManager.db";
        public string db_path = @"data\MstscManager.db";
        public string iniconfig_path = Share.iniconfig_path;
        public string iniconfig_action = Share.iniconfig_action;
        public string password = "123456";
        public string db_password = "123456";
        public string is_open_with_mm = "0";
        public string is_hide_behind = "0";
        public string tool_tip_show_once = "0";
        JObject now_csobj = new JObject();
        List<string> connct_types = new List<string> {
            "RDP","RDP-自定义",
            "Putty-ssh","Putty-telnet","Putty-自定义",
            "Xshell-ssh","Xshell-telnet","Xshell-sftp","Xshell-自定义",
            "Xftp-sftp","Xshell-ftp","Xftp-自定义",
            "Radmin-完全控制","Radmin-仅限查看","Radmin-telnet","Radmin-文件传送","Radmin-关机","Radmin-聊天","Radmin-语音聊天","Radmin-传送信息","Radmin-自定义",
            "VNC-tightvnc","VNC-realvnc","VNC-ultravnc","VNC-自定义",
            "Winscp-sftp","Winscp-scp","Winscp-ftp","Winscp-http","Winscp-https","Winscp-自定义",
            "SecureCrt-ssh1","SecureCrt-ssh2","SecureCrt-telnet","SecureCrt-自定义",
            "Mobaxterm-ssh","Mobaxterm-telnet","Mobaxterm-自定义",
            "Todesk","Todesk-自定义",
        };
        List<string> allow_types = new List<string> {
            "RDP",
            "Putty-ssh","Putty-telnet","Putty-自定义",
            "Xshell-ssh","Xshell-telnet","Xshell-sftp","Xshell-自定义",
            "Xftp-sftp","Xshell-ftp","Xftp-自定义",
            "Radmin-完全控制","Radmin-仅限查看","Radmin-telnet","Radmin-文件传送","Radmin-关机","Radmin-聊天","Radmin-语音聊天","Radmin-传送信息","Radmin-自定义",
            "VNC-tightvnc","VNC-realvnc","VNC-ultravnc",
            "Winscp-sftp","Winscp-scp","Winscp-ftp","Winscp-自定义",
            "SecureCrt-ssh1","SecureCrt-ssh2","SecureCrt-telnet","SecureCrt-自定义",
            "Mobaxterm-ssh","Mobaxterm-telnet","Mobaxterm-自定义",
            "Todesk","Todesk-自定义",
        };
        string current_group_id = "-1";
        string save_width = "";
        string save_height = "";
        public FMain() {
            InitializeComponent();
            ThreadPool.RegisterWaitForSingleObject(Program.ProgramStarted, OnProgramStarted, null, -1, false);
            if (!Directory.Exists("data"))Directory.CreateDirectory("data");
            check();
        }
        //迁移到v1.2
        //private void move_to_1_2() {
        //    if (Share.now_version != "1.2") return;
        //    string? check = DbInihelper.GetIniData(iniconfig_action, "is_ask_move_1_2", iniconfig_path);
        //    if (check == "1") return;
        //    //1.询问，选择数据库位置
        //    if (ShowAskDialog("检测到首次打开v1.2是否自动导入v1.1数据！", false)) {
        //        //2.导入数据库
        //        string file_path = show_dialog_db();
        //        if (file_path == "") return;
        //    } else {
        //        DbInihelper.SetIniData(iniconfig_action, "is_ask_move_1_2", "1", iniconfig_path);
        //        return;
        //    }
        //    //3.清除注册表
        //    //RegistryKey rkey = Registry.CurrentUser;
        //    //rkey.CreateSubKey(@"SOFTWARE\MstscManager");
        //    //rkey.DeleteSubKey(@"SOFTWARE\MstscManager");
        //    //4.存储询问标志
        //    DbInihelper.SetIniData(iniconfig_action, "is_ask_move_1_2", "1", iniconfig_path);
        //}

        private void check() {
            //注册表拿到是否需要开启输入密码配置
            //注册表拿密码
            //注册表拿sql位置
            //v1.2 改用iniconfig 
            Dictionary<string, string>? dict = DbInihelper.GetIniSection(iniconfig_action, iniconfig_path);
            if (dict == null) dict = new Dictionary<string, string>();
            is_open_with_mm = dict.ContainsKey("is_open_with_mm") == true ? dict["is_open_with_mm"] :"-1";
            is_hide_behind = dict.ContainsKey("is_hide_behind") == true ? dict["is_hide_behind"] :"0";
            string mstsc_pass_s = dict.ContainsKey("mstsc_pass") == true ? dict["mstsc_pass"] :"-1";
            string db_path_s = dict.ContainsKey("db_path") == true ? dict["db_path"] :"-1";
            string old_db_path_s = dict.ContainsKey("old_db_path") == true ? dict["old_db_path"] :"-1";
            save_height = dict.ContainsKey("save_height") == true ? dict["save_height"] : "";
            save_width = dict.ContainsKey("save_width") == true ? dict["save_width"] : "";
            //初始化，数据库，界面
            if (db_path_s != "-1") { db_path = db_path_s; }
            if (old_db_path_s != "-1") { if (File.Exists(old_db_path_s))File.Delete(old_db_path_s);  }
            if (mstsc_pass_s == "-1") {//首次启动需要设置超级密码
                string value = "";
                if (this.InputStringDialog(ref value, true, "首次启动，请设置超级密码！", true)) {
                    string mm = common_tools.md5(value);
                    //key.SetValue("mstsc_pass", mm);
                    DbInihelper.SetIniData(iniconfig_action, "mstsc_pass", mm, iniconfig_path);
                    password = mm;
                    db_password = mm.Substring(0, 16);
                    //key.Close();rkey.Close();
                    self_init();
                } else {
                    System.Environment.Exit(0);
                    return;
                }
            } else {
                if (is_open_with_mm == "1") {
                    //弹出密码框
                    string value = "";
                    if (this.InputPasswordDialog(ref value, true, "请输入管理员密码", true)) {
                        if (common_tools.md5(value.ToString()) == mstsc_pass_s) {
                            password = mstsc_pass_s;
                            db_password = mstsc_pass_s.Substring(0, 16);
                            //key.Close(); rkey.Close();
                            self_init();
                            return;
                        } else {
                            ShowErrorTip("密码错误！");
                            Thread.Sleep(1000);
                            System.Environment.Exit(0);
                            return;
                        }
                    } else {
                        System.Environment.Exit(0);
                        return;
                    }
                } else {
                    password = mstsc_pass_s;
                    db_password = mstsc_pass_s.Substring(0, 16);
                    //key.Close(); rkey.Close();
                    self_init();
                }
            }
        }
        public void set_mm_status(string status) {
            is_open_with_mm = status;
        }
        public void set_db_path(string status) {
            db_path = status;
        }
        private void self_init() {
            init_db();
            UI_init();
            init_server_table();
            if (save_width != "") {
                this.Width = Convert.ToInt32(save_width) ;
                this.Height = Convert.ToInt32(save_height);
            }
        }
        private void init_db() {
            try {
                var connectionString = new SqliteConnectionStringBuilder() {
                    Mode = SqliteOpenMode.ReadWriteCreate,
                    DataSource = db_path,
                    Password = db_password,
                    Pooling = true,
                }.ToString();
                DbSqlHelper.ConnectionString = connectionString;
                //if(DbSqlHelper.common_conn==null) DbSqlHelper.common_conn = new SqliteConnection(connectionString);
                //if (DbSqlHelper.common_conn.State != ConnectionState.Open) DbSqlHelper.common_conn.Open();
                // 开始计时
                //Stopwatch watch = new Stopwatch();
                //watch.Start();
                //DbSqlHelper.common_transaction = DbSqlHelper.common_conn.BeginTransaction();
                DbSqlHelper.ExecuteNonQuery2("CREATE TABLE IF NOT EXISTS Group_setting( ID integer PRIMARY KEY AUTOINCREMENT , group_name TEXT,group_head_id TEXT);");
                DbSqlHelper.ExecuteNonQuery2("CREATE TABLE IF NOT EXISTS Commom_setting( ID integer PRIMARY KEY AUTOINCREMENT , key TEXT, val TEXT);");
                DbSqlHelper.ExecuteNonQuery2("CREATE TABLE IF NOT EXISTS User_setting( ID integer PRIMARY KEY AUTOINCREMENT , user_name TEXT, user_pass TEXT, mark_text TEXT);");
                DbSqlHelper.ExecuteNonQuery2("CREATE TABLE IF NOT EXISTS Server_setting( ID integer PRIMARY KEY AUTOINCREMENT , server_name TEXT, group_id integer, connect_type TEXT, ip TEXT, port TEXT, user_name TEXT, user_pass TEXT, end_date TEXT,mark_text TEXT,user_id integer,connect_setting TEXT,connect_string TEXT);");
                //DbSqlHelper.common_transaction.Commit();
                //DbSqlHelper.common_transaction = null;
                // 停止计时
                //watch.Stop();
                //Console.WriteLine(watch.Elapsed);
            } catch (Exception e) {
                Console.WriteLine(e);
                ShowErrorTip("数据库打开错误！");
                Thread.Sleep(1000);
                System.Environment.Exit(0);
            }
        }
        private void UI_init() {
            InitImageList();
            //初始化分组内容
            init_group_name();
            //初始化连接类型分类下拉框
            connct_types.ForEach(t => {
                uiComboBox1.Items.Add(t);
            });
            //表格格式设置
            UIDataGridView view = uiDataGridView1;
            view.AutoGenerateColumns = false;
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            view.AllowUserToResizeRows = false;
            view.AllowUserToResizeColumns = true;
            view.AllowUserToAddRows = false;
            view.AllowUserToDeleteRows = false;
            view.CellBorderStyle = DataGridViewCellBorderStyle.None;
            view.MultiSelect = true;
            view.RowHeadersVisible = false;
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void InitImageList() {
            ImageList il = new ImageList();
            il.Images.Add("icons8_folder_96", Resources.icons8_folder_96);
            il.Images.Add("icons8_documents_folder_96", Resources.icons8_documents_folder_96);
            uiTreeView1.ImageList = il;
        }
        private void init_group_name() {
            this.uiTreeView1.Nodes.Clear();
            this.uiComboBox2.Clear();

            this.uiTreeView1.Nodes.Add("all", "全部分类", 0, 1);
            this.uiComboBox2.Items.Add("全部分类");
            this.uiComboBox2.Text = "全部分类";
            //if (DbSqlHelper.common_conn.State != ConnectionState.Open) DbSqlHelper.common_conn.Open();
            //DbSqlHelper.common_transaction = DbSqlHelper.common_conn.BeginTransaction();
            SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_head_id = -1");
            while (reader.Read()) {
                string group_name = (string)reader["group_name"];
                this.uiTreeView1.Nodes.Add(group_name, group_name, 0, 1);
                uiComboBox2.Items.Add(group_name);
                //添加子分类
                TreeNode parent_node = this.uiTreeView1.Nodes[group_name];
                //Console.WriteLine(parent_node.Text);
                SqliteDataReader reader2 = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_head_id = ?", Convert.ToInt32(reader["id"]));
                if (reader2 == null) continue;
                while (reader2.Read()) {
                    string goup_name_sec = (string)reader2["group_name"];
                    parent_node.Nodes.Add(goup_name_sec, goup_name_sec, 0, 1);
                    uiComboBox2.Items.Add(goup_name_sec);
                }
                reader2.Close();
            }
            reader.Close();
            //DbSqlHelper.common_transaction.Commit();
            //DbSqlHelper.common_transaction = null;
        }
        public void init_server_table() {
            clear_old_info();
            load_server_table("select * from Server_setting where group_id = '-1'");
        }
        public void refresh_server_table() {
            clear_old_info();
            if(current_group_id=="-1") load_server_table("select * from Server_setting where group_id = '-1'");
            else load_server_table("select * from Server_setting where group_id = ?", current_group_id);
        }
        private void load_server_table(string sql, params object[] p) {
            SqliteDataReader reader = DbSqlHelper.ExecuteReader2(sql, p);
            int serer_num = 0;
            while (reader.Read()) {
                int index = uiDataGridView1.Rows.Add((string)reader["connect_type"],
                    (string)reader["server_name"],
                    (string)reader["ip"] + ":" + (string)reader["port"],
                    (string)reader["user_name"],
                    (string)reader["end_date"],
                    "...",
                    reader["id"].ToString(),
                    (string)reader["connect_setting"]
                    );
                check_ping(index, reader["ip"].ToString());
                if (reader["end_date"].ToString() != "") check_end_date(index, reader["end_date"].ToString());
                serer_num++;
            }
            reader.Close();
            uiLabel10.Text = $"当前分类：{Share.now_group_name} 共有 {serer_num} 台服务器";
            if(uiDataGridView1.Rows.Count>0)uiDataGridView1.Rows[0].Selected = false;
        }
        private void check_ping(int index, string ip) {
            Thread th = new Thread(() => {
                try {
                    string status = common_tools.pinghost(ip);
                    uiDataGridView1.Rows[index].Cells[5].Value = status;
                } catch (Exception) { }
            });
            th.Start();
        }
        private void check_end_date(int index, string end_date) {
            Thread th = new Thread(() => {
                DateTime dt = DateTime.ParseExact(end_date, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                DateTime now_time = System.DateTime.Now;
                TimeSpan span1 = dt - now_time;
                //Console.WriteLine(span1.Days);
                if (span1.Days <= 0) {
                    uiDataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.LightPink;
                } else if (span1.Days <= 5 ){
                    uiDataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            });
            th.Start();
        }
        //设置
        private void FMain_ExtendBoxClick(object sender, EventArgs e) {
            var setting = new Setting();
            setting.Show();
        }
        //其他功能
        private void uiButton5_Click(object sender, EventArgs e) {
            if (this.Controls.ContainsKey("OthoerFun")) {
                this.Controls.Remove(this.Controls["OthoerFun"]);
                return;
            }
            OthoerFun thf = new OthoerFun();
            this.Controls.Add(thf);
            Point tmp = common_tools.Compute_absolute_point(uiButton5);
            //tmp.X -= uiButton5.Width;
            tmp.X -= 4;
            tmp.Y += uiButton5.Height;
            thf.Location = tmp;
            thf.Show();
            thf.BringToFront();
        }
        private void FMain_Load(object sender, EventArgs e) {
            this.StartPosition = FormStartPosition.CenterScreen;
            Share.fm = this;
        }
        private void 添加分类ToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode parent_node = uiTreeView1.SelectedNode.Parent;
            //如果父节点不为顶级节点，需要查询父节点id
            int head_group_id = -1;
            if (parent_node != null) {
                SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_name = ?", parent_node.Text.ToString());
                bool flag = reader.Read();
                if (flag) {
                    head_group_id = Convert.ToInt32(reader["id"]);
                }
                reader.Close();
                if (head_group_id == -1) {
                    ShowErrorTip("没有查询到该父级分组！");
                    return;
                }
            }
            string group_name = "";
            if (this.InputStringDialog(ref group_name, true, "请输入分组名称：", false)) {
                if (group_name == "") return;

                //创建一个节点对象，并初始化 
                TreeNode tmp = new TreeNode(group_name, 0, 1);
                ////在TreeView组件中加入子节点 
                //uiTreeView1.SelectedNode.Nodes.Add(tmp);
                //uiTreeView1.SelectedNode = tmp;
                //uiTreeView1.ExpandAll();

                DbSqlHelper.ExecuteNonQuery2("insert into Group_setting (group_name,group_head_id) values (?,?)", group_name, head_group_id);
                if(parent_node==null)
                    uiTreeView1.Nodes.Add(group_name, group_name, 0, 1);
                else 
                    uiTreeView1.SelectedNode.Parent.Nodes.Add(tmp);
                uiComboBox2.Items.Add(group_name);
            }
        }
        private void 添加子分类ToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode parent_node = uiTreeView1.SelectedNode.Parent;
            if (parent_node!= null) {
                ShowInfoTip("暂只支持2级分类");
                return;
            }
            string head_group_name = uiTreeView1.SelectedNode.Text.ToString();
            if (head_group_name == "全部分类") {
                ShowInfoTip("全部分类下不支持添加子分类");
                return;
            }
            string group_name = "";
            if (this.InputStringDialog(ref group_name, true, "请输入分组名称：", false)) {
                if (group_name == "") return;
                //拿到head节点，head节点id
                SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_name = ?", head_group_name);
                int head_group_id = -1;
                bool flag = reader.Read();
                if (flag) {
                    head_group_id = Convert.ToInt32(reader["id"]);
                }
                reader.Close();
                if (head_group_id == -1) {
                    ShowErrorTip("没有查询到该分组！");
                    return;
                }

                //创建一个节点对象，并初始化 
                TreeNode tmp = new TreeNode(group_name, 0, 1);
                //在TreeView组件中加入子节点 
                uiTreeView1.SelectedNode.Nodes.Add(tmp);
                uiTreeView1.SelectedNode = tmp;
                //uiTreeView1.ExpandAll();

                DbSqlHelper.ExecuteNonQuery2("insert into Group_setting (group_name,group_head_id) values (?,?)", group_name, head_group_id);
                //this.uiTreeView1.Nodes.Add(group_name, group_name, 0, 1);
                this.uiComboBox2.Items.Add(group_name);
            }
        }
        private void 删除分类ToolStripMenuItem_Click(object sender, EventArgs e) {
            var select_node = this.uiTreeView1.SelectedNode;
            string select_name = this.uiTreeView1.SelectedNode.Text;
            //Console.WriteLine(select_name);
            if (select_name == "全部分类") {
                UIMessageDialog.ShowMessageDialog("全部分类无需删除！", "提示", false, Style,false);
                return;
            }
            if (uiTreeView1.SelectedNode.Nodes.Count != 0 ) {
                ShowInfoTip("请先删除此节点中的子节点！");
                return;
            }
            if (ShowAskDialog("确定要删除<"+select_name+">分类吗？",false)) {
                SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_name = ?", select_name);
                int group_id = -1;
                bool flag = reader.Read();
                if (flag) {
                    group_id = Convert.ToInt32(reader["id"]);
                }
                reader.Close();
                if (group_id == -1) {
                    ShowErrorTip("没有查询到该分组！");
                    return;
                }
                DbSqlHelper.ExecuteNonQuery2("delete from Group_setting where group_name = ?", select_name);
                DbSqlHelper.ExecuteNonQuery2("delete from Server_setting where group_id = ?", group_id);
                this.uiTreeView1.Nodes.Remove(select_node);
                this.uiComboBox2.Items.Remove(select_name);
                clear_old_info();
            } else { }
        }
        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e) {
            string new_group_name = "";
            string select_name = this.uiTreeView1.SelectedNode.Text;
            if (select_name == "全部分类") {
                UIMessageDialog.ShowMessageDialog("<全部分类>不能重命名！", "提示", false, Style, false);
                return;
            }
            if (this.InputStringDialog(ref new_group_name, true, "请输入新分组名称：", false)) {
                if (new_group_name == "") return;
                DbSqlHelper.ExecuteNonQuery2("update Group_setting set group_name = ? where group_name = ?", new_group_name, select_name);
                this.uiTreeView1.SelectedNode.Text = new_group_name;
                this.uiComboBox2.Items.Remove (select_name);
                this.uiComboBox2.Items.Add (new_group_name);
            }
        }
        //搜索
        private void uiTextBox1_ButtonClick(object sender, EventArgs e) {
            string serch_str = this.uiTextBox1.Text;
            if (serch_str.Trim() == "") {
                ShowInfoTip("搜索内容不能为空！");
                return;
            }
            ShowInfoTip("搜索："+ serch_str);
            clear_old_info();
            string sql = "select * from Server_setting where server_name like '%" + serch_str + "%' or ip like '%"+ serch_str + "%' or connect_type like '%" + serch_str + "%'or port like '%"+ serch_str + "%'";
            //Console.WriteLine(sql);
            load_server_table(sql);
        }
        //回车搜索
        private void uiTextBox1_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                uiTextBox1_ButtonClick(null, null);
            }
        }
        //搜索框tips 
        private void uiTextBox1_MouseEnter(object sender, EventArgs e) {
            uiToolTip1.Show("搜索支持连接类型，主机名称，连接ip，port的模糊搜索", this, 80, 74, 3000);
        }
        //点击分组
        private void uiTreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Button != MouseButtons.Left) { uiTreeView1.SelectedNode = e.Node; return; }
            var group_name = e.Node.Text;
            clear_old_info();
            if (group_name == "全部分类") { init_server_table(); return; }
            SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_name = ?", group_name);
            int group_id = -1;
            bool flag = reader.Read();
            if (flag) { group_id = Convert.ToInt32(reader["id"]); current_group_id = reader["id"].ToString(); }
            reader.Close();
            if (group_id == -1) { ShowErrorTip("没有查询到该分组！"); return; }
            if (uiTreeView1.SelectedNode.Nodes.Count != 0) uiTreeView1.SelectedNode.ExpandAll();
            Share.now_group_name = group_name;
            load_server_table("select * from Server_setting where group_id = ?", group_id);
        }
        //添加服务器
        private void uiButton1_Click(object sender, EventArgs e) {
            var add_server = new Add_server();
            add_server.Show();
        }
        //编辑服务器
        private void uiButton2_Click(object sender, EventArgs e) {
            if (uiDataGridView1.SelectedRows.Count <= 0) { ShowErrorTip("请选择一条记录"); return; }
            string id = uiDataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            string connect_setting = uiDataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            var add_server = new Add_server(connect_setting, id);
            add_server.Show();
        }
        //删除主机
        private void uiButton3_Click(object sender, EventArgs e) {
            if (uiDataGridView1.SelectedRows.Count <= 0) { ShowErrorTip("请选择一条记录"); return; }
            if (ShowAskDialog("确定要删除当前选中的"+ uiDataGridView1.SelectedRows.Count.ToString() + "个主机吗？", false)) {
                ShowStatusForm(uiDataGridView1.SelectedRows.Count, "数据删除中......", 0);
                var selected_rows = uiDataGridView1.SelectedRows;
                try {
                    int len = uiDataGridView1.SelectedRows.Count;
                    if (DbSqlHelper.common_conn == null) DbSqlHelper.common_conn = new SqliteConnection(DbSqlHelper.ConnectionString);
                    if (DbSqlHelper.common_conn.State != ConnectionState.Open) DbSqlHelper.common_conn.Open();
                    DbSqlHelper.common_transaction = DbSqlHelper.common_conn.BeginTransaction();
                    for (int i = 0; i < len; i++) {
                        //SystemEx.Delay(50);
                        SetStatusFormDescription("数据删除中......");
                        StatusFormStepIt();
                        string id = selected_rows[i].Cells[6].Value.ToString();
                        DbSqlHelper.ExecuteNonQuery("delete from Server_setting where id = ?", id);
                        //Thread.Sleep(50);
                        //uiDataGridView1.Rows.Remove(uiDataGridView1.SelectedRows[0]);
                    }
                    DbSqlHelper.common_transaction.Commit();
                    DbSqlHelper.common_transaction = null;
                    DbSqlHelper.common_conn.Close();
                } catch (Exception) { };
                HideStatusForm();
                ShowSuccessTip("数据删除成功");
                refresh_server_table();
            }
        }
        //text导入
        private void uiButton4_Click(object sender, EventArgs e) {
            string tip_msg = "提醒：导入必须按照下面规定数据导入：\n" +
                "服务器名称,分类名称,连接类型,IP,端口,用户名,密码,到期时间,服务器备注,自定义规则\n" +
                "10个数据，中间用英文逗号分开，没有的数据留空，一行一条数据\n" +
                "其中连接类型只能是下面列表里面的之一,区分大小写：\n" +
                String.Join(" , ", allow_types) +
                "不符合格式的数据将直接忽略，可以使用导出的模板进行编辑。";
            if (ShowAskDialog(tip_msg)) {
                //ShowSuccessTip("您点击了确定按钮");
                //选择导入的文件
                string file_path = show_dialog();
                if (file_path == "") return;

                //拿grouplist
                Dictionary<string, string> group_dic = new Dictionary<string, string>();
                DbDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting");
                while (reader.Read()) {
                    group_dic.Add(reader["group_name"].ToString(), reader["id"].ToString());
                }
                group_dic.Add("全部分类","-1");
                reader.Close();

                string[]? contents = null;
                try {
                    contents = File.ReadAllLines(file_path, Encoding.UTF8);
                } catch (Exception) {
                    return;
                }
                // 开始计时
                //Stopwatch watch = new Stopwatch();
                //watch.Start();

                ShowStatusForm(contents.Length, "数据添加中......", 0);
                //添加分组
                for (int i = 0; i < contents.Length; i++) {
                    if (i == 0) continue;
                    string[] strNew = contents[i].Split(new char[] { ',' });
                    if (strNew.Length != 10) continue;
                    if (!allow_types.Contains(strNew[2])) continue;
                    string group_id = "-1";
                    if (!group_dic.Keys.Contains(strNew[1])) { //insert gourp
                        DbSqlHelper.ExecuteNonQuery2("INSERT INTO Group_setting (group_name,group_head_id) VALUES (?,-1)", strNew[1]);
                        //Thread.Sleep(500);
                        SqliteDataReader reader1 = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_name = ?", strNew[1]);
                        if (reader1.Read()) {
                            group_dic.Add(strNew[1], reader1["id"].ToString());
                            this.uiTreeView1.Nodes.Add(strNew[1], strNew[1], 0, 1);
                            this.uiComboBox2.Items.Add(strNew[1]);
                        }
                        reader1.Close();
                    }
                    group_id = group_dic[strNew[1]];
                }
                if (DbSqlHelper.common_conn == null) DbSqlHelper.common_conn = new SqliteConnection(DbSqlHelper.ConnectionString);
                if (DbSqlHelper.common_conn.State != ConnectionState.Open) DbSqlHelper.common_conn.Open();
                DbSqlHelper.common_transaction = DbSqlHelper.common_conn.BeginTransaction();
                
                //插入数据
                for (int i = 0; i < contents.Length; i++) {
                    if (i == 0) continue;
                    string[] strNew = contents[i].Split(new char[] { ',' });
                    if (strNew.Length != 10) continue;
                    if (!allow_types.Contains(strNew[2])) continue;
                    string group_id = "-1";
                    group_id = group_dic[strNew[1]];
                    string connect_setting = get_default_config(true, strNew, group_id);
                    if (connect_setting == "") continue;
                    DbSqlHelper.ExecuteNonQuery("INSERT INTO Server_setting(server_name,group_id,connect_type,ip,port,user_name,user_pass,end_date,mark_text,user_id,connect_setting,connect_string) VALUES (?,?,?,?,?,?,?,?,?,?,?,?);", strNew[0], group_id, strNew[2], strNew[3], strNew[4], strNew[5], strNew[6], strNew[7], strNew[8], "-1", connect_setting, strNew[9]);
                    SetStatusFormDescription("数据添加中......");
                    StatusFormStepIt();
                }

                DbSqlHelper.common_transaction.Commit();
                DbSqlHelper.common_transaction = null;
                DbSqlHelper.common_conn.Close();
                // 停止计时
                //watch.Stop();
                //Console.WriteLine(watch.Elapsed);
                HideStatusForm();
                refresh_server_table();
                UIMessageDialog.ShowMessageDialog("数据导入成功！", "提示", false, Style, false);
            }
        }
        private string show_dialog() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择对应的CSV文件";
            //ofd.Multiselect = true;
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            ofd.Filter = "所有文件|*.csv|文本文件|*.txt|所有文件|*.*";
            ofd.ShowDialog();
            return ofd.FileName;
        }
        private string show_dialog_db() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择对应的数据库文件";
            //ofd.Multiselect = true;
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            ofd.Filter = "所有文件|*.db|所有文件|*.*";
            ofd.ShowDialog();
            return ofd.FileName;
        }
        //其他功能-mstsc一键导入
        public void simple_import() {
            string tip_msg = "注意：一键导入只包括ip和端口，账号密码需要自己重新设置！";
            if (ShowAskDialog(tip_msg)) {
                RegistryKey rkey = Registry.CurrentUser;
                RegistryKey key = rkey.OpenSubKey(@"SOFTWARE\Microsoft\Terminal Server Client\Default");
                String[] names = key.GetValueNames();
                int index = 1;
                ShowStatusForm(names.Length, "数据导入中......", 0);
                //查询当前选中的分组名称和id
                DbDataReader reader = DbSqlHelper.ExecuteReader("select * from Group_setting where group_name = ?;",Share.now_group_name);
                reader.Read();
                string goup_id = reader["id"].ToString();
                reader.Close();
                if (DbSqlHelper.common_conn == null) DbSqlHelper.common_conn = new SqliteConnection(DbSqlHelper.ConnectionString);
                if (DbSqlHelper.common_conn.State != ConnectionState.Open) DbSqlHelper.common_conn.Open();
                DbSqlHelper.common_transaction = DbSqlHelper.common_conn.BeginTransaction();
                foreach (string keyname in names) {
                    string ip = "127.0.0.1";
                    string port = "3389";
                    string[] ipport = key.GetValue(keyname).ToString().Split(":");
                    ip = ipport[0];
                    if (ipport.Length == 2) port = ipport[1];
                    //Console.WriteLine(ip+":"+port);
                    string[] strNew = { "RDP_autoimport"+index.ToString(), Share.now_group_name, "RDP", ip, port, "", "", "", "" };
                    string connect_setting = get_default_config(true, strNew, goup_id);
                    if (connect_setting == "") continue;
                    DbSqlHelper.ExecuteNonQuery("INSERT INTO Server_setting(server_name,group_id,connect_type,ip,port,user_name,user_pass,end_date,mark_text,user_id,connect_setting,connect_string) VALUES (?,?,?,?,?,?,?,?,?,?,?,?);", strNew[0], goup_id, strNew[2], strNew[3], strNew[4], strNew[5], strNew[6], strNew[7], strNew[8], "-1", connect_setting, "");
                    index++;
                    SystemEx.Delay(50);
                    SetStatusFormDescription("数据导入中......");
                    StatusFormStepIt();
                }
                DbSqlHelper.common_transaction.Commit();
                DbSqlHelper.common_transaction = null;
                DbSqlHelper.common_conn.Close();
                key.Close();
                rkey.Close();
                HideStatusForm();
                ShowSuccessTip("一键导入成功！共导入"+ names.Length+ "个服务器");
                refresh_server_table();
            }
        }
        //其他功能批量检测
        public void ping_now_items() {
            //拿到所有items 循环 执行函数
            var rows = uiDataGridView1.Rows;
            foreach(DataGridViewRow item in rows) {
                int index = item.Index;
                string ip = item.Cells[2].Value.ToString().Split(":")[0];
                item.Cells[5].Value = "...";
                check_ping(index, ip);
            }
        }
            //text导出
        private void uiButton6_Click(object sender, EventArgs e) {
            string dir = "";
            if (DirEx.SelectDirEx("选择文件保存位置", ref dir)) {
                //UIMessageTip.ShowOk(dir);
                string path = Path.Combine(dir, "MstscManager_db_output.csv");
                string spilt_txt = ",";
                File.AppendAllText(path, "服务器名称,分类名称,连接类型,IP,端口,用户名,密码,到期时间,服务器备注,自定义规则\r\n", Encoding.UTF8);
                //读取group
                Dictionary<string, string> group_dic = new Dictionary<string, string>();
                DbDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting");
                while (reader.Read()) {
                    group_dic.Add(reader["id"].ToString(), reader["group_name"].ToString());
                }
                reader.Close();
                Dictionary<string, string> user_dic = new Dictionary<string, string>();
                DbDataReader reader1 = DbSqlHelper.ExecuteReader2("select * from User_setting");
                while(reader1.Read()) {
                    user_dic.Add(reader1["id"].ToString(), reader1["user_name"].ToString()+ spilt_txt + reader1["user_pass"].ToString());
                }
                reader1.Close();
                DbDataReader reader2 = DbSqlHelper.ExecuteReader2("select * from Server_setting");
                while (reader2.Read()) {
                    string s = reader2["server_name"].ToString()+ spilt_txt;
                    string group_id = reader2["group_id"].ToString();
                    if (group_id == "-1") { s += "全部分类"+ spilt_txt; } else { s += group_dic[group_id] + spilt_txt; }
                    s+= reader2["connect_type"].ToString() + spilt_txt + reader2["ip"].ToString() + spilt_txt + reader2["port"].ToString() + spilt_txt;
                    string user_id = reader2["user_id"].ToString();
                    if (user_id == "-1") { s += reader2["user_name"].ToString()+ spilt_txt + reader2["user_pass"].ToString()+ spilt_txt; } else { s += user_dic[user_id] + spilt_txt; }
                    s += reader2["end_date"].ToString() + spilt_txt + reader2["mark_text"].ToString().Replace("\r\n", "-").Replace("\n","-") + spilt_txt + reader2["connect_string"].ToString();
                    File.AppendAllText(path,s+"\r\n", Encoding.UTF8);
                }
                reader2.Close();
                ShowInfoNotifier("导出成功！",false,5000);
            }
        }
        //单击显示信息
        private void uiDataGridView1_MouseClick(object sender, MouseEventArgs e) {
            if (uiDataGridView1.SelectedRows.Count <= 0) return;
            string connect_setting = uiDataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            JObject csobj = JsonConvert.DeserializeObject<JObject>(connect_setting);
            now_csobj = csobj;
            //Console.WriteLine(csobj);
            uiTextBox7.Text = csobj["server_name"].ToString();
            uiComboBox1.Text = csobj["connect_type"].ToString();
            uiComboBox2.Text = csobj["group_name"].ToString();
            uiTextBox3.Text = csobj["ip"].ToString();
            uiTextBox2.Text = csobj["port"].ToString();
            string user_id = csobj["user_id"].ToString();
            if (user_id != "-1") {
                uiTextBox4.Text = "";
                uiTextBox5.Text = "";
                //uiTextBox4.Watermark = csobj["user_name"].ToString();
                uiTextBox4.Watermark = "使用账户配置:id:" + user_id;
                uiTextBox5.Watermark = "使用账户配置:id:"+ user_id;
                //uiTextBox4.Enabled = false;
                //uiTextBox5.Enabled = false;
            } else {
                uiTextBox4.Text = csobj["user_name"].ToString();
                if (csobj["user_pass"].ToString().Trim() == "") uiTextBox5.Text = "";
                else uiTextBox5.Text = "*********";
                //uiTextBox5.Text = csobj["user_pass"].ToString();
                uiTextBox4.Watermark = "";
                uiTextBox5.Watermark = "";
                //uiTextBox4.Enabled = true;
                //uiTextBox5.Enabled = true;
            }
            string end_date = csobj["end_date"].ToString();
            if (end_date == "") {
                uiDatePicker1.Text = "";
                uiDatePicker1.Watermark = "未设置到期时间";
            } else {
                uiDatePicker1.Text = end_date;
                uiDatePicker1.Watermark = "";
            }
            
            uiTextBox6.Text = csobj["mark_text"].ToString();
        }
        //双击连接服务器
        private void uiDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e) {
            connect_serer();
        }
        //拿默认配置
        private string get_default_config(bool is_txt = false, string[]? strarr=null,string gpid="-1") {
            //不管其他的东西，只管拿到的数据
            string connect_type = (string)(uiComboBox1.SelectedItem == null ? uiComboBox1.Text : uiComboBox1.SelectedItem);
            string server_name = uiTextBox7.Text;
            string group_name = (string)(uiComboBox2.SelectedItem == null ? uiComboBox2.Text : uiComboBox2.SelectedItem);
            string ip = uiTextBox3.Text;
            string port = uiTextBox2.Text;
            string user_name = uiTextBox4.Text;
            string user_pass = uiTextBox5.Text;
            if(!is_txt) { 
                //适配点击右下角连接服务器 v1.2
                if (user_pass == "") {
                    //补全userid的密码
                    string user_id = now_csobj["user_id"].ToString();
                    if (user_id != "-1") {
                        DbDataReader reader = DbSqlHelper.ExecuteReader("select * from User_setting where id = ?", user_id);
                        if (reader.Read()) {
                            user_name = reader["user_name"].ToString();
                            user_pass = reader["user_pass"].ToString();
                        }
                    }
                }else if (user_pass == "*********") {
                    user_name = now_csobj["user_name"].ToString();
                    user_pass = now_csobj["user_pass"].ToString();
                }
            }
            string end_date = uiDatePicker1.Text;
            string mark_text = uiTextBox6.Text;
            string group_id = "-1";
            if(is_txt) {
                server_name = strarr[0];
                group_id = gpid;
                group_name = strarr[1];
                connect_type = strarr[2];
                ip = strarr[3];
                port = strarr[4];
                user_name = strarr[5];
                user_pass = strarr[6];
                end_date = strarr[7];
                mark_text = strarr[8];
            }
            if (!allow_types.Contains(connect_type)) { ShowErrorTip("缺少参数，不支持当前类型"); return ""; }

            string connect_setting = "";
            //生成默认配置，传入connect_server
            if (connect_type.IndexOf("RDP") != -1) {
                connect_setting = "{ \"audiocapturemode\": \"0\", \"audiocapturemode_name\": \"不录制\", \"audiomode\": \"0\", \"audiomode_name\": \"在此计算机上播放\", \"autoreconnection\": \"1\", \"camerastoredirect\": \" \", \"compression\": \"1\", \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"disable_wallpaper\": \"0\", \"displayconnectionbar\": \"1\", \"drivestoredirect\": \" \", \"end_date\": \""+end_date+"\", \"group_id\": \""+ group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"keyboardhook\": \"2\", \"keyboardhook_name\": \"仅在全屏显示时\", \"mark_text\": \""+ mark_text + "\", \"p_admin_mode\": \"1\", \"p_prompt\": \"0\", \"port\": \"" + port + "\", \"redirectclipboard\": \"1\", \"redirectcomports\": \"0\", \"redirectposdevices\": \"0\", \"redirectprinters\": \"0\", \"redirectsmartcards\": \"0\", \"screen_height\": \"768\", \"screen_mode\": \"2\", \"screen_mode_name\": \"全屏\", \"screen_width\": \"1024\", \"server_name\": \""+ server_name + "\", \"session_bpp\": \"32\", \"use_multimon\": \"0\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\" }";
            } else if (connect_type.IndexOf("Putty") != -1) {
                connect_setting = "{ \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"end_date\": \""+end_date+ "\", \"group_id\": \""+ group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \""+ mark_text + "\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"" + connect_type.Split("-")[1] + "\", \"server_name\": \""+ server_name + "\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\",\"is_ssh_rsa\": \"0\",\"ssh_rsa_path\": \"\" }";
            } else if (connect_type.IndexOf("Xshell") != -1) {
                connect_setting = "{ \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"end_date\": \""+end_date+ "\", \"group_id\": \""+ group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \""+ mark_text + "\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"" + connect_type.Split("-")[1] + "\", \"server_name\": \""+ server_name + "\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\" }";
            } else if (connect_type.IndexOf("Xftp") != -1) {
                connect_setting = "{ \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"end_date\": \""+end_date+ "\", \"group_id\": \""+ group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \""+ mark_text + "\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"" + connect_type.Split("-")[1] + "\", \"server_name\": \""+ server_name + "\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\" }";
            } else if (connect_type.IndexOf("Radmin") != -1) {
                connect_setting = "{ \"color_mode\": \"24bpp\", \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"encrypt\": \"0\", \"end_date\": \""+end_date+ "\", \"fullscreen\": \"0\", \"group_id\": \""+ group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \""+ mark_text + "\", \"nofullkbcontrol\": \"0\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"" + connect_type.Split("-")[1] + "\", \"server_name\": \""+ server_name + "\", \"updates\": \"30\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\" }";
            } else if (connect_type.IndexOf("VNC") != -1) {
                connect_setting = "{ \"autoreconnect\": \"1\", \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"end_date\": \""+end_date+ "\", \"fullscreen\": \"0\", \"group_id\": \""+ group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \""+ mark_text + "\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"" + connect_type.Split("-")[1] + "\", \"server_name\": \""+ server_name + "\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\", \"viewonly\": \"0\" }";
            } else if (connect_type.IndexOf("Winscp") != -1) {
                connect_setting = "{ \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"dav_address\": \"\", \"end_date\": \""+end_date+ "\", \"group_id\": \""+ group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \""+ mark_text + "\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"" + connect_type.Split("-")[1] + "\", \"server_name\": \""+ server_name + "\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\", }";
            } else if (connect_type.IndexOf("SecureCrt") != -1) {
                connect_setting = "{ \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"end_date\": \""+end_date+ "\", \"group_id\": \""+ group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \""+ mark_text + "\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"" + connect_type.Split("-")[1] + "\", \"server_name\": \""+ server_name + "\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\", }";
            } else if (connect_type.IndexOf("Mobaxterm") != -1) {
                connect_setting = "{ \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"end_date\": \"" + end_date + "\", \"group_id\": \"" + group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \"" + mark_text + "\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"" + connect_type.Split("-")[1] + "\", \"server_name\": \"" + server_name + "\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\" ,\"is_ssh_rsa\": \"0\",\"ssh_rsa_path\": \"\"}";
            } else if (connect_type.IndexOf("Todesk") != -1) {
                connect_setting = "{ \"connect_setting\": \"\", \"connect_string\": \"\", \"connect_type\": \"" + connect_type + "\", \"end_date\": \"" + end_date + "\", \"group_id\": \"" + group_id + "\", \"group_name\": \"" + group_name + "\", \"ip\": \"" + ip + "\", \"mark_text\": \"" + mark_text + "\", \"port\": \"" + port + "\", \"sec_connect_mode\": \"\", \"server_name\": \"" + server_name + "\", \"user_id\": \"-1\", \"user_name\": \"" + user_name + "\", \"user_pass\": \"" + user_pass + "\" }";
            }
            //Console.WriteLine(connect_setting);
            return connect_setting;
        }
        //添加服务器 临时
        private void uiButton8_Click(object sender, EventArgs e) {
            string connect_setting = get_default_config();
            if (connect_setting == "") return;
            JObject config = JObject.Parse(connect_setting);
            //string new_connect_setting = JsonConvert.SerializeObject(config);
            string group_name = config["group_name"].ToString();
            if (group_name != "全部分类") {
                SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_name = ?", group_name);
                if (reader.Read()) {
                    config["group_id"] = reader["id"].ToString();
                }
                reader.Close();
            }
            DbSqlHelper.ExecuteNonQuery2("INSERT INTO Server_setting(server_name,group_id,connect_type,ip,port,user_name,user_pass,end_date,mark_text,user_id,connect_setting,connect_string) VALUES (?,?,?,?,?,?,?,?,?,?,?,?);",
                config["server_name"].ToString(), config["group_id"].ToString(), config["connect_type"].ToString(), config["ip"].ToString(), config["port"].ToString(), config["user_name"].ToString(), config["user_pass"].ToString(), config["end_date"].ToString(), config["mark_text"].ToString(), config["user_id"].ToString(), connect_setting, config["connect_string"].ToString());
            refresh_server_table();
            UIMessageDialog.ShowMessageDialog("数据添加成功！", "提示", false, Style, false);
        }
        //连接服务器 临时
        private void uiButton7_Click(object sender, EventArgs e) {
            string connect_setting = get_default_config();
            if (connect_setting == "") return;
            //Console.WriteLine(connect_setting);
            connect_serer(true, connect_setting);
        }
        //清空ui
        private void clear_old_info(bool is_temp=true) {
            if(is_temp)uiDataGridView1.Rows.Clear();
            uiTextBox7.Text = "";
            uiComboBox2.Text = "";
            uiComboBox1.Text = "";
            uiTextBox3.Text = "";
            uiTextBox2.Text = "";
            uiTextBox4.Text = "";
            uiTextBox4.Watermark = "";
            uiTextBox5.Text = "";
            uiTextBox5.Watermark = "";
            uiDatePicker1.Text = "";
            uiDatePicker1.Watermark = "";
            uiTextBox6.Text = "";
        }
        //连接服务器real
        private void connect_serer(bool is_temp = false, string cs_d = "") {
            string connect_setting = "";
            if (is_temp) connect_setting = cs_d;
            else connect_setting = uiDataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            JObject csobj = JsonConvert.DeserializeObject<JObject>(connect_setting);
            string connect_type = csobj["connect_type"].ToString();
            //补全userid的密码
            string user_id = csobj["user_id"].ToString();
            if (user_id != "-1") {
                DbDataReader reader = DbSqlHelper.ExecuteReader2("select * from User_setting where id = ?", user_id);
                if (reader.Read()) {
                    csobj["user_name"] = reader["user_name"].ToString();
                    csobj["user_pass"] = reader["user_pass"].ToString();
                } else {
                    ShowErrorTip("主机所关联的账号已删除！请重新设置");
                    return;
                }
            }
            //替换自定义生成字符串
            string user_connect_string = csobj["connect_string"].ToString().Trim();
            bool is_user_connect_string = user_connect_string == "" ? false : true;
            if (is_user_connect_string) {
                user_connect_string = user_connect_string.Replace("{username}", csobj["user_name"].ToString());
                user_connect_string = user_connect_string.Replace("{password}", csobj["user_pass"].ToString());
                user_connect_string = user_connect_string.Replace("{ip}", csobj["ip"].ToString());
                user_connect_string = user_connect_string.Replace("{port}", csobj["port"].ToString());
            }
            //个性配置初始化
            if (connect_type.IndexOf("RDP") != -1) {
                //判断自定义
                if (is_user_connect_string) {
                    common_tools.RunApp2("mstsc.exe", " " + user_connect_string);
                    return;
                }
                //检查是否设置对应exe
                //参数写入文件
                //string path = System.Environment.GetFolderPath(SpecialFolder.LocalApplicationData) + "\\Temp\\MstscManager.rdp";
                string path = "data\\MstscManager_temp.rdp";
                //Console.WriteLine(path);
                if (File.Exists(path)) { File.Delete(path); };
                File.WriteAllLines(path, new string[] {
                    "screen mode id:i:"+csobj["screen_mode"].ToString(),
                    "use multimon:i:"+csobj["use_multimon"].ToString(),
                    "desktopwidth:i:"+csobj["screen_width"].ToString(),
                    "desktopheight:i:"+csobj["screen_height"].ToString(),
                    "smart sizing:i:1",
                    "session bpp:i:"+csobj["session_bpp"].ToString(),
                    "winposstr:s:0,3,0,0,800,600",
                    "compression:i:"+csobj["compression"].ToString(),
                    "keyboardhook:i:"+csobj["keyboardhook"].ToString(),
                    "displayconnectionbar:i:"+csobj["displayconnectionbar"].ToString(),
                    "audiocapturemode:i:"+csobj["audiocapturemode"].ToString(),
                    "videoplaybackmode:i:1",
                    "username:s:"+csobj["user_name"].ToString(),
                    "networkautodetect:i:1",
                    "bandwidthautodetect:i:1",
                    "connection type:i:7",
                    "disable wallpaper:i:"+csobj["disable_wallpaper"].ToString(),
                    "enableworkspacereconnect:i:0",
                    "disable full window drag:i:1",
                    "allow desktop composition:i:0",
                    "allow font smoothing:i:0",
                    "disable menu anims:i:1",
                    "disable themes:i:0",
                    "disable cursor setting:i:0",
                    "bitmapcachepersistenable:i:1",
                    "full address:s:"+csobj["ip"].ToString()+":"+csobj["port"].ToString(),
                    "password 51:b:"+RDPcrypt.encrpyt(csobj["user_pass"].ToString()),
                    "audiomode:i:"+csobj["audiomode"].ToString(),
                    "redirectprinters:i:"+csobj["redirectprinters"].ToString(),
                    "redirectcomports:i:"+csobj["redirectcomports"].ToString(),
                    "redirectsmartcards:i:"+csobj["redirectsmartcards"].ToString(),
                    "camerastoredirect:s:"+csobj["camerastoredirect"].ToString(),
                    "redirectclipboard:i:"+csobj["redirectclipboard"].ToString(),
                    "redirectposdevices:i:"+csobj["redirectposdevices"].ToString(),
                    "drivestoredirect:s:"+csobj["drivestoredirect"].ToString(),
                    "autoreconnection enabled:i:"+csobj["autoreconnection"].ToString(),
                    "authentication level:i:2",
                    "prompt for credentials:i:0",
                    "negotiate security layer:i:1",
                    "remoteapplicationmode:i:0",
                    "alternate shell:s:",
                    "shell working directory:s:",
                    "gatewayhostname:s:",
                    "gatewayusagemethod:i:4",
                    "gatewaycredentialssource:i:4",
                    "gatewayprofileusagemethod:i:0",
                    "promptcredentialonce:i:0",
                    "gatewaybrokeringtype:i:0",
                    "use redirection server name:i:0",
                    "rdgiskdcproxy:i:0",
                    "kdcproxyname:s:",
                });
                //解决路径空格问题  v1.2
                path = "\"" + path + "\"";
                //生成连接字符串
                string connect_string = path;
                if (csobj["p_admin_mode"].ToString() == "1") connect_string += " /admin";
                else connect_string += " /console";
                if (csobj["p_prompt"].ToString() == "1") connect_string += " /prompt";
                //Console.WriteLine(connect_string);
                common_tools.RunApp2("mstsc.exe", " " + connect_string);
            } 
            else if (connect_type.IndexOf("Putty") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("putty_exe_path");
                if (exe_path == "") return;

                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }

                //根据二次类型生成对应的字符串
                string connect_string = "";
                string sec_connect_mode = csobj["sec_connect_mode"].ToString().ToLower();
                if (sec_connect_mode == "ssh") {
                    if (csobj["user_name"].ToString().Trim() == "") { ShowInfoTip($"{csobj["connect_type"].ToString()} 用户名不能为空！"); return; }
                    //密钥连接
                    if(csobj["is_ssh_rsa"].ToString().Trim() == "1") {
                        //putty.exe -ssh -l 用户名 -P 端口 -i rsa_path 服务器IP
                        connect_string += $" -ssh -l {csobj["user_name"].ToString()} -P {csobj["port"].ToString()}  -i {csobj["ssh_rsa_path"]} {csobj["ip"].ToString()}";
                    } else {
                        //putty.exe -ssh -l 用户名 -pw 密码 -P 端口 服务器IP
                        connect_string += $" -ssh -l {csobj["user_name"].ToString()} -pw {csobj["user_pass"].ToString()} -P {csobj["port"].ToString()} {csobj["ip"].ToString()}";
                    }
                } 
                else if (sec_connect_mode == "telnet") {
                    //putty0.77.exe -telnet -P 20891 127.0.0.1
                    connect_string += $" -telnet -P {csobj["port"].ToString()} {csobj["ip"].ToString()}";
                }
                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
            } 
            else if (connect_type.IndexOf("Xshell") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("xshell_exe_path");
                if (exe_path == "") return;

                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }

                //根据二次类型生成对应的字符串
                string connect_string = "";
                string sec_connect_mode = csobj["sec_connect_mode"].ToString().ToLower();
                if (sec_connect_mode == "ssh") {
                    if (csobj["user_name"].ToString().Trim() == "") { ShowInfoTip($"{csobj["connect_type"].ToString()} 用户名不能为空！"); return; }
                    //Xshell -url [protocol://][user[:password]@]host[:port]
                    connect_string += $" -url ssh://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                } else if (sec_connect_mode == "telnet") {
                    connect_string += $" -url telnet://{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                } else if (sec_connect_mode == "sftp") {
                    connect_string += $" -url sftp://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                }
                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
            } 
            else if (connect_type.IndexOf("Xftp") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("xftp_exe_path");
                if (exe_path == "") return;

                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }

                //根据二次类型生成对应的字符串
                string connect_string = "";
                string sec_connect_mode = csobj["sec_connect_mode"].ToString().ToLower();
                if (sec_connect_mode == "sftp") {
                    connect_string += $" -url sftp://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                } else if (sec_connect_mode == "ftp") {
                    connect_string += $" -url ftp://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                }
                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
            } 
            else if (connect_type.IndexOf("Radmin") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("radmin_exe_path");
                if (exe_path == "") return;

                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }

                //根据二次类型生成对应的字符串
                string connect_string = "";
                string sec_connect_mode = csobj["sec_connect_mode"].ToString().ToLower();
                if (sec_connect_mode == "完全控制") {
                    connect_string += $" /connect:{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                } else if (sec_connect_mode == "仅限查看") {
                    connect_string += $" /connect:{csobj["ip"].ToString()}:{csobj["port"].ToString()} /noinput";
                } else if (sec_connect_mode == "telnet") {
                    connect_string += $" /connect:{csobj["ip"].ToString()}:{csobj["port"].ToString()} /telnet";
                } else if (sec_connect_mode == "文件传送") {
                    connect_string += $" /connect:{csobj["ip"].ToString()}:{csobj["port"].ToString()} /file";
                } else if (sec_connect_mode == "关机") {
                    connect_string += $" /connect:{csobj["ip"].ToString()}:{csobj["port"].ToString()} /shutdown";
                } else if (sec_connect_mode == "聊天") {
                    connect_string += $" /connect:{csobj["ip"].ToString()}:{csobj["port"].ToString()} /chat";
                } else if (sec_connect_mode == "语音聊天") {
                    connect_string += $" /connect:{csobj["ip"].ToString()}:{csobj["port"].ToString()} /voice";
                } else if (sec_connect_mode == "传送讯息") {
                    connect_string += $" /connect:{csobj["ip"].ToString()}:{csobj["port"].ToString()} /message";
                }
                //if (csobj["encrypt"].ToString()=="1") connect_string += " /encrypt";
                if (csobj["fullscreen"].ToString() == "1") connect_string += " /fullscreen";
                if (csobj["nofullkbcontrol"].ToString() == "1") connect_string += " /nofullkbcontrol";

                connect_string += " /" + csobj["color_mode"];
                connect_string += " /updates:" + csobj["updates"];

                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
                Thread.Sleep(200);
                radmin_helper.auto_password(csobj["ip"].ToString(), csobj["user_name"].ToString(), csobj["user_pass"].ToString());
            } 
            else if (connect_type.IndexOf("VNC") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("vnc_exe_path");
                if (exe_path == "") return;

                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }

                //根据二次类型生成对应的字符串
                string connect_string = "";
                string sec_connect_mode = csobj["sec_connect_mode"].ToString().ToLower();
                if (sec_connect_mode == "tightvnc") {
                    connect_string += $" {csobj["ip"].ToString()}:{csobj["port"].ToString()} -password={csobj["user_pass"].ToString()}";
                } else if (sec_connect_mode == "realvnc") {
                    connect_string += $" {csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                } else if (sec_connect_mode == "ultravnc") {
                    connect_string += $" {csobj["ip"].ToString()}:{csobj["port"].ToString()} /password {csobj["user_pass"].ToString()}";
                    if (csobj["fullscreen"].ToString() == "1") connect_string += " /fullscreen";
                    if (csobj["autoreconnect"].ToString() == "1") connect_string += " /autoreconnect 10s";
                    if (csobj["viewonly"].ToString() == "1") connect_string += " /viewonly";
                }
                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
            } 
            else if (connect_type.IndexOf("Winscp") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("winscp_exe_path");
                if (exe_path == "") return;

                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }

                //根据二次类型生成对应的字符串
                string connect_string = "";
                string sec_connect_mode = csobj["sec_connect_mode"].ToString().ToLower();
                string dav_address = csobj["dav_address"].ToString();
                if (sec_connect_mode == "sftp") {
                    connect_string += $" sftp://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                } else if (sec_connect_mode == "scp") {
                    connect_string += $" scp://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                } else if (sec_connect_mode == "ftp") {
                    connect_string += $" ftp://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@{csobj["ip"].ToString()}:{csobj["port"].ToString()}";
                } else if (sec_connect_mode == "http") {
                    //eg.http://127.0.0.1:8080/webdav/
                    if (csobj["user_name"].ToString().Trim() == "") {
                        connect_string += dav_address.Replace("http", $" dav");
                    } else {
                        connect_string += dav_address.Replace("http://", $" dav://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@");
                    }
                } else if (sec_connect_mode == "https") {
                    if (csobj["user_name"].ToString().Trim() == "") {
                        connect_string += dav_address.Replace("https", $" davs");
                    } else {
                        connect_string += dav_address.Replace("https://", $" davs://{csobj["user_name"].ToString()}:{csobj["user_pass"].ToString()}@");
                    }
                }
                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
            } 
            else if (connect_type.IndexOf("SecureCrt") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("securecrt_exe_path");
                if (exe_path == "") return;

                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }

                //根据二次类型生成对应的字符串
                string connect_string = "";
                string sec_connect_mode = csobj["sec_connect_mode"].ToString().ToLower();
                if (sec_connect_mode == "ssh1") {
                    connect_string += $" /ssh1 {csobj["user_name"].ToString()}@{csobj["ip"].ToString()} /P {csobj["port"].ToString()} /PASSWORD {csobj["user_pass"].ToString()}";
                } else if (sec_connect_mode == "ssh2") {
                    connect_string += $" /ssh2 {csobj["user_name"].ToString()}@{csobj["ip"].ToString()} /P {csobj["port"].ToString()} /PASSWORD {csobj["user_pass"].ToString()}";
                } else if (sec_connect_mode == "telnet") {
                    connect_string += $" /telnet {csobj["user_name"].ToString()}@{csobj["ip"].ToString()}";
                }
                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
            } 
            else if (connect_type.IndexOf("Mobaxterm") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("mobaxterm_exe_path");
                if (exe_path == "") return;
                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }
                //根据二次类型生成对应的字符串
                string connect_string = "";
                string sec_connect_mode = csobj["sec_connect_mode"].ToString().ToLower();
                if (sec_connect_mode == "ssh") {
                    if (csobj["user_name"].ToString().Trim() == "") { ShowInfoTip($"{csobj["connect_type"].ToString()} 用户名不能为空！"); return; }
                    //密钥连接
                    if (csobj["is_ssh_rsa"].ToString().Trim() == "1") {
                        //MobaXterm.exe -newtab "ssh -i $ppk_path $Username@$host -p $port"
                        //Console.WriteLine(csobj["ssh_rsa_path"].ToString().Replace("\\", "/"));
                        connect_string += $" -newtab \"ssh -i {csobj["ssh_rsa_path"].ToString().Replace("\\","/")} {csobj["user_name"].ToString()}@{csobj["ip"].ToString()} -p {csobj["port"].ToString()}\"";
                    } else {
                        //MobaXterm.exe -newtab "sshpass -p $password ssh $Username@$host -p $port"
                        connect_string += $" -newtab \"sshpass -p {csobj["user_pass"].ToString()} ssh {csobj["user_name"].ToString()}@{csobj["ip"].ToString()} -p {csobj["port"].ToString()}\"";
                    }
                } else if (sec_connect_mode == "telnet") {
                    //.\MobaXterm1_CHS1.exe -newtab "telnet -4 192.168.152.128 22"
                    connect_string += $" -newtab \"telnet -4 {csobj["ip"].ToString()} {csobj["port"].ToString()} \"";
                }
                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
            } 
            else if (connect_type.IndexOf("Todesk") != -1) {
                //检查exe是否存在
                string exe_path = check_exe_path("todesk_exe_path");
                if (exe_path == "") return;
                //判断自定义
                if (is_user_connect_string) { common_tools.RunApp(exe_path, " " + user_connect_string); return; }
                //根据二次类型生成对应的字符串
                //.\ToDesk.exe -control -id 432160856 -passwd ipa2q65t
                string connect_string = $" -control -id {csobj["ip"].ToString().Replace(" ","")} -passwd {csobj["user_pass"].ToString()}";
                //Console.WriteLine(connect_string);
                common_tools.RunApp(exe_path, connect_string);
            }
        }
        private string check_exe_path(string store_name) {
            SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from Commom_setting where key = ?", store_name);
            bool flag = reader.Read();
            if (!flag) {
                ShowInfoTip("exe路径未设置，请到[设置]->[三方EXE位置]设置相关路径");
                return "";
            }
            string exe_path = reader["val"].ToString();
            reader.Close();
            return exe_path;
        }
        //查看密码
        private void uiSymbolButton2_Click(object sender, EventArgs e) {
            //是否使用账户体系
            if (now_csobj.ToString().Trim() == "{}") return;
            //弹出密码框
            string value = "";
            if (this.InputPasswordDialog(ref value, true, "请输入管理员密码", true)) {
                if (common_tools.md5(value.ToString()) == this.password) {
                    if (now_csobj["user_id"].ToString() != "-1") {
                        ShowInfoTip("请到账户管理里面查看该id的密码！");
                        return;
                    } else {
                        uiTextBox5.Text = now_csobj["user_pass"].ToString();
                    }
                    return;
                } else {
                    ShowErrorTip("密码错误！");
                    return;
                }
            }
        }
        //清空临时窗口
        private void uiSymbolButton1_Click(object sender, EventArgs e) {
            clear_old_info(false);
        }
        //记录窗口大小
        private void FMain_ClientSizeChanged(object sender, EventArgs e) {
            save_width = this.ClientSize.Width.ToString();
            save_height = this.ClientSize.Height.ToString();
            //Console.WriteLine(save_width+" "+save_height);
        }
        //记录窗口大小
        private void FMain_FormClosing(object sender, FormClosingEventArgs e) {
            //1150, 580
            if (save_height == "") {
                save_width = this.ClientSize.Width.ToString();
                save_height = this.ClientSize.Height.ToString();
            }

            DbInihelper.SetIniData(iniconfig_action, "save_width", save_width, iniconfig_path);
            DbInihelper.SetIniData(iniconfig_action, "save_height", save_height, iniconfig_path);
            if (is_hide_behind == "1") {
                if(tool_tip_show_once=="0") notifyIcon1.ShowBalloonTip(1500);
                this.Hide();
                e.Cancel = true;
                tool_tip_show_once = "1";
            } else {
                if (File.Exists("data/MstscManager_temp.rdp")) File.Delete("data/MstscManager_temp.rdp");
                return;
            }
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;

        //拖动事件
        private void uiDataGridView1_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        private void uiDataGridView1_MouseDown(object sender, MouseEventArgs e) {
            rowIndexFromMouseDown = uiDataGridView1.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1) {
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            } else dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void uiDataGridView1_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    DragDropEffects dropEffect = uiDataGridView1.DoDragDrop(
                    uiDataGridView1.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
            base.OnMouseMove(e);
        }

        //接受事件
        private void uiTreeView1_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        private void uiTreeView1_DragDrop(object sender, DragEventArgs e) {
            Point clientPoint = uiTreeView1.PointToClient(new Point(e.X, e.Y));
            TreeViewHitTestInfo info= uiTreeView1.HitTest(clientPoint.X, clientPoint.Y);
            if (e.Effect == DragDropEffects.Move) {
                if (info.Node == null) return;
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                TreeNode hitNode = info.Node;
                //Console.WriteLine(rowToMove.Cells[6].Value.ToString());
                //Console.WriteLine(hitNode.Text);
                drag_move_group(rowToMove.Cells[6].Value.ToString(), hitNode.Text);
            }
        }

        private void drag_move_group(string target_id,string group_name) {
            string group_id = "-1";
            if (group_name != "全部分类") {
                SqliteDataReader reader = DbSqlHelper.ExecuteReader2("select * from Group_setting where group_name = ?", group_name);
                if (reader.Read()) {
                    group_id = reader["id"].ToString();
                }
                reader.Close();
                if (group_id == "-1") return;
            }
            string connect_setting = "";
            SqliteDataReader reader1 = DbSqlHelper.ExecuteReader2("select connect_setting from Server_setting where id = ?", target_id.ToString());
            if (reader1.Read()) {
                connect_setting = reader1["connect_setting"].ToString();
            }
            reader1.Close();
            if (connect_setting == "") return;
            JObject connect_setting1 = JObject.Parse(connect_setting);
            connect_setting1["group_id"] = group_id;
            connect_setting1["group_name"] = group_name;
            connect_setting = JsonConvert.SerializeObject(connect_setting1);
            DbSqlHelper.ExecuteNonQuery2("update Server_setting set group_id = ? ,connect_setting = ? where id = ? ", group_id, connect_setting, target_id);
            ShowSuccessTip("移动成功！");
            refresh_server_table();
        }
        // 当收到第二个进程的通知时，显示气球消息
        void OnProgramStarted(object state, bool timeout) {
            ShowSuccessTip("当前程序正在运行");
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Environment.Exit(0);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.Show();
        }

        internal void set_hide_behind(string status) {
            is_hide_behind = status;
        }


    }
}
