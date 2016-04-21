using BusinessLogic;
using DataTransferObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BusinessLogic.Tests
{
    [TestClass()]
    public class FormLogicTests
    {
        private BusinessLogic bl;

        [TestMethod()]
        public void GetUserFormsTest()
        {
            List<FormDTO> x= bl.FormLogic.GetUserForms(" ",1,1,"ok");
        }

      
    }
}