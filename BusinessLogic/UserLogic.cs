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

    /// <summary>
    /// Logica gestionarii utilizatorilor
    /// </summary>
    public class UserLogic
    {
        private IAzureDataAccess _dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDataAccess"></param>
        public UserLogic(IAzureDataAccess objDataAccess)
        {
            _dataAccess = objDataAccess;
        }

        /// <summary>
        /// Returnarea listei tuturor utilizatorilor
        /// </summary>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        /// <returns></returns>
        public List<UserDetailDTO> GetAllUsers(int page, int per_page)
        {
            List<User> userList = _dataAccess.UserRepository.GetAll().ToList();
            userList = userList.Skip(page*per_page).Take(per_page).ToList();
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

        /// <summary>
        /// Returnare rol utilizator
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetUserRole(string username)
        {
            return _dataAccess.UserRepository.FindFirstBy(u => u.Username.Equals(username)).Role;
        }

        /// <summary>
        /// Adaugare utilizator
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public int AddUser(UserRegistrationDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Username) || string.IsNullOrWhiteSpace(userDTO.Password) ||
                string.IsNullOrWhiteSpace(userDTO.Email))
            {
                throw new System.Exception("failed");
            }
            else
            {
                // Adauga un user
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] textToHash = Encoding.Default.GetBytes(userDTO.Password);
                byte[] result = md5.ComputeHash(textToHash);
                string passHash = BitConverter.ToString(result);

                User user = new User()
                {
                    Username = userDTO.Username,
                    Password = passHash,
                    Email = userDTO.Email,
                    Role = "user",
                    Verified = "no"
                };
                _dataAccess.UserRepository.Add(user);

                return _dataAccess.UserRepository.FindFirstBy(u => u.Username.Equals(userDTO.Username)).UserID;
            }
        }

        /// <summary>
        /// Returnarea listei cu toti userii si id-urile asociate
        /// </summary>
        /// <returns></returns>
        public List<UsernameDTO> GetAllUsernames()
        {
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

        /// <summary>
        /// Cautare user dupa id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDetailDTO GetUser(int id)
        {
            User user = _dataAccess.UserRepository.FindFirstBy(u => u.UserID == id);
            UserDetailDTO userDTO = new UserDetailDTO();

            userDTO.Username = user.Username;
            userDTO.Password = user.Password;
            userDTO.Email = user.Email;
            userDTO.Role = user.Role;
            userDTO.UserID = user.UserID;

            return userDTO;
        }

        /// <summary>
        /// Stergere utilizator
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUser(int id)
        {
            // Sterge user dupa id, dar care difera de contul de admin "Admin" care nu poate fi sters niciodata
            User u = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id && user.Username != "Admin");
            if (null == u)
            {
                throw new Exception("'Admin' cannot be deleted");
            }
            _dataAccess.UserRepository.Delete(u);
        }

        /// <summary>
        /// Stergere utilizator
        /// </summary>
        /// <param name="userDTO"></param>
        public void DeleteUser(UserDetailDTO userDTO)
        {
            if (userDTO.Username == "Admin")
            {
                throw new Exception("'Admin' cannot be deleted");
            }

            User user = new User()
            {
                Username = userDTO.Username,
                Password = userDTO.Password,
                Email = userDTO.Email,
                Role = userDTO.Role
            };
            _dataAccess.UserRepository.Delete(user);
        }

        /// <summary>
        /// Cautare id user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetUserID(string username)
        {
            return _dataAccess.UserRepository.FindFirstBy(user => user.Username.Equals(username)).UserID;
        }

        /// <summary>
        /// Cautare nume user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUsername(int id)
        {
            return _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id).Username;
        }

        /// <summary>
        /// Promovare utilizator
        /// </summary>
        /// <param name="id"></param>
        public void PromoteUser(int id)
        {
            // User devine admin
            User u = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id);
            _dataAccess.UserRepository.ChangeRole(id, "admin");
        }

        public void DemoteUser(int id)
        {
            // Admin devine user
            User u = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id && user.Username != "Admin");
            if (null == u)
            {
                throw new Exception("'Admin' cannot be demote");
            }
            _dataAccess.UserRepository.ChangeRole(id, "user");
        }

        /// <summary>
        /// Trimitere mail de confirmare
        /// </summary>
        /// <param name="token"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        public void SendAuthEmail(string token, string username, string email)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("votemypoll@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Welcome to VoteMyPoll";
            mail.Body = "<h3>Hello " + username + ", </h3>";
            mail.Body +=
                "<p>Thanks for signing up! Before you start, please verify your email address by clicking <a href=\"http://votemypoll.azurewebsites.net/#/?verifymail=" +
                token + "\">here</a>.</p>";
            mail.Body += "<p>This link will expire in 24 hours if it's not activated.</p>";
            mail.Body += "<h5>The VoteMyPoll team</h5>";
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("votemypoll@gmail.com", "votemypollteam1234");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        /// <summary>
        /// Actualizare conturi si sondaje
        /// </summary>
        public void ScheduledJobs()
        {
            _dataAccess.UserRepository.ScheduleDeleteUsers();
            _dataAccess.FormRepository.ScheduleUpdateForms();
        }
    }
}
