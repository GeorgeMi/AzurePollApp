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
    /// Gestionare cereri HTTP pentru categorii
    /// </summary>
    public class CategoryController : ApiController
    {
        CategoryModel categoryModel = new CategoryModel();

        /// <summary>
        /// Returnarea tuturor categoriilor
        /// </summary>
        [RequireToken]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage responseMessage;
            JSend json;
            List<CategoryDTO> list = categoryModel.GetAllCategories();

            if (list.Count > 0)
            {
                json = new JSendData<CategoryDTO>("success", list);
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
        /// Adaugarea unei noi categorii
        /// </summary>
        /// <param name="categoryDTO">category ID si category name</param>
        /// <returns>http status code OK sau ExpectationFailed</returns>
        [RequireAdminToken]
        public HttpResponseMessage Post(CategoryDTO categoryDTO)
        {
            HttpResponseMessage responseMessage;
            JSendMessage json;
            bool response = categoryModel.AddCategory(categoryDTO);

            if (response)
            {
                json = new JSendMessage("success", "Category successfully added");
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
        /// Stergerea unei categorii
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <returns>http status code OK sau ExpectationFailed</returns>
        [RequireAdminToken]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage responseMessage;
            JSendMessage json;
            bool response = categoryModel.DeleteCategory(id);

            if (response)
            {
                json = new JSendMessage("success", "Category successfully deleted");
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, json);
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
