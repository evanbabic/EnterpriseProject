using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Entities
{
    public class Resume
    {
        public const string Directory = "Resumes";
        public const string FileExtension = ".pdf";

        [Key]
        public int ResumeId { get; set; }

        [Required]
        public string FilePath { get; set; } = String.Empty;
        public string RelativeFilePath => Path.Combine("/" + Directory, UserId.ToString(), FilePath);

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
