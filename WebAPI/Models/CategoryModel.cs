/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace WebAPI.Models
{

    /// <summary>
    /// encapsulate category model
    /// </summary>
    public class CategoryModel
    {
        private BusinessLogic.BusinessLogic bl;
        /// <summary>
        /// Construct. Initializes the Unity container and injects dependency into BLL and DAL classes
        /// </summary>
        public CategoryModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        /// <summary>
        /// ask business logic to get all categories from database
        /// </summary>
        public List<CategoryDTO> GetAllCategories()
        {
            try
            {
                return bl.CategoryLogic.GetAllCategories();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///  ask business logic to add new category todatabase
        /// </summary>
        /// <param name="categoryDTO">category ID and category name</param>
        /// <returns>true or false</returns>
        public bool AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                bl.CategoryLogic.AddCategory(categoryDTO);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  ask business logic to delete category from database
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <returns>true or false</returns>
        public bool DeleteCategory(int categoryID)
        {
            try
            {
                bl.CategoryLogic.DeleteCategory(categoryID);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}