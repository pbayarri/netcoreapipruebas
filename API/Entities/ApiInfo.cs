using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("API")]
    public class ApiInfo
    {
        [Key]
        [Column("ApiId")]
        public int Id { get; set; }
        [Required]
        [Column("ApiType")]
        public int ApiType { get; set; }
        [Required]
        [Column("ApiTypeDesc")]
        public string ApiTypeDesc { get; set; }
        [Required]
        [Column("BaseHref")]
        public string BaseHref { get; set; }
    }
}
