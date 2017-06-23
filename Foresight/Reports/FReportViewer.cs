using System.Drawing;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.DataAccess;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Reports
{
    public partial class FReportViewer : Form
    {
        public FReportViewer(UReportBase report)
        {
            InitializeComponent();
            AddReportControl(report);
        }

        public void AddReportControl(UReportBase report)
        {
            Controls.Add(report);
            report.BringToFront();
            report.Dock = DockStyle.Fill;
            report.Show();
        }

        public static void ShowLedger(Form owner, Daybook jvDaybook)
        {
            var command = ForesightDatabaseFactory.GetInstance().GetCommandByNr(Constants.LedgerReportNr);
            var summary = new ULedgerSummaryReport(command, jvDaybook);
            var detail = new ULedgerDetailReport(command, jvDaybook);
            show(owner, summary, detail);
        }

        public static void ShowLedger(Form owner, Account account)
        {
            var command = ForesightDatabaseFactory.GetInstance().GetCommandByNr(Constants.LedgerReportNr);
            var summary = new ULedgerSummaryReport(command, account);
            var detail = new ULedgerDetailReport(command, account);
            show(owner, summary, detail);
        }

        private static void show(Form owner, ULedgerSummaryReport summary, ULedgerDetailReport detail)
        {
            detail.SummaryControl = summary;
            summary.DetailControl = detail;
            detail.Enabled = false;

            var viewer = new FReportViewer(detail);
            viewer.Size = new Size(742, 480);
            viewer.AddReportControl(summary);
            viewer.Show(owner);
        }
    }
}
