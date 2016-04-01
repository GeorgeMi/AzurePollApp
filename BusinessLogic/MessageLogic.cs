using AzureDataAccess;
using DataTransferObject;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Mail;

namespace BusinessLogic
{
    public class MessageLogic
    {
        private IAzureDataAccess _dataAccess;

        public MessageLogic(IAzureDataAccess objDataAccess)
        {
            //primesc obiectul, nu e treaba UserLogic ce dataAccess se foloseste
            //unity are grija de dependency injection

            _dataAccess = objDataAccess;
        }

        public void SendMessage(string token, ContactMessageDTO contactMessageDTO)
        {
            int userID = _dataAccess.TokenRepository.FindFirstBy(t=>t.TokenString == token).UserID;
            string username = _dataAccess.UserRepository.FindFirstBy(u => u.UserID == userID).Username;
            //trimite mail de confirmare
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("votemypoll@gmail.com");
            mail.To.Add("george.miron2003@gmail.com");
            mail.Subject = contactMessageDTO.Category;
            mail.Body = "<h3>Message from " + username + ", </h3>";
            mail.Body += "<p>"+contactMessageDTO.Message+"</p>";
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("votemypoll@gmail.com", "votemypollteam1234");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }       
    
    }
}
