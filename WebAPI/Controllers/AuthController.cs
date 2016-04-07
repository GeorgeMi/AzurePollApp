/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using DataTransferObject;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// handles HTTP authentification requests
    /// </summary>
    public class AuthController : ApiController
    {
        /// <summary>
        /// receive username and password from user
        /// </summary>
        /// <param name="user">object that containes username and password</param>
        /// <returns>user's token if params are valid, error message else</returns>
        public HttpResponseMessage Post(UserDTO user)
        {
            AuthModel auth = new AuthModel();
            HttpResponseMessage responseMessage;
            string response = auth.Authenticate(user.Username, user.Password);

            if (response != null)
            {
                //valid username and password
                string role = auth.GetRole(user.Username);
                TokenMessage msg = new TokenMessage(response, role);
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, msg);

            }
            else
            {
                //invalid username and password
                ErrorMessage msg = new ErrorMessage("Invalid username or password");
                responseMessage = Request.CreateResponse(HttpStatusCode.Forbidden, msg);
            }

            return responseMessage;
        }

        /// <summary>
        /// receive token sent in registration mail and activate account
        /// </summary>
        /// <param name="id">token</param>
        /// <returns>success message or error message </returns>
        public HttpResponseMessage Get(string id)
        {
            AuthModel auth = new AuthModel();
            bool verify = auth.VerifyMailToken(id);
            HttpResponseMessage response;

            if (verify)
            {
                SuccessMessage msg = new SuccessMessage("Your account has been successfully verified!");
                response = Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                ErrorMessage msg = new ErrorMessage("Invalid verification link!");
                response = Request.CreateResponse(HttpStatusCode.Forbidden, msg);
            }

            return response;
        }
    }
}
