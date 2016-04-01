using DataTransferObject;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ContactController : ApiController
    {
        ContactModel contactModel = new ContactModel();

        [RequireToken]
        public HttpResponseMessage Post(ContactMessageDTO contactMessageDTO)
        {
            HttpResponseMessage responseMessage;
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();

            bool response = contactModel.SendMessage(token, contactMessageDTO);

            if (response)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }

            return responseMessage;

        }

    }
}
