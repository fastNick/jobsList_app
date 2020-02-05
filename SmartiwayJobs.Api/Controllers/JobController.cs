using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartiwayJobs.Api.Entities;
using SmartiwayJobs.Api.Models;
using SmartiwayJobs.Api.Services;
using System;
using System.Collections.Generic;

namespace SmartiwayJobs.Api.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public JobController(IJobRepository jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
                
        [HttpGet]
        public ActionResult<IEnumerable<JobDto>> GetJobs()
        {
            var jobsFromRepo = _jobRepository.GetJobs();
            return Ok(_mapper.Map<IEnumerable<JobDto>>(jobsFromRepo));
        }

        [HttpGet("{jobPriority}", Name = "GetJob")]
        public ActionResult<JobDto> GetJob(int jobPriority)
        {
            var jobFromRepo = _jobRepository.GetJob(jobPriority);
            if (jobFromRepo == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<JobDto>(jobFromRepo));
        }

        [HttpPost]
        public ActionResult<JobDto> CreateJob(JobForCreationDto job)
        {
            if (String.IsNullOrEmpty(job.Name))
            {
                return BadRequest();
            }
            var jobEntity = _mapper.Map<Job>(job);
            _jobRepository.AddJob(jobEntity);
            
            var jobToReturn = _mapper.Map<JobDto>(jobEntity);
            return CreatedAtRoute("GetJob", new { jobPriority = jobToReturn.Priority }, jobToReturn);
        }

        [HttpPut("[action]/{jobPriority}")]
        public ActionResult<JobDto> IncrementJobPriority(int jobPriority)
        {
            var jobFromRepo = _jobRepository.GetJob(jobPriority);

            if (jobFromRepo == null)
            {
                return NotFound();
            }            

            var incrementedPriorityJobEntity = _jobRepository.IncrementJobPriority(jobFromRepo);

            var jobToReturn = _mapper.Map<JobDto>(incrementedPriorityJobEntity);

            return CreatedAtRoute("GetJob", new { jobPriority = jobToReturn.Priority }, jobToReturn);
        }

        [HttpPut("[action]/{jobPriority}")]
        public ActionResult<JobDto> DecrementJobPriority(int jobPriority)
        {
            var jobFromRepo = _jobRepository.GetJob(jobPriority);

            if (jobFromRepo == null)
            {
                return NotFound();
            }

            var decrementedPriorityJobEntity = _jobRepository.DecrementJobPriority(jobFromRepo);

            var jobToReturn = _mapper.Map<JobDto>(decrementedPriorityJobEntity);

            return CreatedAtRoute("GetJob", new { jobPriority = jobToReturn.Priority }, jobToReturn);
        }

        [HttpPut("[action]/{oldJobPriority}")]
        public ActionResult<JobDto> SetJobPriority(int oldJobPriority, [FromBody] int newJobPriority)
        {
            var jobFromRepo = _jobRepository.GetJob(oldJobPriority);

            if (jobFromRepo == null)
            {
                return NotFound();
            }

            var newJobEntity = _jobRepository.SetJobPriority(jobFromRepo, newJobPriority);

            var jobToReturn = _mapper.Map<JobDto>(newJobEntity);

            return CreatedAtRoute("GetJob", new { jobPriority = jobToReturn.Priority }, jobToReturn);
        }

        [HttpDelete("{jobPriority}")]
        public ActionResult DeleteJob(int jobPriority)
        {
            var jobFromRepo = _jobRepository.GetJob(jobPriority);
            if(jobFromRepo == null)
            {
                return NotFound();
            }
            _jobRepository.DeleteJob(jobFromRepo);
            return NoContent();
        }
    }
}
