using System;
using BusinessLogic;
using DataTransferObject;
using System.Collections.Generic;
using AzureDataAccess;
using BookLogicTest;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace BusinessLogicTests
{
    [TestClass()]
    public class TokenLogicTests
    {
        IAzureDataAccess _mockDataAccess = new AzureDataAccessMock();
        TokenLogic _tokenLogic;

        [SetUp]
        public void Setup()
        {
            _mockDataAccess = new AzureDataAccessMock();
            _tokenLogic = new TokenLogic(_mockDataAccess);
        }

        [Test]
        public void AddToken_Test()
        {
            _tokenLogic.AddToken(new Token() {CreatedDate = DateTime.Now, ExpirationDate = DateTime.Now.AddHours(3),TokenID = 4,TokenString = "4",UserID = 1});
        }
    }
}