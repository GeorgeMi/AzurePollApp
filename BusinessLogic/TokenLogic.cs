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
    public class TokenLogic
    {
        private IAzureDataAccess _dataAccess;

        public TokenLogic(IAzureDataAccess objDataAccess)
        {
            // primesc obiectul, nu e treaba TokenLogic ce dataAccess se foloseste
            // unity are grija de dependency injection
            _dataAccess = objDataAccess;
        }
        public void AddToken(Token token)
        {
            _dataAccess.TokenRepository.Add(token);
        }

        public Token GetToken(int id)
        {
            return _dataAccess.TokenRepository.FindFirstBy(token => token.TokenID == id);
        }
        public int GetTokenID(string tokenString)
        {
            return _dataAccess.TokenRepository.FindFirstBy(token => token.TokenString == tokenString).TokenID;
        }
        public Token GetTokenByUserID(int id)
        {
            return _dataAccess.TokenRepository.FindFirstBy(token => token.UserID == id);
        }

        public string GetRoleByToken(string tokenString)
        {
            int userID = _dataAccess.TokenRepository.FindFirstBy(token => token.TokenString == tokenString).UserID;
            return _dataAccess.UserRepository.FindFirstBy(user => user.UserID == userID).Role;
        }

        public string UpdateToken(int id, string username, string password)
        {
            Token t;
            string text;
            MD5 md5;
            byte[] textToHash;
            byte[] result;

            try
            {
                // daca exista un token pentru user preiau obiectul
                t = GetTokenByUserID(id);
                var createdDate = DateTime.Now;
                var expirationDate = DateTime.Now.AddHours(3);

                // preiau adresa mac
                var MAC = NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
              
                // creez token string
                text = t.TokenID + username + password + createdDate + MAC;

                md5 = new MD5CryptoServiceProvider();
                textToHash = Encoding.Default.GetBytes(text);
                result = md5.ComputeHash(textToHash);

                // conversie la string 
                text = BitConverter.ToString(result);

                try
                {
                    // verificare update
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

                // creez token string
                text = t.TokenID + username + t.CreatedDate;

                md5 = new MD5CryptoServiceProvider();
                textToHash = Encoding.Default.GetBytes(text);
                result = md5.ComputeHash(textToHash);

                // conversie la string 
                t.TokenString = BitConverter.ToString(result);

                try
                {
                    // verificare inserare
                    _dataAccess.TokenRepository.Add(t);
                }
                catch (Exception ex)
                {
                    return ex.InnerException.InnerException.Message;
                }

                return t.TokenString;
            }
        }
        public DateTime GetTokenExpirationDate(string tokenString)
        {
            return _dataAccess.TokenRepository.FindFirstBy(token => token.TokenString == tokenString).ExpirationDate;
        }

        public void UpdateTokenExpirationDate(string tokenString)
        {
            int id = GetTokenID(tokenString);
            _dataAccess.TokenRepository.UpdateExpirationDate(id, DateTime.Now.AddHours(3));
        }
    }
}
