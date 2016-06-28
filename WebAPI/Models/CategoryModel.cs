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
    /// Modelul pentru gestionarea categoriilor
    /// </summary>
    public class CategoryModel
    {
        private BusinessLogic.BusinessLogic bl;
        /// <summary>
        /// Constructor. Initializeaza Unity container si injecteaza dependenta in BLL si DAL 
        /// </summary>
        public CategoryModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        /// <summary>
        /// Cere BLL sa returneze toate categoriile
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
        ///  Cere BLL sa adauge o noua categorie
        /// </summary>
        /// <param name="categoryDTO">category ID si category name</param>
        /// <returns>true sau false</returns>
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
        ///  Cere BLL sa stearga o categorie
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <returns>true sau false</returns>
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