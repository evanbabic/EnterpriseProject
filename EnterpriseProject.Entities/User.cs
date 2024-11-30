using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string UserName { get; set; } = String.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;

        public string? AboutMe { get; set; }

        // Navigation Properties
        public List<Project> Projects { get; set; } = new List<Project>();
        public Resume? Resume { get; set; }
    }
}
