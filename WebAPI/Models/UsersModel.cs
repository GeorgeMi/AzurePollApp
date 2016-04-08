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
    public class UsersModel
    {
        /// <summary>
        /// encapsulate user model
        /// </summary>
        private BusinessLogic.BusinessLogic bl;

        /// <summary>
        /// Construct. Initializes the Unity container and injects dependency into BLL and DAL classes
        /// </summary>
        public UsersModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        /// <summary>
        /// ask business logic to add new user to database
        /// </summary>
        /// <param name="userDTO">user's details</param>
        public bool AddUser(UserRegistrationDTO userDTO)
        {
            try
            {
                int id = bl.UserLogic.AddUser(userDTO);
                //create new token
                string token = bl.TokenLogic.UpdateToken(id, userDTO.Username, userDTO.Password);
                //send verification mail
                bl.UserLogic.SendAuthEmail(token, userDTO.Username, userDTO.Email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// get list of all users' username and id from database
        /// </summary>
        public List<UsernameDTO> GetAllUsernames()
        {
            try
            {
                return bl.UserLogic.GetAllUsernames();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ask business logic to get username's id
        /// </summary>
        /// <param name="username">username</param>
        public int GetUserID(string username)
        {
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
        public List<UserDetailDTO> GetAllUsers(int page_nr,int per_page)
        {
            try
            {
                return bl.UserLogic.GetAllUsers(page_nr, per_page);
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