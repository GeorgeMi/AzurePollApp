/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using Microsoft.Practices.Unity;

namespace WebAPI.Models
{
    /// <summary>
    /// encapsulate authentification model
    /// </summary>
    public class AuthModel
    {
        private BusinessLogic.BusinessLogic bl;

        /// <summary>
        /// Construct. Initializes the Unity container and injects dependency into BLL and DAL classes
        /// </summary>
        public AuthModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        /// <summary>
        ///  ask business logic to validate username and password and to get updated token string
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>updated token string</returns>
        public string Authenticate(string username, string password)
        {
            return bl.AuthLogic.Authenticate(username, password);
        }

        /// <summary>
        /// ask business logic to get user's role
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>user's role</returns>
        public string GetRole(string username)
        {
            return bl.UserLogic.GetUserRole(username);
        }

        /// <summary>
        /// ask business logic to check if token string exists
        /// </summary>
        /// <param name="token">token string</param>
        /// <returns>true or false</returns>
        public bool VerifyToken(string token)
        {
            return bl.AuthLogic.VerifyTokenDate(token);
        }

        /// <summary>
        /// ask business logic to check if token's owner is admin
        /// </summary>
        /// <param name="token">token string</param>
        /// <returns>true or false</returns>
        public bool VerifyAdminToken(string token)
        {
            return bl.AuthLogic.VerifyAdminToken(token);
        }

        /// <summary>
        ///  ask business logic to verify token from mail
        /// </summary>
        /// <param name="token">token string</param>
        /// <returns>true or false</returns>
        public bool VerifyMailToken(string token)
        {
            return bl.AuthLogic.VerifyMailToken(token);
        }

    }
}