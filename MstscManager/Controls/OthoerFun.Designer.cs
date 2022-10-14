namespace MstscManager.Controls {
    partial class OthoerFun {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.uiButton5 = new Sunny.UI.UIButton();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // uiButton5
            // 
            this.uiButton5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiButton5.Location = new System.Drawing.Point(1, 3);
            this.uiButton5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton5.Name = "uiButton5";
            this.uiButton5.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.uiButton5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uiButton5.Size = new System.Drawing.Size(89, 35);
            this.uiButton5.TabIndex = 2;
            this.uiButton5.Text = "导入本机mstsc";
            this.uiButton5.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiButton5.Click += new System.EventHandler(this.uiButton5_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiButton1.Location = new System.Drawing.Point(1, 44);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.uiButton1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uiButton1.Size = new System.Drawing.Size(89, 35);
            this.uiButton1.TabIndex = 3;
            this.uiButton1.Text = "账户管理";
            this.uiButton1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // OthoerFun
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiButton1);
            this.Controls.Add(this.uiButton5);
            this.MaximumSize = new System.Drawing.Size(89, 85);
            this.Name = "OthoerFun";
            this.Size = new System.Drawing.Size(89, 85);
            this.ResumeLayout(false);

        }

        #endregion
        private Sunny.UI.UIButton uiButton5;
        private Sunny.UI.UIButton uiButton1;
    }
}
