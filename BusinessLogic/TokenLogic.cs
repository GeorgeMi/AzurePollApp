/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 * Role:
 *  Token logic. 
 *
 * History:
 * 10.02.2016    Miron George       Created class.
 */

using AzureDataAccess;
using Entities;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic
{
    /// <summary>
    /// Logica gestionarii token-urilor
    /// </summary>
    public class TokenLogic
    {
        private IAzureDataAccess _dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDataAccess"></param>
        public TokenLogic(IAzureDataAccess objDataAccess)
        {
            _dataAccess = objDataAccess;
        }
        /// <summary>
        /// Adaugare token
        /// </summary>
        /// <param name="token"></param>
        public void AddToken(Token token)
        {
            _dataAccess.TokenRepository.Add(token);
        }

        /// <summary>
        /// Returnare token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Token GetToken(int id)
        {
            return _dataAccess.TokenRepository.FindFirstBy(token => token.TokenID == id);
        }
        /// <summary>
        /// Returnare id token
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public int GetTokenID(string tokenString)
        {
            return _dataAccess.TokenRepository.FindFirstBy(token => token.TokenString == tokenString).TokenID;
        }
        /// <summary>
        /// Returnare id token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Token GetTokenByUserID(int id)
        {
            return _dataAccess.TokenRepository.FindFirstBy(token => token.UserID == id);
        }

        /// <summary>
        /// Returnare rol
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public string GetRoleByToken(string tokenString)
        {
            int userID = _dataAccess.TokenRepository.FindFirstBy(token => token.TokenString == tokenString).UserID;
            return _dataAccess.UserRepository.FindFirstBy(user => user.UserID == userID).Role;
        }

        /// <summary>
        /// Actualizare token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string UpdateToken(int id, string username, string password)
        {
            Token t;
            string text;
            MD5 md5;
            byte[] textToHash;
            byte[] result;

            try
            {
                // Daca exista un token pentru user preiau obiectul
                t = GetTokenByUserID(id);
                var createdDate = DateTime.Now;
                var expirationDate = DateTime.Now.AddHours(3);

                // Preiau adresa mac
                var MAC = NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
              
                // Creez token string
                text = t.TokenID + username + password + createdDate + MAC;

                md5 = new MD5CryptoServiceProvider();
                textToHash = Encoding.Default.GetBytes(text);
                result = md5.ComputeHash(textToHash);

                // Conversie la string 
                text = BitConverter.ToString(result);

                try
                {
                    // Verificare update
                    _dataAccess.TokenRepository.UpdateToken(t.TokenID, createdDate, expirationDate, text);
                }
                catch (Exception ex)
                {
                    return ex.InnerException.InnerException.Message;
                }
                return text;

            }
            catch
            {
                t = new Token();
                t.UserID = id;

                t.CreatedDate = DateTime.Now;
                t.ExpirationDate = t.CreatedDate.AddHours(3);

                // Creez token string
                text = t.TokenID + username + t.CreatedDate;

                md5 = new MD5CryptoServiceProvider();
                textToHash = Encoding.Default.GetBytes(text);
                result = md5.ComputeHash(textToHash);

                // Conversie la string 
                t.TokenString = BitConverter.ToString(result);

                try
                {
                    // Verificare inserare
                    _dataAccess.TokenRepository.Add(t);
                }
                catch (Exception ex)
                {
                    return ex.InnerException.InnerException.Message;
                }

                return t.TokenString;
            }
        }
        /// <summary>
        /// Returnare data expirare token
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public DateTime GetTokenExpirationDate(string tokenString)
        {
            return _dataAccess.TokenRepository.FindFirstBy(token => token.TokenString == tokenString).ExpirationDate;
        }

        /// <summary>
        /// Actualizare data expirare
        /// </summary>
        /// <param name="tokenString"></param>
        public void UpdateTokenExpirationDate(string tokenString)
        {
            int id = GetTokenID(tokenString);
            _dataAccess.TokenRepository.UpdateExpirationDate(id, DateTime.Now.AddHours(3));
        }
    }
}
