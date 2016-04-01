/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 * Role:
 *  Category Model.
 *
 * History:
 * 25.02.2016    Miron George       Created class and implemented methods.
 */
using DataTransferObject;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class ContactModel
    {
        private BusinessLogic.BusinessLogic bl;
        public ContactModel()
        {
            IUnityContainer objContainer = new UnityContainer();
            objContainer.RegisterType<BusinessLogic.BusinessLogic>();
            objContainer.RegisterType<AzureDataAccess.IAzureDataAccess, AzureDataAccess.AzureDataAccess>();
            bl = objContainer.Resolve<BusinessLogic.BusinessLogic>();
        }

        public bool SendMessage(string token, ContactMessageDTO contactMessageDTO)
        {
            try
            {
                bl.MessageLogic.SendMessage(token, contactMessageDTO);
                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}