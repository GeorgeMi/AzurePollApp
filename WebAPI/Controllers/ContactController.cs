/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// handles HTTP contact requests
    /// </summary>
    public class ContactController : ApiController
    {
        ContactModel contactModel = new ContactModel();

        /// <summary>
        /// receive message from user and send it to admin
        /// </summary>
        /// <param name="contactMessageDTO">message category and message string</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireToken]
        public HttpResponseMessage Post(ContactMessageDTO contactMessageDTO)
        {
            HttpResponseMessage responseMessage;
            //get token to indentify user's contact details
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
