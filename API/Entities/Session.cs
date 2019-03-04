using OF.API.Base.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("SESSION")]
    public class Session : ISessionAuthetication
    {
        [Key]
        [Column("SessionId")]
        public int Id { get; set; }
        [Column()]
        [Required]
        public int UserId { get; set; }
        [Column()]
        [Required]
        public DateTime CreatedAt { get; set; }
        [Column()]
        [Required]
        public string Ip { get; set; }
        [Column()]
        [Required]
        public DateTime LastAccess { get; set; }
        [Column()]
        [Required]
        public string Token { get; set; }
        [Column()]
        [Required]
        public bool IsActive { get; set; }

        public int GetId()
        {
            return Id;
        }

        public DateTime GetLastAccess()
        {
            return LastAccess;
        }

        public string GetToken()
        {
            return Token;
        }
    }
}
