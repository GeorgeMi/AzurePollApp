using AzureDataAccess;
using DataAccess;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace ConsoleApplicationClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IAzureDataAccess dal = new AzureDataAccess.AzureDataAccess();
            BusinessLogic.BusinessLogic bl = new BusinessLogic.BusinessLogic(dal);

            User u = new User { Email = "email", Username = "geo", Password = "p", Role = "user" };

            Console.WriteLine(NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault());
        }
    }
}
