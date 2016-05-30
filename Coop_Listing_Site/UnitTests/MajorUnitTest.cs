using System;
using NUnit.Framework;
using Coop_Listing_Site.Models;

namespace UnitTests
{
    [TestFixture]
    public class MajorUnitTest
    {
        [Test]
        public void TestMethod1()
        {
            TestRepository repo = new TestRepository();

            Major m = new Major() { MajorID = 5, MajorName = "Testing" };
            repo.Add(m);

            StudentInfo sInfo = new StudentInfo() { LNumber = "L00592182", GPA = 3.6, Major = m };
            repo.Add(sInfo);

            Assert.AreSame(m, repo.GetByID<Major>(5));
            Assert.AreSame(sInfo, repo.GetByID<StudentInfo>("L00592182"));
            
        }
    }
}
