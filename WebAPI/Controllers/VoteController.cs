/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
using System.Linq;
using System.Web.Http;
using WebAPI.ActionFilters;
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
        public VoteResultDTO Post([FromBody] VoteListDTO voteDTO)
        {
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();

            VoteResultDTO result = formModel.Vote(voteDTO,token);
            return result;
        }

    }
}