using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using AzureDataAccess;
using BookLogicTest;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace BusinessLogicTests
{
    [TestClass()]
    public class AuthLogicTests
    {
        IAzureDataAccess _mockDataAccess = new AzureDataAccessMock();
        AuthLogic _authLogic;

        [SetUp]
        public void Setup()
        {
            _mockDataAccess = new AzureDataAccessMock();
            _authLogic = new AuthLogic(_mockDataAccess);
        }

        [Test]
        public void VerifyAdminToken_Test()
        {
           Assert.AreEqual(true,_authLogic.VerifyAdminToken("1"));
        }

        [Test]
        public void VerifyAdminToken_Test2()
        {
            Assert.AreEqual(false, _authLogic.VerifyAdminToken("5"));
        }

        [Test]
        public void VerifyAdminToken_Test3()
        {
            Assert.AreEqual(false, _authLogic.VerifyAdminToken("-500"));
        }
    }
}