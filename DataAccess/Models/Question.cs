using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Question
    {
        public Question()
        {
            this.Answers = new List<Answer>();
        }

        public int QuestionID { get; set; }
        public int FormID { get; set; }
        public decimal NrVotes { get; set; }
        public string Content { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Form Form { get; set; }
    }
}
