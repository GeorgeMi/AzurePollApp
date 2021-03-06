﻿/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using AzureDataAccess;
using DataTransferObject;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace BusinessLogic
{
    /// <summary>
    /// Logica gestionarii mesajelor
    /// </summary>
    public class MessageLogic
    {
        private IAzureDataAccess _dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDataAccess"></param>
        public MessageLogic(IAzureDataAccess objDataAccess)
        {
            // Primesc obiectul, nu e treaba UserLogic ce dataAccess se foloseste
            // Unity are grija de dependency injection
            _dataAccess = objDataAccess;
        }

        /// <summary>
        /// Trimitere mesaj
        /// </summary>
        /// <param name="token"></param>
        /// <param name="contactMessageDTO"></param>
        public void SendMessage(string token, ContactMessageDTO contactMessageDTO)
        {
            if (string.IsNullOrWhiteSpace(contactMessageDTO.Category) || string.IsNullOrWhiteSpace(contactMessageDTO.Message) || contactMessageDTO.Receiver < 0)
                throw new System.Exception("Values cannot be null");

            List<User> adminsList;
            User receiver;
            int userID = _dataAccess.TokenRepository.FindFirstBy(t => t.TokenString.Equals(token)).UserID;
            string username = _dataAccess.UserRepository.FindFirstBy(u => u.UserID == userID).Username;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("votemypoll@gmail.com");
            mail.Subject = contactMessageDTO.Category + " from " + username;
            mail.Body = "<p>" + contactMessageDTO.Message + "</p>";
            mail.IsBodyHtml = true;

            if (contactMessageDTO.Receiver == 0)
            {
                adminsList = _dataAccess.UserRepository.FindAllBy(user => user.Role.Equals("admin")).ToList();
                foreach (User admin in adminsList)
                {
                    mail.Bcc.Add(new MailAddress(admin.Email));
                }
            }
            else
            {
                receiver = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == contactMessageDTO.Receiver);
                mail.To.Add(new MailAddress(receiver.Email));
                mail.Bcc.Add(new MailAddress("george.miron2003@gmail.com"));
            }

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("votemypoll@gmail.com", "votemypollteam1234");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
