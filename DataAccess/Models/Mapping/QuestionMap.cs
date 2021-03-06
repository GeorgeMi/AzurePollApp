using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Models.Mapping
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.QuestionID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Question");
            this.Property(t => t.QuestionID).HasColumnName("QuestionID");
            this.Property(t => t.FormID).HasColumnName("FormID");
            this.Property(t => t.NrVotes).HasColumnName("NrVotes");
            this.Property(t => t.Content).HasColumnName("Content");

            // Relationships
            this.HasRequired(t => t.Form)
                .WithMany(t => t.Questions)
                .HasForeignKey(d => d.FormID);

        }
    }
}
