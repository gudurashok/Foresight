using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Scalable.Shared.Connection;
using Scalable.Shared.DataAccess;
using Scalable.Shared.Enums;
using Scalable.Shared.Properties;

namespace Scalable.Shared.Common
{
    public static class ScalableUtil
    {
        public static object ConvertToDbValue(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
                return DBNull.Value;

            return value;
        }

        public static object ConvertDbNull(object value)
        {
            return DBNull.Value == value ? null : value;
        }

        public static decimal GetAmountFormatValue(AmountFormatStyle format)
        {
            if (format == AmountFormatStyle.Crores) return 10000000;
            if (format == AmountFormatStyle.Lacs) return 100000;
            if (format == AmountFormatStyle.Thousands) return 1000;
            return 1;
        }

        public static void ExecuteEventHandler(EventHandler handler, object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                handler(sender, e);
            }
            catch (ValidationException ex)
            {
                ShowMessage(ex.Message);
                SetFocusToControl(sender, ex.PropertyName);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public static void SetFocusToControl(object sender, string propertyName)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                return;

            var control = sender as Control;
            if (control == null)
                return;

            var form = control.FindForm();

            //var result = (from c in form.Descendants<Control>()
            //              where c.Tag != null && c.Tag.ToString().Equals(propertyName)
            //              select c).FirstOrDefault();

            var result = form.Descendants<Control>()
                .FirstOrDefault(c => c.Tag != null && c.Tag.ToString().Equals(propertyName));

            if (result == null)
                return;

            result.Focus();
        }

        public static IEnumerable<T> Descendants<T>(this Control control) where T : class
        {
            foreach (Control child in control.Controls)
            {
                T childOfT = child as T;
                if (childOfT != null)
                    yield return childOfT;

                if (child.HasChildren)
                    foreach (T descendant in Descendants<T>(child))
                        yield return descendant;
            }
        }

        //public static Control GetControl(Control.ControlCollection controls, Predicate<Control> match)
        //{
        //    //USAGE:
        //    //Control control = GetControl(this.Controls, ctl => ctl.TabIndex == 9);
        //    //Control control = GetControl(this.Controls, ctl => ctl.Text == "Some text");

        //    foreach (Control control in controls)
        //    {
        //        if (match(control)) return control;

        //        if (control.Controls.Count > 0)
        //        {
        //            Control result = GetControl(control.Controls, match);
        //            if (result != null) return result;
        //        }
        //    }

        //    return null;
        //}

        public static string[] GetEnumDescriptions(Type enumType)
        {
            var fieldInfos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            return (from fieldInfo in fieldInfos
                    let attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    select attributes.Length > 0 ? attributes[0].Description : fieldInfo.GetValue(fieldInfo.Name).ToString()).ToArray();
        }

        public static string GetEnumDescription(object enumValue)
        {
            var enumType = enumValue.GetType();
            var fieldInfos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var fieldInfo in fieldInfos)
            {
                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length <= 0)
                    return fieldInfo.GetValue(fieldInfo.Name).ToString();

