using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SmartiwayJobs.Api.Controllers;
using SmartiwayJobs.Api.DbContext;
using SmartiwayJobs.Api.Entities;
using SmartiwayJobs.Api.Models;
using SmartiwayJobs.Api.Profiles;
using SmartiwayJobs.Api.Services;
using System.Collections.Generic;
using System.Linq;

namespace SmartiwayJobs.Tests
{
    [TestFixture]
    public class JobIntegrationTests
    {
        private JobController _jobController;
        private Mock<IJobRepository> _mockRepository;

        [SetUp]
        public void Setup() {

            _mockRepository = new Mock<IJobRepository>();
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new JobProfile());
            });

            var mapper = config.CreateMapper();


            _jobController = new JobController(_mockRepository.Object, mapper);

            var testJobsList = new List<Job>()
            {
                new Job{Priority = 1, Name = "Write high-quality code!"},
                new Job{Priority = 2, Name = "Read book about Angular"}
            };

            _mockRepository.Setup(j => j.GetJobs()).Returns(testJobsList);
        }

        [Test]
        public void GetJobsTest_ReturnJobs()
        {           
            var returnedJobs = _jobController.GetJobs();
            var okResult = returnedJobs.Result as OkObjectResult;
            var jobsList =  okResult.Value as List<JobDto>;
            Assert.AreEqual(2, jobsList.Count());
        }
    }
}
