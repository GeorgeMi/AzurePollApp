using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.ActionFilters
{
    public class RequireAdminTokenOrUsername : ActionFilterAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public override void OnActionExecuting(HttpActionContext context)
        {
            AuthModel authModel = new AuthModel();
            FormModel formModel = new FormModel();
            JSendMessage json;
            var header = context.Request.Headers.SingleOrDefault(x => x.Key == "token");
            var formIdToDelete = context.Request.RequestUri.Segments[3];

            bool valid=false, isAdmin=false, okDate=false, formIsFromUser=false;

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

                // Tokenul si sondajul apartin aceluiasi user
                formIsFromUser = formModel.FormIdCreatedbyUserId(Int32.Parse(formIdToDelete), header.Value.First());

            }

            if (!(valid || formIsFromUser))
            {
                // Token invalid
                json = new JSendMessage("fail", "Invalid Authorization Key");
                context.Response = context.Request.CreateResponse(HttpStatusCode.Forbidden, json);
            }
        }
    }
}