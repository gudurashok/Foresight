using Microsoft.VisualBasic.ApplicationServices;
using ScalableApps.Foresight.Win.Forms;

namespace ScalableApps.Foresight.Win.Common
{
    internal class ForesightApplication : WindowsFormsApplicationBase
    {
        protected override void OnCreateSplashScreen()
        {
            SplashScreen = new FSplash();
        }

        protected override void OnCreateMainForm()
        {
            MainForm = new FLogin();
        }
    }
}
