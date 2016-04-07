/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// handles HTTP register requests
    /// </summary>
    public class SearchController: ApiController
    {
        FormModel formModel = new FormModel();

        /// <summary>
        /// get list of all forms that contains specific string
        /// </summary>
        /// <param name="id">string to search</param>
        [RequireToken]
        public IEnumerable<FormDTO> Get(string id)
        {
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();
            int[] pageVal = GetPageNumberAndElementNumber();
            int page_nr = pageVal[0];
            int per_page = pageVal[1];

            List<FormDTO> list = new List<FormDTO>();
            list = formModel.GetAllForms(token, id, page_nr, per_page);
            return list;
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