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
        /// Modelul pentru gestionarea utilizatorilor
        /// </summary>
        private BusinessLogic.BusinessLogic bl;

        /// <summary>
        /// Constructor. Initializeaza Unity container si injecteaza dependenta in BLL si DAL 
        /// </summary>
        public UsersModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        /// <summary>
        /// Cere BLL sa adauge un nou utilizator 
        /// </summary>
        /// <param name="userDTO">user's details</param>
        public bool AddUser(UserRegistrationDTO userDTO)
        {
            try
            {
                int id = bl.UserLogic.AddUser(userDTO);
                // Creare token
                string token = bl.TokenLogic.UpdateToken(id, userDTO.Username, userDTO.Password);
                // Trimitere mai verificare
                bl.UserLogic.SendAuthEmail(token, userDTO.Username, userDTO.Email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Cere BLL sa returneze toate id-urile si username-urile utilizatorilor 
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
        /// Cere BLL sa returneze id-ul utilizatorului
        /// </summary>
        /// <param name="username">username</param>
        public int GetUserID(string username)
        {
            return bl.UserLogic.GetUserID(username);
        }

        public bool PromoteUser(int id)
        {
            // Avanseaza user la rol de admin
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
            // Retrogradeaza user la rol de user
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