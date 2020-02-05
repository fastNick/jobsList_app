using NUnit.Framework;
using SmartiwayJobs.Api.DbContext;
using SmartiwayJobs.Api.Entities;
using SmartiwayJobs.Api.Services;
using System.Collections.Generic;
using System.Linq;

namespace SmartiwayJobs.Tests
{
    [TestFixture]
    public class JobRepositoryShould
    {

        private JobContextFake jobContextFake;
        private JobRepository sut;

        [SetUp]
        public void Setup()
        {
            jobContextFake = new JobContextFake();
            sut = new JobRepository(jobContextFake);
        }

        [Test]
        public void GetCorrectNameForJobPriority()
        {            
            var job = sut.GetJob(2);
            Assert.AreEqual("Read book about Angular", job.Name);
        }

        [Test]
        public void AppendJobToTheEndOfList()
        {            
            var name = "Watch Pluralsight courses";
            var job = new Job { Name = name };
            sut.AddJob(job);
            Assert.AreEqual(name, sut.GetJobs().Last().Name);
        }

        [Test]
        public void IncrementJobPriorityIfJobWithMorePriorityAlreadyExists()
        {
            var secondJob = sut.GetJob(2);
            var moreJob = sut.GetJob(3);
            var incrementedJob = sut.IncrementJobPriority(secondJob);
            Assert.AreEqual(incrementedJob.Priority, 3);
            Assert.AreEqual(moreJob.Priority, 2);
        }

        [Test]
        public void DecrementJobPriorityIfJobWithFewerPriorityAlreadyExists()
        {
            var secondJob = sut.GetJob(2);
            var fewerJob = sut.GetJob(1);
            var decrementedJob = sut.DecrementJobPriority(secondJob);
            Assert.AreEqual(decrementedJob.Priority, 1);
            Assert.AreEqual(fewerJob.Priority, 2);
        }

        [Test]
        public void SetJobPriorityUpdateOtherPrioritiesProperly()
        {
            var firstJob = sut.GetJob(1);
            var secondJob = sut.GetJob(2);
            var thirdJob = sut.GetJob(3);
            var updatedJob = sut.SetJobPriority(thirdJob, 1);
            Assert.AreEqual(updatedJob.Priority, 1);
            Assert.AreEqual(firstJob.Priority, 2);
            Assert.AreEqual(secondJob.Priority, 3);
        }
    }
}
