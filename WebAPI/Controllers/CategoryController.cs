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
        public IEnumerable<CategoryDTO> Get()
        {
           return categoryModel.GetAllCategories();
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

            bool response = categoryModel.AddCategory(categoryDTO);
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
        /// delete category from database
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <returns>http status code OK or ExpectationFailed</returns>
        [RequireAdminToken]
        public HttpResponseMessage Delete (int id)
        {
            HttpResponseMessage responseMessage;

            bool response = categoryModel.DeleteCategory(id);
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
    }
}
