/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using DataTransferObject;
using Microsoft.Practices.Unity;
using System;

namespace WebAPI.Models
{
    public class ContactModel
    {
        /// <summary>
        /// encapsulate contact model
        /// </summary>
        private BusinessLogic.BusinessLogic bl;

        /// <summary>
        /// Construct. Initializes the Unity container and injects dependency into BLL and DAL classes
        /// </summary>
        public ContactModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        /// <summary>
        /// ask business logic to send message from user
        /// </summary>
        /// <param name="token">token string</param>
        /// <param name="contactMessageDTO"></param>
        /// <returns>true or false</returns>
        public bool SendMessage(string token, ContactMessageDTO contactMessageDTO)
        {
            try
            {
                bl.MessageLogic.SendMessage(token, contactMessageDTO);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}