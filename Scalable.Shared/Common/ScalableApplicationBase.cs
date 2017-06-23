using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.IO;
using System.Collections.ObjectModel;

namespace Scalable.Shared.Common
{
    public abstract class ScalableApplicationBase : WindowsFormsApplicationBase
    {
        //public GravityApplication()
        //{
        //    this.ApplicationContext.ExitThread
        //    this.ApplicationContext.MainForm
        //    this.ApplicationContext.Tag
        //    this.ApplicationContext.ThreadExit
        //    this.CommandLineArgs;
        //    this.Culture;
        //    this.ChangeCulture
        //    this.ChangeUICulture
        //    this.Deployment
        //    this.DoEvents;
        //    this.EnableVisualStyles
        //    this.GetEnvironmentVariable
        //    this.HideSplashScreen
        //    this.Info
        //    this.InternalCommandLine.
        //    this.IsNetworkDeployed --?
        //    this.IsSingleInstance -- ?
        //    this.Log -- ?
        //    this.MainForm
        //    this.MinimumSplashScreenDisplayTime
        //    this.NetworkAvailabilityChanged --?
        //    this.OnCreateMainForm;
        //    this.OnCreateSplashScreen;
        //    this.OnInitialize
        //    this.OpenForms
        //    this.Run
        //    this.ShutdownStyle
        //    this.SaveMySettingsOnExit
        //    this.SplashScreen
        //    this.UICulture

        //}

        protected override bool OnInitialize(ReadOnlyCollection<string> commandLineArgs)
        {
            var configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            if (!File.Exists(configFile))
            {
                MessageBox.Show(string.Format("Cannot find application config file: \n{0}", configFile));
                return false;
            }

            return base.OnInitialize(commandLineArgs);
        }

        //protected override bool OnStartup(StartupEventArgs eventArgs)
        //{
        //    //if Application.ExecutablePath

        //    MessageBox.Show(string.Format("OnStartup  Args{0}", eventArgs.CommandLine.Count));
        //    return base.OnStartup(eventArgs);
        //}

        //protected override void OnRun()
        //{
        //    MessageBox.Show("OnRun");
        //    base.OnRun();
        //}

        //protected override void OnShutdown()
        //{
        //    MessageBox.Show("OnShutdown");
        //    base.OnShutdown();
        //}

        //protected override bool OnUnhandledException(UnhandledExceptionEventArgs e)
        //{
        //    MessageBox.Show("OnUnhandledException");
        //    return base.OnUnhandledException(e);
        //}

        //protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        //{
        //    MessageBox.Show("OnStartupNextInstance");
        //    base.OnStartupNextInstance(eventArgs);
        //}
    }
}
