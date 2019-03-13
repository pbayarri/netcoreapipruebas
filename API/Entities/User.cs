using OF.API.Base.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("USER")]
    public class User : IUserAuthentication
    {
        [Key]
        [Column("UserId")]
        public int Id { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Required]
        [Column("Username")]
        public string Username { get; set; }
        [Column("PasswordHash")]
        public byte[] PasswordHash { get; set; }
        [Column("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }
        [Column("UserAccessToken")]
        public string UserAccessToken { get; set; }
        [Column("IsDefaultUserForApiKey")]
        public bool IsDefaultUserForApiKey { get; set; }

        public string GetGeneratedToken()
        {
            return Encoding.UTF8.GetString(PasswordSalt);
        }

        public int GetUserId()
        {
            return Id;
        }

        public string GetUserName()
        {
            return Username;
        }

        public string GetUserPasword()
        {
            return Encoding.UTF8.GetString(PasswordHash);
        }
    }
}
