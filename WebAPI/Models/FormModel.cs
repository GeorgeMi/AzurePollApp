/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 * Role:
 *  Form Model.
 *
 * History:
 * 25.02.2016    Miron George       Created class and implemented methods.
 */
using DataTransferObject;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class FormModel
    {
        private BusinessLogic.BusinessLogic bl;
        public FormModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

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

        internal List<FormDTO> GetCategoryForms(int categoryID, string token, int page_nr, int per_page)
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