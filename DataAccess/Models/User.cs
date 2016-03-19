using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            this.Forms = new List<Form>();
            this.Tokens = new List<Token>();
            this.Forms1 = new List<Form>();
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
        public virtual ICollection<Form> Forms1 { get; set; }
    }
}
