/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// Clasa ce corespunde tabelului "Question" din baza de date
    /// </summary>
    public partial class Question
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Question()
        {
            this.Answers = new List<Answer>();
        }

        public int QuestionID { get; set; }
        public int FormID { get; set; }
        public string Content { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Form Form { get; set; }
    }
}
