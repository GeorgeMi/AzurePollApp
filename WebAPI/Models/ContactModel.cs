﻿/* Copyright (C) Miron George - All Rights Reserved
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
        /// Modelul pentru gestionarea contactarii
        /// </summary>
        private BusinessLogic.BusinessLogic bl;

        /// <summary>
        /// Constructor. Initializeaza Unity container si injecteaza dependenta in BLL si DAL 
        /// </summary>
        public ContactModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        /// <summary>
        /// Cere BLL sa trimita mesaj primit de la utilizator
        /// </summary>
        /// <param name="token">token string</param>
        /// <param name="contactMessageDTO"></param>
        /// <returns>true sau false</returns>
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