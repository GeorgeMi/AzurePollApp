using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAPI.Messages;
using WebAPI.Models;


namespace WebAPI.ActionFilters
{
    public class RequirePasswordForScheduler : ActionFilterAttribute
    {
        /// <summary>
        /// Public default Constructor
        /// </summary>
        public override void OnActionExecuting(HttpActionContext context)
        {
            var header = context.Request.Headers.SingleOrDefault(x => x.Key == "scheduler");
            bool valid = false;
            JSendMessage json;

            if (header.Value == null)
            {
                valid = false;
            }
            else
            {
                if (header.Value.ToArray()[0].Equals("Irm@ge0mi"))
                {
                    valid = true;
                }
            }

            if (!valid)
            {
                //Invalid Authorization Key
                json = new JSendMessage("fail", "Invalid Authorization Key");
                context.Response = context.Request.CreateResponse(HttpStatusCode.Forbidden, json);
            }



        }
    }
}