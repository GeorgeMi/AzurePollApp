using AzureDataAccess;
using DataAccess;
using Entities;
using System;
using System.Collections.Generic;

namespace ConsoleApplicationClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IAzureDataAccess dal = new AzureDataAccess.AzureDataAccess();
            BusinessLogic.BusinessLogic bl = new BusinessLogic.BusinessLogic(dal);

            User u = new User { Email = "email", Username = "geo", Password = "p", Role = "user" };
            try
            {

                DataTransferObject.UserRegistrationDTO x = new DataTransferObject.UserRegistrationDTO() { Email = "email", Username = "geo", Password = "p" };
                bl.UserLogic.AddUser(x);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.InnerException.Message);
              
            }

           // bl.UserLogic.ScheduledJobs();
       //     bl.UserLogic.DeleteUser(5);
          //  bl.AuthLogic.send_email("1123xzzxcasdsad","george", "george.miron2003@gmail.com");
        }
    }
}
