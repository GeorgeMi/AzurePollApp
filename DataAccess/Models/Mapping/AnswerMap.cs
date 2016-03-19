using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Models.Mapping
{
    public class AnswerMap : EntityTypeConfiguration<Answer>
    {
        public AnswerMap()
        {
            // Primary Key
            this.HasKey(t => t.AnswerID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Answer");
            this.Property(t => t.AnswerID).HasColumnName("AnswerID");
            this.Property(t => t.QuestionID).HasColumnName("QuestionID");
            this.Property(t => t.NrVotes).HasColumnName("NrVotes");
            this.Property(t => t.Content).HasColumnName("Content");

            // Relationships
            this.HasRequired(t => t.Question)
                .WithMany(t => t.Answers)
                .HasForeignKey(d => d.QuestionID);

        }
    }
}
