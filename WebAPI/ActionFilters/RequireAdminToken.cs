using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.ActionFilters
{
    public class RequireAdminToken : ActionFilterAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public override void OnActionExecuting(HttpActionContext context)
        {
            AuthModel authModel = new AuthModel();
            JSendMessage json;
            var header = context.Request.Headers.SingleOrDefault(x => x.Key == "token");

            bool valid, isAdmin, okDate;

            if (header.Value == null)
            {
                valid = false;
            }
            else
            {
                // Tokenul apartine unui admin
                isAdmin = authModel.VerifyAdminToken(header.Value.First());

                // Tokenul este valid
                okDate = authModel.VerifyToken(header.Value.First());

                valid = isAdmin && okDate;
            }

            if (!valid)
            {
                json = new JSendMessage("fail", "Invalid Authorization Key");
                context.Response = context.Request.CreateResponse(HttpStatusCode.Forbidden, json);
            }
        }
    }
}