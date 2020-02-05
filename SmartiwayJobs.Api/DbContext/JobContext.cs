using SmartiwayJobs.Api.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SmartiwayJobs.Api.DbContext
{
    public class JobContext : IJobContext
    {
        private volatile List<Job> jobs;

        public List<Job> Jobs
        {
            get
            {
                return jobs.OrderBy(x => x.Priority).ToList();
            }
            set
            {
                jobs = value.ToList();
            }
        }

        public JobContext()
        {
            jobs = new List<Job>()
            {
                new Job{Priority = 1, Name = "Write high-quality code"},
                new Job{Priority = 2, Name = "Read book about Angular"},
                new Job{Priority = 3, Name = "Have a short vacation"},
            };
        }
    }
}
