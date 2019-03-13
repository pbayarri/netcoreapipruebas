using OF.API.Base.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("APIKEY")]
    public class ApiKey : IApiKeyAuthentication
    {
        [Key]
        [Column("ApiKeyId")]
        public int Id { get; set; }
        [Column("Key")]
        [Required]
        public string Key { get; set; }
        [Column("AllowImpersonate")]
        [Required]
        public bool AllowImpersonate { get; set; }

        public bool AllowImpersonation()
        {
            return AllowImpersonate;
        }

        public string GetKey()
        {
            return Key;
        }
    }
}
