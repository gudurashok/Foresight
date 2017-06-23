using System;

namespace Scalable.Win.Common
{
    public class Stopwatch
    {
        public readonly DateTime StartTime;

        public Stopwatch()
        {
            StartTime = DateTime.Now;
        }

        public TimeSpan Stop()
        {
            return DateTime.Now.Subtract(StartTime);
        }
    }
}
