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
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// store votes in database
    /// </summary>
    public class VoteController : ApiController
    {
        FormModel formModel = new FormModel();

        /// <summary>
        /// store vote in database
        /// </summary>
        /// <param name="voteDTO">username and answers</param>
        /// <returns>form's results</returns>
        [RequireToken]
        public HttpResponseMessage Post([FromBody] VoteListDTO voteDTO)
        {
            HttpResponseMessage responseMessage;
            JSendMessage json;
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();
            VoteResultDTO result = formModel.Vote(voteDTO,token);
            
            if (result != null)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                json = new JSendMessage("fail", "Something bad happened");
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, json);
            }

            return responseMessage;
        }

    }
}