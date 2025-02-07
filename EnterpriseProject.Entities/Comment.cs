﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Entities
{
	/// <summary>
	/// AUTHOR: EVAN
	/// Model representing a comment that can either be on a profile or a project.
    /// References the user who made it, profile its on, or project its on
	/// </summary>
	public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Profile")]
        public int? ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
