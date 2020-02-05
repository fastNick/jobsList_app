using SmartiwayJobs.Api.Entities;
using SmartiwayJobs.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartiwayJobs.Api.Services
{
    public interface IJobRepository
    {
        IEnumerable<Job> GetJobs();
        Job GetJob(int jobPriority);
        void AddJob(Job job);
        void DeleteJob(Job job);
        Job IncrementJobPriority(Job job);
        Job DecrementJobPriority(Job Job);
        Job SetJobPriority(Job job, int newJobPriority);
    }
}
