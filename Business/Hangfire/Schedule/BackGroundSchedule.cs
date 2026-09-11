using Business.Hangfire.Manager;
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
