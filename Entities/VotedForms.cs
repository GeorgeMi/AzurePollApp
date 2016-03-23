
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
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
