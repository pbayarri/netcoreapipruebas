using System.Collections.Generic;

namespace OF.API.Front.Hateoas
{
    public interface IHateoasCollaborator
    {
        void CompleteLinks(UsersListDto users, List<string> allowedFunctionalities);
        void CompleteLinks(UserDto user, List<string> allowedFunctionalities);
    }
}
