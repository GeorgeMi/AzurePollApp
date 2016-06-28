/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 * Role:
 *  Authentication logic. 
 *
 * History:
 * 12.02.2016    Miron George       Created class and implemented methods.
 */

using AzureDataAccess;
using Entities;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic
{
    /// <summary>
    /// Logica de autentificare
    /// </summary>
    public class AuthLogic
    {
        private IAzureDataAccess _dataAccess;
        private TokenLogic TokenLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDataAccess"></param>
        public AuthLogic(IAzureDataAccess objDataAccess)
        {
            // Primesc obiectul, nu e treaba AuthLogic ce dataAccess se foloseste
            // Unity are grija de dependency injection
            _dataAccess = objDataAccess;
            TokenLogic = new TokenLogic(_dataAccess);
        }
        /// <summary>
        /// Verifica daca in baza de date exista un tuplu ce corespunde cu datele introduse
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private int Validate(string username, string password)
        {
            try
            {
                // Creez token string
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] textToHash = Encoding.Default.GetBytes(password);
                byte[] result = md5.ComputeHash(textToHash);
                string passHash =  BitConverter.ToString(result);
                return _dataAccess.UserRepository.FindFirstBy(user => user.Username.Equals(username) && user.Password.Equals(passHash) && user.Verified == "yes").UserID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// Daca userul si pass carespund se updateaza tokenul in baza de date si se intoarce stringul updatat
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Authenticate(string username, string password)
        {
            int userID = Validate(username, password);

            if (userID == -1)
            {
                return null;
            }
            else
            {
                string token = TokenLogic.UpdateToken(userID, username, password);
                return token;
            }
        }

        /// <summary>
        /// Verifica daca tokenul corespunde cu cel trimis prin mail
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool VerifyMailToken(string token)
        {
            int userID;
            
            try
            {
                bool verify = VerifyTokenDate(token);

                if (verify)
                {
                    // Se updateaza contul userului, verified => true
                    userID = _dataAccess.TokenRepository.FindFirstBy(t => t.TokenString.Equals(token)).UserID;
                    _dataAccess.UserRepository.Verified(userID);
                    return true;
                }
                else
                {
                    // Tokenul nu corespunde
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica disponibilitatea numelui si introduce un nou user in baza de date
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public string Register(string username, string password, string email)
        {
            try
            {
                // Creez token string
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] textToHash = Encoding.Default.GetBytes(password);
                byte[] result = md5.ComputeHash(textToHash);
                string passHash = BitConverter.ToString(result);

                // Incearca sa adauge un nou user
                User user = new User() { Username = username, Password = passHash, Email = email, Role = "user" };
                _dataAccess.UserRepository.Add(user);
            }
            catch
            {
                // Numele exista deja in baza de date
                return "Name already exists";
            }

            var userID = Validate(username, password);
            if (userID == -1)
            {
                return "register failed";
            }
            else
            {
                return "register successfully";
            }
        }

        /// <summary>
        /// Verifica tokenul din baza de date. 
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public bool VerifyTokenDate(string tokenString)
        {
            // Daca nu exista sau este expirat->eroare. 
            // Altfel se updateaza data de expirare si se intoarce ok
            try
            {
                DateTime expirationDate = TokenLogic.GetTokenExpirationDate(tokenString);
                if (expirationDate.CompareTo(DateTime.Now) != 1)
                {
                    // Token-ul este expirat
                    return false;
                }
                else
                {
                    TokenLogic.UpdateTokenExpirationDate(tokenString);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Token-ul nu exista in baza de date
                return false;
            }
        }

        /// <summary>
        /// Verifica daca userul cu tokenul tokenString este sau nu admin
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public bool VerifyAdminToken(string tokenString)
        {
            try
            {
                string role = TokenLogic.GetRoleByToken(tokenString);

                if (role == "admin")
                {
                    // userul este admin
                    return true;
                }
                else
                {
                    // userul nu este admin
                    return false;
                }
            }
            catch (Exception ex)
            {
                // userul nu exista
                return false;
            }
        }
    }
}
