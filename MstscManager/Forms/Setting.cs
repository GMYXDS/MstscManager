using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MstscManager.Controls;

namespace MstscManager.Forms {
    public partial class Setting : UIAsideMainFrame {
        public Setting() {
            InitializeComponent();
            int pageIndex = 1000;

            pageIndex++;
            AddPage(new Common_setting(), pageIndex);
            Aside.CreateNode("常规", pageIndex);
            SelectPage(pageIndex);

            pageIndex++;
            AddPage(new Change_pass(), pageIndex);
            Aside.CreateNode("更改超级密码", pageIndex);

            pageIndex++;
            AddPage(new Set_exe_directory(), pageIndex);
            Aside.CreateNode("三方EXE位置", pageIndex);

            pageIndex++;
            AddPage(new About(), pageIndex);
            Aside.CreateNode("关于", pageIndex);
        }
    }
}
