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
    /// handles HTTP register requests
    /// </summary>
    public class RegistrationController : ApiController
    {
        /// <summary>
        /// get user's details, verify their uniqueness and add user to database 
        /// </summary>
        /// <param name="user">user's details</param>
        /// <returns></returns>
        public HttpResponseMessage Post(UserRegistrationDTO user)
        {
            UsersModel userModel = new UsersModel();
            HttpResponseMessage response;
            JSend json;
            bool add = userModel.AddUser(user);

            if (add)
            {
                json = new JSendMessage("success", "Registration successful! Please, verify your mail address.");
                response = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("failed", "Registration failed! Please, try another username or email.");
                response = Request.CreateResponse(HttpStatusCode.Forbidden, json);
            }

            return response;
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
            JSend json;

            if (verify)
            {
                json = new JSendMessage("success", "Your account has been successfully verified!");
                response = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("failed", "Invalid verification link!");
                response = Request.CreateResponse(HttpStatusCode.Forbidden, json);
            }

            return response;
        }

    }
}
