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
    /// Gestionare cereri HTTP pentru votare
    /// </summary>
    public class VoteController : ApiController
    {
        FormModel formModel = new FormModel();

        /// <summary>
        /// Votare
        /// </summary>
        /// <param name="voteDTO">username si raspunsuri</param>
        /// <returns>rezultatele sondajului</returns>
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
                json = new JSendMessage("fail", "Poll already voted");
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, json);
            }

            return responseMessage;
        }
    }
}