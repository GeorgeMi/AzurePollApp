/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Clasa ce corespunde tabelului "VotedForm" din baza de date
    /// </summary>
    public partial class VotedForm
    {
        [Key]
        [Column(Order = 1)]
        public int UserID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int FormID { get; set; }
        public virtual Form Form { get; set; }
    }
}
