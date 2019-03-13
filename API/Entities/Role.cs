using OF.API.Base.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("ROL")]
    public class Role
    {
        [Key]
        [Column("RolId")]
        public int Id { get; set; }
        [Required]
        [Column("Name")]
        public string Name { get; set; }
    }
}
