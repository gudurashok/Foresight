using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using ScalableApps.Foresight.Win.Properties;
using System.Diagnostics;
using ScalableApps.Foresight.Logic.Report;

namespace ScalableApps.Foresight.Win.Common
{
    public static class Utilities
    {
        public static string[] GetEnumDescriptions(Type enumType)
        {
            var fieldInfos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            return (from fieldInfo in fieldInfos
                    let attributes = (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false)
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
            return values.Cast<object>().Select((t, index) => new KeyValuePair<int, string>((int) values.GetValue(index), descriptions[index])).ToList();
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
            showMessageBox(message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowError(Exception exception)
        {
            ShowError(exception, false);
        }

        public static void ShowError(Exception exception, bool ignoreLog)
        {
            if (!ignoreLog)
                LogError(exception);

#if DEBUG
            showMessageBox(exception.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#else
            showMessageBox(exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            return showMessageBox(message, buttons, MessageBoxIcon.Question);
        }

        private static DialogResult showMessageBox(string message,
                                                    MessageBoxButtons buttons,
                                                    MessageBoxIcon icon)
        {
            return MessageBox.Show(message, Constants.AppName,
                                    buttons, icon, MessageBoxDefaultButton.Button1);
        }

        public static void LogError(Exception exception)
        {
            try
            {
                ForesightDatabaseFactory.GetInstance().InsertError(
                    new ErrorMessage
                        {
                            DateTime = DateTime.Now,
                            Text = exception.ToString()
                        });
            }
            catch (Exception ex)
            {

#if DEBUG
                showMessageBox(string.Format("{0}. Original Exception: {1}",
                                        Resources.CannotLogException, ex),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
#else
                showMessageBox(string.Format("{0}. Message: {1}", 
                                        Resources.CannotLogException, ex.Message),
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
#endif
            }
        }

        public static void ProcessException(Exception exp)
        {
            if (exp is ValidationException)
                ShowMessage(exp.Message);
            else
                ShowError(exp);
        }

        public static void SelectListItem(ListView control, int index)
        {
            SelectListItem(control, index, false);
        }

        public static void SelectListItem(ListView control, int index, bool setFocus)
        {
            if (control.Items.Count == 0)
                return;

            control.Items[index].Selected = true;

            if (!setFocus) 
                return;

            control.EnsureVisible(index);
            control.Focus();
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
            return Util.GetAmountFormatValue((ReportsAmountFormat)(control.SelectedValue));
        }
    }
}
