using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Answer
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public decimal NrVotes { get; set; }
        public string Content { get; set; }
        public virtual Question Question { get; set; }
    }
}
