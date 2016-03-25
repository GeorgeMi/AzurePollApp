/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 * Role:
 *  User Model.
 *
 * History:
 * 14.02.2016    Miron George       Created class and implemented methods.
 */
using DataTransferObject;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class UsersModel
    {
        private BusinessLogic.BusinessLogic bl;
        public UsersModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        public bool AddUser(UserRegistrationDTO userDTO)
        {
            try
            {
                int id = bl.UserLogic.AddUser(userDTO);
                //creeaza un nou token
                string token = bl.TokenLogic.UpdateToken(id, userDTO.Username, userDTO.Password);
                //trimite mail de verificare
                bl.UserLogic.Send_email(token, userDTO.Username, userDTO.Email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public int GetUserID(string username)
        {
            //cauta id-ul userului dupa username
            return bl.UserLogic.GetUserID(username);
        }

        public bool PromoteUser(int id)
        {
            //avanseaza user la rol de admin
            try
            {
                bl.UserLogic.PromoteUser(id);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool DemoteUser(int id)
        {
            //user la rol de user
            try
            {
                bl.UserLogic.DemoteUser(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<UserDetailDTO> GetAllUsers()
        {
            try
            {
                return bl.UserLogic.GetAllUsers();
            }
            catch
            {
                return null;
            }
        }

        public UserDetailDTO GetUser(int id)
        {
            try
            {
                return bl.UserLogic.GetUser(id);
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(int userID)
        {
            try
            {
                bl.UserLogic.DeleteUser(userID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ScheduleUpdates()
        {
            bl.UserLogic.ScheduledJobs();
        }
    }
}