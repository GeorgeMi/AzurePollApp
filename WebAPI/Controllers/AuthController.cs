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
    /// Gestionare cereri HTTP de autentificare
    /// </summary>
    public class AuthController : ApiController
    {
        /// <summary>
        /// Primire username si parola de la client
        /// </summary>
        /// <param name="user">obiect care contine username si parola</param>
        /// <returns>token</returns>
        public HttpResponseMessage Post(UserDTO user)
        {
            AuthModel auth = new AuthModel();
            HttpResponseMessage responseMessage;
            string response = auth.Authenticate(user.Username, user.Password);

            if (response != null)
            {
                // Username si parola valide
                string role = auth.GetRole(user.Username);
                TokenMessage msg = new TokenMessage(response, role);
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                //invalid username and password
                JSendMessage msg = new JSendMessage("fail","Invalid username or password");
                responseMessage = Request.CreateResponse(HttpStatusCode.Forbidden, msg);
            }

            return responseMessage;
        }

        /// <summary>
        /// Primire token ce a fost trimis in mailul de activare a contului
        /// </summary>
        /// <param name="id">token</param>
        /// <returns>mesaj succes sau eroare</returns>
        public HttpResponseMessage Get(string id)
        {
            AuthModel auth = new AuthModel();
            HttpResponseMessage response;
            JSendMessage json;
            bool verify = auth.VerifyMailToken(id);

            if (verify)
            {
                json = new JSendMessage("success", "Your account has been successfully verified");
                response = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "Invalid verification link");
                response = Request.CreateResponse(HttpStatusCode.Forbidden, json);
            }

            return response;
        }
    }
}
