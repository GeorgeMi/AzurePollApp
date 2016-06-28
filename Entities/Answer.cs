/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

namespace Entities
{
    /// <summary>
    /// Clasa ce corespunde tabelului "Answer" din baza de date
    /// </summary>
    public partial class Answer
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public string Content { get; set; }
        public decimal NrVotes { get; set; }
        public virtual Question Question { get; set; }
    }
}
