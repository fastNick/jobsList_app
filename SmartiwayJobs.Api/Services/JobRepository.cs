using SmartiwayJobs.Api.DbContext;
using SmartiwayJobs.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SmartiwayJobs.Api.Services
{
    public class JobRepository : IJobRepository
    {
        private readonly IJobContext _context;

        private readonly object objLock = new object();

        public JobRepository(IJobContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void AddJob(Job job)
        {
            if(job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            if (!_context.Jobs.Any())
            {
                var jobs = _context.Jobs;
                jobs.Add(job);
                _context.Jobs = jobs;
            }
            else
            {
                var lastJob = _context.Jobs.Last();
                job.Priority = lastJob.Priority + 1;                
                var jobs = _context.Jobs;
                jobs.Add(job);
                _context.Jobs = jobs;
            }
            
        }

        public Job DecrementJobPriority(Job job)
        {
            if(job.Priority == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(job));
            }
            lock(objLock) {
                var oldJobPriority = job.Priority;                
                var onePriorityFewerJob = _context.Jobs.FirstOrDefault(x => x.Priority == oldJobPriority-1);
                if (onePriorityFewerJob != null)
                {
                    onePriorityFewerJob.Priority = oldJobPriority;
                }
                job.Priority--;
            }            
            return job;
        }

        public void DeleteJob(Job job)
        {
            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            var jobs = _context.Jobs;
            jobs.Remove(job);
            _context.Jobs = jobs;
        }

        public Job GetJob(int jobPriority)
        {
            return _context.Jobs.FirstOrDefault(j => j.Priority == jobPriority);
        }

        public IEnumerable<Job> GetJobs()
        {
            return _context.Jobs.ToList();
        }

        public Job IncrementJobPriority(Job job)
        {
            if (job.Priority == int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(job));
            }
            lock (objLock)
            {
                var oldJobPriority = job.Priority;                
                var onePriorityMoreJob = _context.Jobs.FirstOrDefault(x => x.Priority == oldJobPriority+1);
                if (onePriorityMoreJob != null)
                {
                    onePriorityMoreJob.Priority = oldJobPriority;
                }
                job.Priority++;
            }            
            return job;
        }
        
        public Job SetJobPriority(Job job, int newJobPriority)
        {            
            foreach(var updateJob in _context.Jobs.Where(j => j.Priority >= newJobPriority && j.Priority != job.Priority))
            {
                updateJob.Priority++;
            }
            job.Priority = newJobPriority;
            return job;
        }
    }
}
