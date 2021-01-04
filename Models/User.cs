using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Models
{
    public record User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(320)]
        [Column(TypeName = "varchar(320)")]
        public string Email { get; set; }

        [Required]
        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(512)]
        [Column(TypeName = "varchar(512)")]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string Name { get; set; }

        public List<Scope> Scopes { get; set; } = new List<Scope>();
    }
}