                if (enumValue.ToString().Equals(fieldInfo.GetValue(fieldInfo.Name).ToString()))
                    return attributes[0].Description;
            }
            return "";
        }

        public static List<KeyValuePair<int, string>> GetEnumList(Type enumType)
        {
            var values = Enum.GetValues(enumType);
            var descriptions = GetEnumDescriptions(enumType);
            return values.Cast<object>().Select((t, index) => new KeyValuePair<int, string>((int)values.GetValue(index), descriptions[index])).ToList();
        }

        public static void LoadEnumListItems(ListControl control, Type enumType, object defaultValue)
        {
            LoadEnumListItems(control, enumType);
            control.SelectedValue = defaultValue;
        }

        public static void LoadEnumListItems(ListControl control, Type enumType)
        {
            bindListControl(control, "Key", "Value", GetEnumList(enumType));
        }

        private static void bindListControl(ListControl control, string valueMember,
                                            string displayMember, object dataSource)
        {
            control.ValueMember = valueMember;
            control.DisplayMember = displayMember;
            control.DataSource = dataSource;
        }

        public static void ShowMessage(string message)
        {
            ShowMessageBox(message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowError(Exception exception)
        {
#if DEBUG
            ShowMessageBox(exception.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#else
            ShowMessageBox(exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
        }

        public static DialogResult GetConfirmationOKCancel(string message)
        {
            return GetConfirmation(message, MessageBoxButtons.OKCancel);
        }

        public static DialogResult GetConfirmationYesNo(string message)
        {
            return GetConfirmation(message, MessageBoxButtons.YesNo);
        }

        private static DialogResult GetConfirmation(string message,
                                                    MessageBoxButtons buttons)
        {
            return ShowMessageBox(message, buttons, MessageBoxIcon.Question);
        }

        public static DialogResult ShowMessageBox(string message,
                                                  MessageBoxButtons buttons,
                                                  MessageBoxIcon icon)
        {
            return MessageBox.Show(message, Constants.AppName,
                                   buttons, icon, MessageBoxDefaultButton.Button1);
        }

        public static void ProcessException(Exception exp)
        {
            if (exp is ValidationException)
                ShowMessage(exp.Message);
            else
                ShowError(exp);
        }

        public static void SelectListItem(ListView lvw, int index)
        {
            SelectListItem(lvw, index, false);
        }

        public static void SelectListItem(ListView lvw, int index, bool setFocus)
        {
            if (lvw.Items.Count == 0)
                return;

            SelectListItem(lvw.Items[index]);

            if (!setFocus)
                return;

            lvw.Focus();
        }

        public static void SelectListItem(ListViewItem lvi)
        {
            lvi.Selected = true;
            lvi.EnsureVisible();
        }

        public static int GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        public static IEnumerable<Process> GetExecutingProcesses()
        {
            return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
        }

        public static decimal GetAmountFormatValue(ListControl control)
        {
            return GetAmountFormatValue((AmountFormatStyle)(control.SelectedValue));
        }

        public static string GetAppPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static Assembly GetSqlCeAssembly()
        {
#if DEBUG
            return Assembly.LoadFrom(getSqlCeAssemblyPath());
#else
            return Assembly.LoadFrom(string.Format(@"{0}\Private\System.Data.SqlServerCe.dll", GetAppPath()));
#endif
        }

#if DEBUG

        private static string getSqlCeAssemblyPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                    @"\Microsoft SQL Server Compact Edition\v4.0\Desktop\System.Data.SqlServerCe.dll";

        }
#endif

        public static void TestDatabaseConnection(IDbConnectionInfo sqlConnInfo)
        {
            new SqlDbManager(DbConnectionFactory.GetConnection(
                DatabaseProvider.SqlServer, sqlConnInfo.GetConnectionString()));
        }

        public static DatabaseProvider GetAppDatabaseProvider()
        {
            var genus = GetAppGenus();

            switch (genus)
            {
                case Genus.Cheetah:
                    return DatabaseProvider.SqlCe;
                case Genus.Lion:
                    return DatabaseProvider.SqlServer;
                default:
                    throw new ValidationException(String.Format(Resources.GenusNotSupported, genus));
            }
        }

        public static Genus GetAppGenus()
        {
            var genus = ConfigurationManager.AppSettings.Get("Genus");

            switch (genus)
            {
                case "Cheetah":
                    return Genus.Cheetah;
                case "Lion":
                    return Genus.Lion;
                default:
                    throw new ValidationException(String.Format("Incorrect Genus {0}", (object) genus));
            }
        }

        public static string GetAppGenusLionValue()
        {
            return ConfigurationManager.AppSettings.Get("Lion");
        }

        public static string GetAppDataPath()
        {
            return ConfigurationManager.AppSettings.Get("DataPath");
        }
    }
}
