/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// Clasa ce corespunde tabelului "User" din baza de date
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public User()
        {
            this.Forms = new List<Form>();
            this.Tokens = new List<Token>();
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Verified { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
