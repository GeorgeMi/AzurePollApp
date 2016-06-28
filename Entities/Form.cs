/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// Clasa ce corespunde tabelului "Form" din baza de date
    /// </summary>
    public partial class Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Form()
        {
            this.Questions = new List<Question>();
            this.VotedForms = new List<VotedForm>();
        }

        public int FormID { get; set; }
        public int UserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public int NrVotes { get; set; }
        public System.DateTime Deadline { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<VotedForm> VotedForms { get; set; }
    }
}
