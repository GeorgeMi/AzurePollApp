/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// handles HTTP contact requests
    /// </summary>
    public class FormController : ApiController
    {
        FormModel formModel = new FormModel();

        /// <summary>
        /// get all forms from database
        /// </summary>
        [RequireToken]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage responseMessage;
            JSend json;
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();
            int[] pageVal = GetPageNumberAndElementNumber();
            int page_nr = pageVal[0];
            int per_page = pageVal[1];
            List<FormDTO> list = formModel.GetAllForms(token, page_nr, per_page);

            if (list.Count > 0)
            {
                json = new JSendData<FormDTO>("success", list);
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "No items found");
                responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, json);
            }

            return responseMessage;

        }

        /// <summary>
        /// get all users's forms from database
        /// </summary>
        /// <param name="id">username</param>
        [RequireToken]
        [HttpGet]
        [ActionName("user")]
        public new HttpResponseMessage User(string id)
        {
            HttpResponseMessage responseMessage;
            JSend json;
            int[] pageVal = GetPageNumberAndElementNumber();
            int page_nr = pageVal[0];
            int per_page = pageVal[1];
            List<FormDTO> list = formModel.GetUserForms(id, page_nr, per_page);

            if (list.Count > 0)
            {
                json = new JSendData<FormDTO>("success", list);
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "No items found");
                responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, json);
            }

            return responseMessage;
        }

        /// <summary>
        /// get result of specific form
        /// </summary>
        /// <param name="id">form ID</param>
        [RequireToken]
        [HttpGet]
        [ActionName("result")]
        public HttpResponseMessage Result(int id)
        {
            HttpResponseMessage responseMessage;
            JSendMessage json;
            VoteResultDetailDTO voteResult = formModel.GetDetailResultForm(id);

            if (voteResult != null)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, voteResult);
            }
            else
            {
                json = new JSendMessage("fail", "Results not found");
                responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, json);
            }

            return responseMessage;
        }

        /// <summary>
        /// get content of specific form
        /// </summary>
        /// <param name="id">form ID</param>
        [RequireToken]
        [HttpGet]
        [ActionName("getForm")]
        public HttpResponseMessage GetForm(int id)
        {
            HttpResponseMessage responseMessage;
            JSendMessage json;
            FormDetailDTO formContent = formModel.GetContentForm(id);

            if (formContent != null)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, formContent);
            }
            else
            {
                json = new JSendMessage("fail", "Poll not found");
                responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, json);
            }

            return responseMessage;
        }

        /// <summary>
        /// get list of all polls voted by a specific user
        /// </summary>
        /// <param name="id">username</param>
        [RequireToken]
        [HttpGet]
        [ActionName("voted")]
        public HttpResponseMessage Voted(string id)
        {
            HttpResponseMessage responseMessage;
            JSend json;
            int[] pageVal = GetPageNumberAndElementNumber();
            int page_nr = pageVal[0];
            int per_page = pageVal[1];

            List<FormDTO> list = new List<FormDTO>();
            list = formModel.GetVotedForms(id, page_nr, per_page);

            if (list.Count > 0)
            {
                json = new JSendData<FormDTO>("success", list);
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "No items found");
                responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, json);
            }

            return responseMessage;
        }

        /// <summary>
        /// get list of all polls from a specific category
        /// </summary>
        /// <param name="id">category ID</param>
        [RequireToken]
        [HttpGet]
        [ActionName("category")]
        public HttpResponseMessage Category(int id)
        {
            HttpResponseMessage responseMessage;
            JSend json;
            List<FormDTO> list;
            int[] pageVal = GetPageNumberAndElementNumber();
            int page_nr = pageVal[0];
            int per_page = pageVal[1];
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();

            list = formModel.GetCategoryForms(id, token, page_nr, per_page);

            if (list.Count > 0)
            {
                json = new JSendData<FormDTO>("success", list);
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "No items found");
                responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, json);
            }

            return responseMessage;
        }

        /// <summary>
        /// add new form to database
        /// </summary>
        /// <param name="formDTO">form detailes</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireToken]
        public HttpResponseMessage Post([FromBody] FormDetailDTO formDTO)
        {
            HttpResponseMessage responseMessage;
            JSendMessage json;

            bool response = formModel.AddForm(formDTO);
            if (response)
            {
                json = new JSendMessage("success", "Form successfully added");
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "Something bad happened");
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, json);
            }

            return responseMessage;
        }

        /// <summary>
        /// delete form from database
        /// </summary>
        /// <param name="id">form ID</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireAdminTokenOrUsername]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage responseMessage;
            JSendMessage json;

            bool response = formModel.DeleteForm(id);
            if (response)
            {
                json = new JSendMessage("success", "Form successfully deleted");
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "Something bad happened");
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, json);
            }

            return responseMessage;
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
