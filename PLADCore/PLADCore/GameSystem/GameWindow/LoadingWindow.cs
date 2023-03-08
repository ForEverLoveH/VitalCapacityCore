using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLADCore.GameSystem.GameWindow
{
    public partial class LoadingWindow : Form
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }
        int val = 0;
        private void LoadingWindow_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            val += 10;
            if (val >= 500) val = 0;
            uchScrollbar1.Value= val;
        }

        private void LoadingWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (timer1.Enabled == true)
                timer1.Stop();
        }

        private void LoadingWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1.Enabled == true)
                timer1.Stop();
        }
    }
}
