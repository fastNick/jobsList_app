using AutoMapper;
using SmartiwayJobs.Api.Entities;
using SmartiwayJobs.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartiwayJobs.Api.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Entities.Job, Models. JobDto>();
            CreateMap<Models.JobForCreationDto, Entities.Job>();
        }
    }
}
