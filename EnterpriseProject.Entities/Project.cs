using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Entities
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProjectTitle { get; set; } = String.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = String.Empty;

        public string ImagePath { get; set; } = String.Empty;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CompletedDate { get; set; }

        public bool IsPublic { get; set; } = false;

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; } //Foreign Key
        public virtual User User { get; set; }  //Navigation Property

        public List<Comment> Comments { get; set; }
    }
}
