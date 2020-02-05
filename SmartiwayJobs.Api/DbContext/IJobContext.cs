using SmartiwayJobs.Api.Entities;
using System.Collections.Generic;

namespace SmartiwayJobs.Api.DbContext
{
    public interface IJobContext
    {
        List<Job> Jobs { get; set; }
    }
}
