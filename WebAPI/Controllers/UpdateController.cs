using DataTransferObject;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class UpdateController : ApiController
    {
        [RequirePasswordForScheduler]
        public void Get()
        {
            //o data pe zi se vor sterge userii care nu au validat contul 
            //sondajele care au expirat vor fi facute closed
            UsersModel model = new UsersModel();

            model.ScheduleUpdates();
            
        }
    }
}