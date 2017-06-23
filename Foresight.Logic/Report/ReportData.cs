namespace ScalableApps.Foresight.Logic.Report
{
    public class ReportData
    {
        public object Result { get; private set; }

        public ReportData(object result)
        {
            Result = result;
        }
    }
}
