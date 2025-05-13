using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Hangfire.Manager;
using Entities;
using Hangfire;

namespace Business.Hangfire.Jobs
{
    public class BackGroundSchedule
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public BackGroundSchedule(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager;
        }

        public void ScheduleRecurringJobs()
        {
            _recurringJobManager.AddOrUpdate<GetForex>(
                "example-job",
                job => job.Run(),
                "0 */2 * * *", // 2 saatte bir
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Local
                }
            );
        }
    }
}
