using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseProject.Entities
{
	/// <summary>
    /// Author: Nicholas
	/// Represents a user's resume with associated file path and user information.
	/// The Resume class is linked to a User entity via the UserId.
	/// </summary>
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
