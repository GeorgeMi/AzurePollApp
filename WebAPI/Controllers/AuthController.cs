﻿using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AuthController : ApiController
    {
       
        public HttpResponseMessage Post(UserDTO user)
        {
            AuthModel auth = new AuthModel();
            HttpResponseMessage responseMessage;
            string response = auth.Authenticate(user.Username, user.Password);

            if (response != null)
            {
                string role = auth.GetRole(user.Username);
                TokenMessage msg = new TokenMessage(response,role);
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, msg);

            }
            else
            {
                ErrorMessage msg = new ErrorMessage("Invalid username or password");
                responseMessage = Request.CreateResponse(HttpStatusCode.Forbidden, msg);
            }

            return responseMessage;
        }
        public HttpResponseMessage Get(string id)
        {
            AuthModel auth = new AuthModel();

            bool add = auth.VerifyMailToken(id);
            HttpResponseMessage response;
            
            if (add)
            {
                SuccessMessage msg = new SuccessMessage("Registration successful!");

                response = Request.CreateResponse(HttpStatusCode.OK, msg);
                return response;

            }
            else
            {
                ErrorMessage msg = new ErrorMessage("Registration failed! Please, try another username or email ");

                response = Request.CreateResponse(HttpStatusCode.Forbidden, msg);
                return response;

            }
        }
    }
}
