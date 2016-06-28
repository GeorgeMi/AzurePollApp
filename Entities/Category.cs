/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// Clasa ce corespunde tabelului "Category" din baza de date
    /// </summary>
    public partial class Category
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Category()
        {
            this.Forms = new List<Form>();
        }

        public int CategoryID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
    }
}
