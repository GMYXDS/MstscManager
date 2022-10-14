namespace MstscManager.Controls {
    partial class Add_securecrt {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.uiRadioButton3 = new Sunny.UI.UIRadioButton();
            this.uiRadioButton2 = new Sunny.UI.UIRadioButton();
            this.uiRadioButton1 = new Sunny.UI.UIRadioButton();
            this.uiGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.uiRadioButton3);
            this.uiGroupBox2.Controls.Add(this.uiRadioButton2);
            this.uiGroupBox2.Controls.Add(this.uiRadioButton1);
            this.uiGroupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiGroupBox2.Location = new System.Drawing.Point(9, 6);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(510, 300);
            this.uiGroupBox2.TabIndex = 13;
            this.uiGroupBox2.Text = "额外信息";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiGroupBox2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiRadioButton3
            // 
            this.uiRadioButton3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiRadioButton3.Location = new System.Drawing.Point(14, 105);
            this.uiRadioButton3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRadioButton3.Name = "uiRadioButton3";
            this.uiRadioButton3.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.uiRadioButton3.Size = new System.Drawing.Size(120, 29);
            this.uiRadioButton3.TabIndex = 2;
            this.uiRadioButton3.Text = "Telnet模式";
            this.uiRadioButton3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiRadioButton3.Click += new System.EventHandler(this.uiRadioButton3_Click);
            // 
            // uiRadioButton2
            // 
            this.uiRadioButton2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiRadioButton2.Location = new System.Drawing.Point(14, 70);
            this.uiRadioButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRadioButton2.Name = "uiRadioButton2";
            this.uiRadioButton2.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.uiRadioButton2.Size = new System.Drawing.Size(120, 29);
            this.uiRadioButton2.TabIndex = 1;
            this.uiRadioButton2.Text = "SSH2模式";
            this.uiRadioButton2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiRadioButton2.Click += new System.EventHandler(this.uiRadioButton2_Click);
            // 
            // uiRadioButton1
            // 
            this.uiRadioButton1.Checked = true;
            this.uiRadioButton1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiRadioButton1.Location = new System.Drawing.Point(14, 35);
            this.uiRadioButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRadioButton1.Name = "uiRadioButton1";
            this.uiRadioButton1.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.uiRadioButton1.Size = new System.Drawing.Size(120, 29);
            this.uiRadioButton1.TabIndex = 0;
            this.uiRadioButton1.Text = "SSH1模式";
            this.uiRadioButton1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiRadioButton1.Click += new System.EventHandler(this.uiRadioButton1_Click);
            // 
            // Add_securecrt
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(520, 306);
            this.Controls.Add(this.uiGroupBox2);
            this.MaximumSize = new System.Drawing.Size(520, 306);
            this.Name = "Add_securecrt";
            this.ReceiveParams += new Sunny.UI.OnReceiveParams(this.Add_securecrt_ReceiveParams);
            this.uiGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox2;
        private Sunny.UI.UIRadioButton uiRadioButton3;
        private Sunny.UI.UIRadioButton uiRadioButton2;
        private Sunny.UI.UIRadioButton uiRadioButton1;
    }
}
