using System;

namespace ScalableApps.Foresight.Win.Common
{
    public class StopWatch
    {
        public readonly DateTime StartTime;

        public StopWatch()
        {
            StartTime = DateTime.Now;
        }

        public TimeSpan Stop()
        {
            return DateTime.Now.Subtract(StartTime);
        }
    }
}
