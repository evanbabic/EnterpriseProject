using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Entities
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public string ProfilePicturePath { get; set; } = String.Empty;
        public string BannerImagePath { get; set; } = String.Empty;
        public string AboutMe { get; set; } = String.Empty;

        //Navigation Properties
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
