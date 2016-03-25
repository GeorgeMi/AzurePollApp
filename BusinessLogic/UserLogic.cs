/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 * Role:
 *  User logic. 
 *
 * History:
 * 10.02.2016    Miron George       Created class.
 */
using AzureDataAccess;
using DataTransferObject;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace BusinessLogic
{
    public class UserLogic
    {
        private IAzureDataAccess _dataAccess;
       
        public UserLogic(IAzureDataAccess objDataAccess)
        {
            //primesc obiectul, nu e treaba UserLogic ce dataAccess se foloseste
            //unity are grija de dependency injection

            _dataAccess = objDataAccess;
        }
        public List<UserDetailDTO> GetAllUsers()
        {
            //returneaza lista cu toti userii
            List<User> userList = _dataAccess.UserRepository.GetAll().ToList();
            List<UserDetailDTO> userDtoList = new List<UserDetailDTO>();
            UserDetailDTO userDTO;

            foreach (User u in userList)
            {
                userDTO = new UserDetailDTO();
                userDTO.Email = u.Email;
                userDTO.Password = u.Password;
                userDTO.Role = u.Role;
                userDTO.Username = u.Username;
                userDTO.UserID = u.UserID;

                userDtoList.Add(userDTO);
            }

            return userDtoList;
        }

        public string GetUserRole(string username)
        {
            return _dataAccess.UserRepository.FindFirstBy(u => u.Username == username).Role;
        }

        public int AddUser(UserRegistrationDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Username) || string.IsNullOrWhiteSpace(userDTO.Password) || string.IsNullOrWhiteSpace(userDTO.Email))
            {
                throw new System.Exception("failed");
            }
            else
            {
                //adauga un user
                User user = new User() { Username = userDTO.Username, Password = userDTO.Password, Email = userDTO.Email, Role = "user" ,Verified="no"};
                _dataAccess.UserRepository.Add(user);

               
                return _dataAccess.UserRepository.FindFirstBy(u => u.Username == userDTO.Username).UserID;
                 
            }
           
        }
        public UserDetailDTO GetUser(int id)
        {
            //gaseste user dupa id
            User user = _dataAccess.UserRepository.FindFirstBy(u => u.UserID == id);
            UserDetailDTO userDTO = new UserDetailDTO();

            userDTO.Username = user.Username;
            userDTO.Password = user.Password;
            userDTO.Email = user.Email;
            userDTO.Role = user.Role;
            userDTO.UserID = user.UserID;

            return userDTO;
        }
        public void DeleteUser(int id)
        {
            //sterge user dupa id
            User u = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id);
            _dataAccess.UserRepository.Delete(u);
        }
        public void DeleteUser(UserDetailDTO userDTO)
        {
            //sterge user
            User user = new User() { Username = userDTO.Username, Password = userDTO.Password, Email = userDTO.Email, Role = userDTO.Role };

            _dataAccess.UserRepository.Delete(user);
        }
        public int GetUserID(string username)
        {
            //cauta id-ul userului dupa username
            return _dataAccess.UserRepository.FindFirstBy(user => user.Username == username).UserID;
        }

        public string GetUsername(int id)
        {
            //cauta numele userului dupa id
            return _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id).Username;
        }
        public void PromoteUser(int id)
        {
            //user devine admin
            User u = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id);
            _dataAccess.UserRepository.ChangeRole(id, "admin");
        }
        public void DemoteUser(int id)
        {
            //admin devine user
            User u = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id);
            _dataAccess.UserRepository.ChangeRole(id, "user");
        }

        public void Send_email(string token, string username, string email)
        {
            //trimite mail de confirmare

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("votemypoll@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Welcome to VoteMyPoll";
            mail.Body = "<h3>Hello " + username + ", </h3>";
            mail.Body += "<p>Thanks for signing up! Before you start, please verify your email address by clicking <a href=\"http://votemypoll.azurewebsites.net/#/?verifymail=" + token + "\">here</a>.</p>";
            mail.Body += "<p>This link will expire in 24 hours if it's not activated.</p>";
            mail.Body += "<h5>The VoteMyPoll team</h5>";
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("votemypoll@gmail.com", "votemypollteam1234");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }


        public void ScheduledJobs()
        {
            _dataAccess.UserRepository.ScheduleDeleteUsers();
            _dataAccess.FormRepository.ScheduleUpdateForms();
        }






    }
}
