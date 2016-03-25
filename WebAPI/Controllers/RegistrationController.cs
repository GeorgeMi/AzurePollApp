using DataTransferObject;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegistrationController : ApiController
    {
         public HttpResponseMessage Post(UserRegistrationDTO user)
        {
            UsersModel userModel = new UsersModel();

            bool add = userModel.AddUser(user);
            HttpResponseMessage response;
            

            if (add)
            {
                SuccessMessage msg = new SuccessMessage("Registration successful! Please, verify your mail address.");
                
                response = Request.CreateResponse(HttpStatusCode.OK, msg);
                return response;

            }
            else
            {
                ErrorMessage msg = new ErrorMessage("Registration failed! Please, try another username or email. ");

                response = Request.CreateResponse(HttpStatusCode.Forbidden, msg);
                return response;

            }
        }

        public HttpResponseMessage Get(string id)
        {
            AuthModel authModel = new AuthModel();

            bool verify = authModel.VerifyMailToken(id);
            HttpResponseMessage response;


            if (verify)
            {
                SuccessMessage msg = new SuccessMessage("Success!");

                response = Request.CreateResponse(HttpStatusCode.OK, msg);
                return response;

            }
            else
            {
                ErrorMessage msg = new ErrorMessage("Mail token is invalid!");

                response = Request.CreateResponse(HttpStatusCode.Forbidden, msg);
                return response;

            }
        }

    }
}
