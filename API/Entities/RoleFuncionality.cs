using OF.API.Base.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("ROLFUNCTIONALITY")]
    public class RoleFuncionality
    {
        [Key]
        [Column("FunctionalityId")]
        public int Id { get; set; }
        [Column("Name")]
        [Required]
        public string Name { get; set; }
    }
}
