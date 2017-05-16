using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace GitHubSearch.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        public void UserTest(Models.User user)
        {
            var controller = new UserController();
            var result = controller.User() as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod()]
        public void UserTest1()
        {
            Assert.Fail();
        }
    }
}