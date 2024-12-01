using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseProject.Entities
{
    public class Resume
    {
        [Key]
        public int ResumeId { get; set; }

        [Required]
        public string FilePath { get; set; } = String.Empty;

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
