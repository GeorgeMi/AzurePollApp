/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// handles HTTP user requests
    /// </summary>
    public class UserController : ApiController
    {
        UsersModel userModel = new UsersModel();

        /// <summary>
        /// get list of all users'details from database
        /// </summary>
        [RequireAdminToken]
        public IEnumerable<UserDetailDTO> Get()
        {
            int[] pageVal = GetPageNumberAndElementNumber();
            int page_nr = pageVal[0];
            int per_page = pageVal[1];

            List<UserDetailDTO> list = new List<UserDetailDTO>();
            list = userModel.GetAllUsers(page_nr, per_page);
            return list;
        }

        /// <summary>
        /// get user detail from database
        /// </summary>
        /// <param name="id">user ID</param>
        [RequireToken]
        public UserDetailDTO Get(int id)
        {
            return userModel.GetUser(id);
        }

        /// <summary>
        /// delete user from database
        /// </summary>
        /// <param name="id">user ID</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireAdminToken]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage responseMessage;

            bool response = userModel.Delete(id);
            if (response)
            {
                SuccessMessage msg = new SuccessMessage("deleted");
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }

            return responseMessage;
        }

        /// <summary>
        /// change user's role to admin
        /// </summary>
        /// <param name="id">user ID</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireAdminToken]
        [HttpGet]
        [ActionName("promote")]
        public HttpResponseMessage Promote(int id)
        {

            HttpResponseMessage responseMessage;

            bool response = userModel.PromoteUser(id);
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

        /// <summary>
        /// change user's role to user
        /// </summary>
        /// <param name="id">user ID</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireAdminToken]
        [HttpGet]
        [ActionName("demote")]
        public HttpResponseMessage Demote(int id)
        {
            HttpResponseMessage responseMessage;

            bool response = userModel.DemoteUser(id);
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

        [RequireToken]
        [ActionName("aaa")]
        public string GetUser(int id, string s)
        {
            return id.ToString() + " asdasdasdasds";
        }

        /// <summary>
        /// parse query from request and get page number and number of elements per page
        /// </summary>
        /// <returns>page number and number of elements per page</returns>
        private int[] GetPageNumberAndElementNumber()
        {
            int[] result = new int[2];
            int page_nr = 0, per_page = 10;
            try
            {
                //if query exists and it is valid, default page number and number of elements per page values are changing 
                var queryString = this.Request.GetQueryNameValuePairs();

                foreach (KeyValuePair<string, string> pair in queryString)
                {
                    if (pair.Key == "page")
                    {
                        page_nr = Int32.Parse(pair.Value);
                    }
                    if (pair.Key == "per_page")
                    {
                        per_page = Int32.Parse(pair.Value);
                    }
                }

                if (page_nr < 0 || per_page < 0)
                {
                    page_nr = 0;
                    per_page = 10;
                }
            }
            catch
            {
                page_nr = 0;
                per_page = 10;
            }

            result[0] = page_nr;
            result[1] = per_page;

            return result;
        }
    }
}
