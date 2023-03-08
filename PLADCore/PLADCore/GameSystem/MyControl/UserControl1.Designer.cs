namespace PLADCore.GameSystem.MyControl
{
    partial class UserControl1
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.stateCbx = new System.Windows.Forms.ComboBox();
            this.roundCbx = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolState = new System.Windows.Forms.ToolStripStatusLabel();
            this.mScore = new Sunny.UI.UILabel();
            this.mName = new Sunny.UI.UILabel();
            this.mIdNumber = new Sunny.UI.UILabel();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.mtitle = new Sunny.UI.UILabel();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stateCbx);
            this.panel1.Controls.Add(this.roundCbx);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.mScore);
            this.panel1.Controls.Add(this.mName);
            this.panel1.Controls.Add(this.mIdNumber);
            this.panel1.Controls.Add(this.uiLabel5);
            this.panel1.Controls.Add(this.uiLabel4);
            this.panel1.Controls.Add(this.uiLabel3);
            this.panel1.Controls.Add(this.uiLabel2);
            this.panel1.Controls.Add(this.uiLabel1);
            this.panel1.Controls.Add(this.mtitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(152, 272);
            this.panel1.TabIndex = 1;
            // 
            // stateCbx
            // 
            this.stateCbx.FormattingEnabled = true;
            this.stateCbx.Items.AddRange(new object[] {
            "未测试",
            "已测试",
            "中退",
            "缺考",
            "犯规",
            "弃权"});
            this.stateCbx.Location = new System.Drawing.Point(72, 208);
            this.stateCbx.Name = "stateCbx";
            this.stateCbx.Size = new System.Drawing.Size(72, 20);
            this.stateCbx.TabIndex = 13;
            // 
            // roundCbx
            // 
            this.roundCbx.FormattingEnabled = true;
            this.roundCbx.Location = new System.Drawing.Point(72, 173);
            this.roundCbx.Name = "roundCbx";
            this.roundCbx.Size = new System.Drawing.Size(72, 20);
            this.roundCbx.TabIndex = 12;
           // this.roundCbx.SelectedIndexChanged += new System.EventHandler(this.roundCbx_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 250);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(152, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolState
            // 
            this.toolState.ForeColor = System.Drawing.Color.Red;
            this.toolState.Name = "toolState";
            this.toolState.Size = new System.Drawing.Size(68, 17);
            this.toolState.Text = "设备未连接";
            // 
            // mScore
            // 
            this.mScore.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mScore.Location = new System.Drawing.Point(76, 126);
            this.mScore.Name = "mScore";
            this.mScore.Size = new System.Drawing.Size(68, 23);
            this.mScore.TabIndex = 8;
            this.mScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mScore.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // mName
            // 
            this.mName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mName.Location = new System.Drawing.Point(72, 82);
            this.mName.Name = "mName";
            this.mName.Size = new System.Drawing.Size(72, 23);
            this.mName.TabIndex = 7;
            this.mName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mName.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // mIdNumber
            // 
            this.mIdNumber.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mIdNumber.Location = new System.Drawing.Point(72, 48);
            this.mIdNumber.Name = "mIdNumber";
            this.mIdNumber.Size = new System.Drawing.Size(72, 23);
            this.mIdNumber.TabIndex = 6;
            this.mIdNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mIdNumber.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel5
            // 
            this.uiLabel5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel5.Location = new System.Drawing.Point(13, 203);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(65, 23);
            this.uiLabel5.TabIndex = 5;
            this.uiLabel5.Text = "状态：";
            this.uiLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uiLabel5.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel4
            // 
            this.uiLabel4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel4.Location = new System.Drawing.Point(7, 168);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(71, 23);
            this.uiLabel4.TabIndex = 4;
            this.uiLabel4.Text = "轮次：";
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uiLabel4.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.Location = new System.Drawing.Point(3, 126);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(75, 23);
            this.uiLabel3.TabIndex = 3;
            this.uiLabel3.Text = "成绩：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uiLabel3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(7, 82);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(75, 23);
            this.uiLabel2.TabIndex = 2;
            this.uiLabel2.Text = "姓名：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uiLabel2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(3, 48);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(79, 23);
            this.uiLabel1.TabIndex = 1;
            this.uiLabel1.Text = "考号：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // mtitle
            // 
            this.mtitle.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.mtitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mtitle.Location = new System.Drawing.Point(6, 0);
            this.mtitle.Name = "mtitle";
            this.mtitle.Size = new System.Drawing.Size(141, 23);
            this.mtitle.TabIndex = 0;
            this.mtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mtitle.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(152, 272);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private Sunny.UI.UILabel mtitle;
        private Sunny.UI.UILabel mIdNumber;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolState;
        private Sunny.UI.UILabel mScore;
        private Sunny.UI.UILabel mName;
        private System.Windows.Forms.ComboBox stateCbx;
        private System.Windows.Forms.ComboBox roundCbx;
    }
}
