
using AzureDataAccess;
using DataTransferObject;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    /// <summary>
    /// Logica gestionarii categoriilor
    /// </summary>
    public class CategoryLogic
    {
        private IAzureDataAccess _dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDataAccess"></param>
        public CategoryLogic(IAzureDataAccess objDataAccess)
        {
            //Primesc obiectul, nu e treaba CategoryLogic ce dataAccess se foloseste
            //Unity are grija de dependency injection
            _dataAccess = objDataAccess;
        }

        /// <summary>
        /// Intoarce toate categoriile 
        /// </summary>
        /// <returns></returns>
        public List<CategoryDTO> GetAllCategories()
        {
            List<CategoryDTO> listCategoryDTO = new List<CategoryDTO>();
            List<Category> listCategory = _dataAccess.CategoryRepository.GetAll().ToList();
            CategoryDTO categoryDTO;

            foreach(Category c in listCategory)
            {
                categoryDTO = new CategoryDTO();
                categoryDTO.CategoryID = c.CategoryID;
                categoryDTO.Name = c.Name;

                listCategoryDTO.Add(categoryDTO);
            }
            return listCategoryDTO;
        }
        /// <summary>
        /// Adaugare categorie
        /// </summary>
        /// <param name="categoryDTO"></param>
        public void AddCategory(CategoryDTO categoryDTO)
        {
            Category category = new Category { Name = categoryDTO.Name };

            _dataAccess.CategoryRepository.Add(category);
        }

        /// <summary>
        /// Stergere categorie
        /// </summary>
        /// <param name="categoryID"></param>
        public void DeleteCategory(int categoryID)
        {
            Category category = _dataAccess.CategoryRepository.FindFirstBy(cat => cat.CategoryID == categoryID);
            _dataAccess.CategoryRepository.Delete(category);
        }
    }
}
