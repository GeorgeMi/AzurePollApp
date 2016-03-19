
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public partial class VotedForms
    {
        [Key, Column(Order = 0)]
        public int UserID { get; set; }
        [Key, Column(Order = 1)]
        public int FormID { get; set; }
        public virtual User User { get; set; }
        public virtual Form Form { get; set; }
    }
}
