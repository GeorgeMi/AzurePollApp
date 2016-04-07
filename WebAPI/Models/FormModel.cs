/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class FormModel
    {
        /// <summary>
        /// encapsulate form model
        /// </summary>
        private BusinessLogic.BusinessLogic bl;

        /// <summary>
        /// Construct. Initializes the Unity container and injects dependency into BLL and DAL classes
        /// </summary>
        public FormModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        /// <summary>
        /// ask business logic to get all forms from database
        /// </summary>
        /// <param name="token">token string</param>
        /// <param name="page">page number</param>
        /// <param name="per_page">elements per page</param>
        public List<FormDTO> GetAllForms(string token, int page, int per_page)
        {
            try
            {
                return bl.FormLogic.GetAllForms(token, page, per_page);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// ask business logic to get form's detailed results
        /// </summary>
        /// <param name="id">form ID</param>
        public VoteResultDetailDTO GetDetailResultForm(int id)
        {
            try
            {
                return bl.FormLogic.GetDetailResultForm(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// ask business logic to get all forms from specific user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="page">page number</param>
        /// <param name="per_page">elements per page</param>
        public List<FormDTO> GetUserForms(string username, int page, int per_page)
        {
            try
            {
                return bl.FormLogic.GetUserForms(username, page, per_page);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///  ask business logic to get all forms that containes specific string
        /// </summary>
        /// <param name="token">token string</param>
        /// <param name="searchedName">string to search</param>
        /// <param name="page">page number</param>
        /// <param name="per_page">elements per page</param>
        internal List<FormDTO> GetAllForms(string token, string searched, int page_nr, int per_page)
        {
            try
            {
                return bl.FormLogic.GetAllForms(token, searched, page_nr, per_page);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// ask business logic to get all voted forms from specific user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="page">page number</param>
        /// <param name="per_page">elements per page</param>
        public List<FormDTO> GetVotedForms(string username, int page_nr, int per_page)
        {
            try
            {
                return bl.FormLogic.GetVotedForms(username, page_nr, per_page);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///  ask business logic to specific form's content
        /// </summary>
        /// <param name="id">form ID</param>
        /// <returns></returns>
        public FormDetailDTO GetContentForm(int id)
        {
            try
            {
                FormDetailDTO formDTO = bl.FormLogic.GetContentForm(id);
                return formDTO;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ask business logic to get all forms from category
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <param name="token">token string</param>
        /// <param name="page_nr">page number</param>
        /// <param name="per_page">elements per page</param>
        public List<FormDTO> GetCategoryForms(int categoryID, string token, int page_nr, int per_page)
        {
            try
            {
                return bl.FormLogic.GetCategoryForms(categoryID, token, page_nr, per_page);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ask business logic to add new form to database
        /// </summary>
        /// <param name="formDTO">form details</param>
        /// <returns></returns>
        public bool AddForm(FormDetailDTO formDTO)
        {
            try
            {
                bl.FormLogic.AddForm(formDTO);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// ask business logic to delete form from database
        /// </summary>
        /// <param name="formID">form ID</param>
        /// <returns></returns>
        public bool DeleteForm(int formID)
        {
            try
            {
                bl.FormLogic.DeleteForm(formID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// ask business logic to check if form's owner is user 
        /// </summary>
        /// <param name="formID">form ID</param>
        /// <param name="userToken">token string</param>
        /// <returns>true or false</returns>
        public bool FormIdCreatedbyUserId(int formID, string userToken)
        {
            try
            {
                bl.FormLogic.FormIdCreatedbyUserId(formID, userToken);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// ask business logic to add vote 
        /// </summary>
        /// <param name="voteDTO">vote's details</param>
        /// <param name="token">token string</param>
        /// <returns></returns>
        public VoteResultDTO Vote(VoteListDTO voteDTO, string token)
        {
            try
            {
                return bl.FormLogic.Vote(voteDTO, token);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}