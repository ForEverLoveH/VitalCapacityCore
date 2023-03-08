namespace PLADCore.GameSystem.GameWindow
{
    partial class LoadingWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.uchScrollbar1 = new HZH_Controls.Controls.UCHScrollbar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // uchScrollbar1
            // 
            this.uchScrollbar1.BtnWidth = 18;
            this.uchScrollbar1.ConerRadius = 2;
            this.uchScrollbar1.FillColor = System.Drawing.Color.White;
            this.uchScrollbar1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.uchScrollbar1.IsRadius = true;
            this.uchScrollbar1.IsShowRect = true;
            this.uchScrollbar1.LargeChange = 10;
            this.uchScrollbar1.Location = new System.Drawing.Point(13, 14);
            this.uchScrollbar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uchScrollbar1.Maximum = 500;
            this.uchScrollbar1.Minimum = 0;
            this.uchScrollbar1.MinimumSize = new System.Drawing.Size(0, 10);
            this.uchScrollbar1.Name = "uchScrollbar1";
            this.uchScrollbar1.RectColor = System.Drawing.Color.White;
            this.uchScrollbar1.RectWidth = 1;
            this.uchScrollbar1.Size = new System.Drawing.Size(659, 89);
            this.uchScrollbar1.SmallChange = 50;
            this.uchScrollbar1.TabIndex = 3;
            this.uchScrollbar1.ThumbColor = System.Drawing.Color.ForestGreen;
            this.uchScrollbar1.Value = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LoadingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 121);
            this.Controls.Add(this.uchScrollbar1);
            this.Name = "LoadingWindow";
            this.Text = "加载页面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadingWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoadingWindow_FormClosed);
            this.Load += new System.EventHandler(this.LoadingWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HZH_Controls.Controls.UCHScrollbar uchScrollbar1;
        private System.Windows.Forms.Timer timer1;
    }
}