using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Category
    {
        public Category()
        {
            this.Forms = new List<Form>();
        }

        public int CategoryID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
    }
}
