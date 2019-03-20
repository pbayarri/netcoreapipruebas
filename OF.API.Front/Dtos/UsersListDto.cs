using OF.API.Base.Hateoas;
using System.Collections.Generic;

namespace OF.API.Front
{
    public class UsersListDto : ObjectWithHateoas
    {
        public List<UserDto> Users { get; set; }
    }
}
