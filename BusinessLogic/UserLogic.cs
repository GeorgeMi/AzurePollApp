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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

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
        public List<UserDetailDTO> GetAllUsers(int page, int per_page)
        {
            //returneaza lista cu toti userii
            List<User> userList = _dataAccess.UserRepository.GetAll().ToList();
            userList = userList.Skip(page * per_page).Take(per_page).ToList();
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

            return userDtoList.ToList();
        }

        public string GetUserRole(string username)
        {
            return _dataAccess.UserRepository.FindFirstBy(u => u.Username.Equals(username)).Role;
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
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] textToHash = Encoding.Default.GetBytes(userDTO.Password);
                byte[] result = md5.ComputeHash(textToHash);
                string passHash = BitConverter.ToString(result);

                User user = new User() { Username = userDTO.Username, Password = passHash, Email = userDTO.Email, Role = "user", Verified = "no" };
                _dataAccess.UserRepository.Add(user);
                
                return _dataAccess.UserRepository.FindFirstBy(u => u.Username.Equals(userDTO.Username)).UserID;
            }
        }

        public List<UsernameDTO> GetAllUsernames()
        {
            //returneaza lista cu toti userii si id-urile
            List<User> userList = _dataAccess.UserRepository.GetAll().ToList();
            List<UsernameDTO> userDtoList = new List<UsernameDTO>();
            UsernameDTO userDTO;

            foreach (User u in userList)
            {
                userDTO = new UsernameDTO();
                userDTO.Username = u.Username;
                userDTO.UserID = u.UserID;

                userDtoList.Add(userDTO);
            }

            return userDtoList.ToList();
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
            //sterge user dupa id, dar care difera de contul de admin "Admin" care nu poate fi sters niciodata
            User u = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id && user.Username != "Admin" );
            if (null == u)
            {
                throw new Exception("'Admin' cannot be deleted");
            }
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
            return _dataAccess.UserRepository.FindFirstBy(user => user.Username.Equals(username)).UserID;
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
            User u = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id && user.Username != "Admin");
            if (null == u)
            {
                throw new Exception("'Admin' cannot be demote");
            }
            _dataAccess.UserRepository.ChangeRole(id, "user");
        }

        public void SendAuthEmail(string token, string username, string email)
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
