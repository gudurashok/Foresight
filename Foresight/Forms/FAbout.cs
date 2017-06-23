using System;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace ScalableApps.Foresight.Win.Forms
{
    public partial class FAbout : Form
    {
        public FAbout()
        {
            InitializeComponent();
        }

        private void FAbout_Load(object sender, EventArgs e)
        {
            var asm = Assembly.GetExecutingAssembly();
            var copyright = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(asm,
                            typeof(AssemblyCopyrightAttribute));
            lblCopyright.Text = copyright.Copyright;

            var version = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(asm,
                            typeof(AssemblyFileVersionAttribute));
            lblVersion.Text = string.Format("Version: {0}", version.Version);
        }

        private void FAbout_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void FAbout_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }

        private void lnkiScalableWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.iscalable.com");
        }
    }
}
