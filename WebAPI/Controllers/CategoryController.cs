/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using DataTransferObject;
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
    /// handles HTTP categories requests
    /// </summary>
    public class CategoryController : ApiController
    {
        CategoryModel categoryModel = new CategoryModel();

        /// <summary>
        /// get all categories from database
        /// </summary>
        [RequireToken]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage responseMessage;
            JSend json;
            List<CategoryDTO> list = categoryModel.GetAllCategories();

            if (list.Count > 0)
            {
                json = new JSendDataList<CategoryDTO>("success", list);
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "no items found");
                responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, json);
            }

            return responseMessage;
        }

        /// <summary>
        /// add new category todatabase
        /// </summary>
        /// <param name="categoryDTO">category ID and category name</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireAdminToken]
        public HttpResponseMessage Post(CategoryDTO categoryDTO)
        {
            HttpResponseMessage responseMessage;
            JSend json;
            bool response = categoryModel.AddCategory(categoryDTO);

            if (response)
            {
                json = new JSendMessage("success", "category successfully added");
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "something bad happened");
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed, json);
            }

            return responseMessage;

        }

        /// <summary>
        /// delete category from database
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireAdminToken]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage responseMessage;
            JSend json;
            bool response = categoryModel.DeleteCategory(id);

            if (response)
            {
                json = new JSendMessage("success", "category successfully deleted");
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
            }
            else
            {
                json = new JSendMessage("fail", "something bad happened");
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed, json);
            }

            return responseMessage;
        }
    }
}
