using OF.API.Base.Hateoas;

namespace OF.API.Front
{
    public class UserDto : ObjectWithHateoas
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
