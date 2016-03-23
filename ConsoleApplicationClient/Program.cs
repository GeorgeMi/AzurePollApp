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
               // bl.UserLogic.AddUser(u);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.InnerException.Message);
                Console.WriteLine(DateTime.Now.ToString());
                Console.WriteLine(DateTime.Now.AddHours(2));
            }
            bl.UserLogic.DeleteUser(5);
          //  bl.AuthLogic.send_email("1123xzzxcasdsad","george", "george.miron2003@gmail.com");
        }
    }
}
