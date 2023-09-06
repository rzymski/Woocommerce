using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PosnetServerWinFormsApp
{
    public partial class SystemTray : Form
    {
        public SystemTray()
        {
            InitializeComponent();
            this.MinimizeBox = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            //Console.WriteLine("\nDZIALA\n");
            Environment.Exit(0);
        }

        private void SystemTray_Move(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Minimized)
            //{
            //    this.Hide();
            //    notifyIcon1.ShowBalloonTip(1000, "Wazna wiadomosc", "Cos waznego, Kliknij zeby wiedziec wicej", ToolTipIcon.Info);
            //}
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //this.Show();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void SystemTray_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
        }

        private void SystemTray_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }

        private void SystemTray_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
