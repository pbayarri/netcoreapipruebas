using OF.API.Base.Hateoas;
using System.Collections.Generic;
using static OF.API.Front.Helpers.Functionalities;
using static OF.API.Front.Helpers.HateoasHelper;

namespace OF.API.Front.Hateoas
{
    public class HateoasCoreCollaborator : HateoasEmptyCollaborator
    {
        public string BaseHref { get; set; }

        public HateoasCoreCollaborator(string baseHref)
        {
            BaseHref = baseHref;
        }

        public override void CompleteLinks(UsersListDto users, List<string> allowedFunctionalities)
        {
            if (users.Links == null)
            {
                users.Links = new List<HateoaLink>();
            }
            if (allowedFunctionalities.Contains(FunctionalitiesList.UsersGet.ToString()))
            {
                users.Links.Add(new HateoaLink() { Href = $"{BaseHref}/users", Method = GET, Rel = FunctionalitiesList.UsersGet.ToString() });
            }
            if (allowedFunctionalities.Contains(FunctionalitiesList.UserCreate.ToString()))
            {
                users.Links.Add(new HateoaLink() { Href = $"{BaseHref}/users/register", Method = POST, Rel = FunctionalitiesList.UserCreate.ToString() });
            }
        }
        public override void CompleteLinks(UserDto user, List<string> allowedFunctionalities)
        {
            if (user.Links == null)
            {
                user.Links = new List<HateoaLink>();
            }
            if (allowedFunctionalities.Contains(FunctionalitiesList.UserGet.ToString()))
            {
                user.Links.Add(new HateoaLink() { Href = $"{BaseHref}/users/{user.Id}", Method = GET, Rel = FunctionalitiesList.UserGet.ToString() });
            }
            if (allowedFunctionalities.Contains(FunctionalitiesList.UserDelete.ToString()))
            {
                user.Links.Add(new HateoaLink() { Href = $"{BaseHref}/users/{user.Id}", Method = DELETE, Rel = FunctionalitiesList.UserDelete.ToString() });
            }
            if (allowedFunctionalities.Contains(FunctionalitiesList.UserUpdate.ToString()))
            {
                user.Links.Add(new HateoaLink() { Href = $"{BaseHref}/users/{user.Id}", Method = PUT, Rel = FunctionalitiesList.UserUpdate.ToString() });
            }
            if (allowedFunctionalities.Contains(FunctionalitiesList.UserUpdatePartial.ToString()))
            {
                user.Links.Add(new HateoaLink() { Href = $"{BaseHref}/users/{user.Id}", Method = PATCH, Rel = FunctionalitiesList.UserUpdatePartial.ToString() });
            }
        }
    }
}
