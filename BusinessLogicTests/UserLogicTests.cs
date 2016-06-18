using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BusinessLogic;
using DataTransferObject;
using AzureDataAccess;
using BookLogicTest;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace BusinessLogicTests
{
    [TestClass()]
    public class UserLogicTests
    {
        IAzureDataAccess _mockDataAccess = new AzureDataAccessMock();
        UserLogic _userLogic;

        [SetUp]
        public void Setup()
        {
            _mockDataAccess = new AzureDataAccessMock();
            _userLogic = new UserLogic(_mockDataAccess);
        }

        [Test]
        public void GetAllUsers_Test()
        {
            Assert.AreEqual(3, _userLogic.GetAllUsers(0, 5).Count);
        }

        [Test]
        public void GetAllUsers_Test2()
        {
            Assert.AreEqual(1, _userLogic.GetAllUsers(0, 1).Count);
        }

        [Test]
        public void GetAllUsers_Test3()
        {
            Assert.AreEqual(0, _userLogic.GetAllUsers(0, -1).Count);
        }

        [Test]
        public void GetUserRole_Test()
        {
            Assert.AreEqual(0, _userLogic.GetUserRole("user1").CompareTo("admin"));
        }

        [Test]
        public void GetUserRole_Test2()
        {
            Assert.AreEqual(0, _userLogic.GetUserRole("user2").CompareTo("user"));
        }

        [Test]
        public void GetUserRole_Test3()
        {
            try
            {
                _userLogic.GetUserRole("user4");
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void AddUser_Test()
        {
            UserRegistrationDTO u = new UserRegistrationDTO()
            {
                Email = "user4@email.com",
                Password = "pass4",
                Username = "user4"
            };

            Assert.AreEqual(3, _userLogic.GetAllUsers(0, 5).Count);
            _userLogic.AddUser(u);
            Assert.AreEqual(4, _userLogic.GetAllUsers(0, 5).Count);
        }

        [Test]
        public void GetAllUsernames_Test()
        {
            Assert.AreEqual(3, _userLogic.GetAllUsernames().Count);
        }

        [Test]
        public void GetUser_Test()
        {
            Assert.AreEqual(1, _userLogic.GetUser(1).UserID);
        }

        [Test]
        public void GetUser_Test2()
        {
            try
            {
                _userLogic.GetUser(-1);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void GetUser_Test3()
        {
            try
            {
                _userLogic.GetUser(100);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void DeleteUser_Test()
        {
            Assert.AreEqual(3, _userLogic.GetAllUsers(0, 5).Count);
            _userLogic.DeleteUser(1);
            Assert.AreEqual(2, _userLogic.GetAllUsers(0, 5).Count);
        }

        [Test]
        public void DeleteUser_Test2()
        {
            try
            {
                _userLogic.DeleteUser(-1);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void DeleteUser_Test3()
        {
            try
            {
                _userLogic.DeleteUser(10);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void GetUsername_Test()
        {
            Assert.AreEqual(0, _userLogic.GetUsername(1).CompareTo("user1"));
        }

        [Test]
        public void GetUsername_Test2()
        {
            try
            {
                _userLogic.GetUsername(-1);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}