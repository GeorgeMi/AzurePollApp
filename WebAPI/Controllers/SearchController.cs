﻿/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
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
    /// Gestionare cereri HTTP pentru cautare
    /// </summary>
    public class SearchController: ApiController
    {
        FormModel formModel = new FormModel();

        /// <summary>
        /// Returnarea tuturor sondajelor care contin o anumita secventa
        /// </summary>
        /// <param name="id">secventa cautata</param>
        [RequireToken]
        public HttpResponseMessage Get(string id)
        {
            HttpResponseMessage responseMessage;
            JSend json;
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();
            int[] pageVal = GetPageNumberAndElementNumber();
            int page_nr = pageVal[0];
            int per_page = pageVal[1];
            string state = GetState();

            List<FormDTO> list = new List<FormDTO>();
            list = formModel.GetAllForms(token, id, page_nr, per_page,  state);

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

        private string GetState()
        {
            string state = "open";

            try
            {
                //if query exists and it is valid, default state value is changing 
                var queryString = this.Request.GetQueryNameValuePairs();

                foreach (KeyValuePair<string, string> pair in queryString)
                {
                    if (pair.Key == "state")
                    {
                        state = pair.Value.ToString();
                    }
                }

                if (state != "open" && state != "closed" && state != "all")
                {
                    state = "open";
                }
            }
            catch
            {
                state = "open";
            }

            return state;
        }
    }
}