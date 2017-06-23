using System;
using System.Windows.Forms;
using System.Reflection;

namespace ScalableApps.Foresight.Win.Forms
{
    public partial class FSplash : Form
    {
        public FSplash()
        {
            InitializeComponent();
        }

        private void FSplash_Load(object sender, EventArgs e)
        {
            var asm = Assembly.GetExecutingAssembly();
            var version = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(asm,
                                                    typeof(AssemblyFileVersionAttribute));
            lblVersion.Text = string.Format("Version: {0}", version.Version);
        }
    }
}
