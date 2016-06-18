using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataTransferObject;
using AzureDataAccess;
using BookLogicTest;
using BusinessLogic;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace BusinessLogicTests
{
    [TestClass()]
    public class CategoryLogicTests
    {
        IAzureDataAccess _mockDataAccess = new AzureDataAccessMock();
        CategoryLogic _categoryLogic;

        [SetUp]
        public void Setup()
        {
            _mockDataAccess = new AzureDataAccessMock();
            _categoryLogic = new CategoryLogic(_mockDataAccess);
        }

        [Test]
        public void GetUserForms_Test()
        {
            Assert.AreEqual(3, _categoryLogic.GetAllCategories().Count);
        }

        [Test]
        public void AddCategory_Test()
        {
            Assert.AreEqual(3, _categoryLogic.GetAllCategories().Count);
            _categoryLogic.AddCategory(new CategoryDTO() {CategoryID = 5, Name = "category5"});
            Assert.AreEqual(4, _categoryLogic.GetAllCategories().Count);
        }

        [Test]
        public void DeleteCategory_Test()
        {
            Assert.AreEqual(3, _categoryLogic.GetAllCategories().Count);
            _categoryLogic.DeleteCategory(1);
            Assert.AreEqual(2, _categoryLogic.GetAllCategories().Count);
        }

        [Test]
        public void DeleteCategory_Test2()
        {
            try
            {
                _categoryLogic.DeleteCategory(5);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
   }
}