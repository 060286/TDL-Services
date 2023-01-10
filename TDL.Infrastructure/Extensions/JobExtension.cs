using Hangfire;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDL.Infrastructure.Extensions
{
    public static class JobExtension
    {
        public static bool ExeedAttempts(this PerformContext context)
        {
            var retryCount = context.GetJobParameter<int>("RetryCount");

            var attemps = context.BackgroundJob.Job.Method.GetCustomAttributes(typeof(AutomaticRetryAttribute), false)
                .Cast<AutomaticRetryAttribute>()
                .Select(a => a.Attempts)
                .FirstOrDefault();

            return retryCount == attemps;
        }
    }
}
