using System.Collections.Generic;
using OF.API.Base.Hateoas;
using static OF.API.Front.Helpers.Functionalities;
using static OF.API.Front.Helpers.HateoasHelper;

namespace OF.API.Front.Hateoas
{
    public class HateoasReportingCollaborator : HateoasEmptyCollaborator
    {
        public string BaseHref { get; set; }

        public HateoasReportingCollaborator(string baseHref)
        {
            BaseHref = baseHref;
        }
        
        public override void CompleteLinks(UserDto user, List<string> allowedFunctionalities)
        {
            // Fake para pruebas
            user.Links.Add(new HateoaLink() { Href = $"{BaseHref}/reporting/{user.Id}", Method = GET, Rel = FunctionalitiesList.UserGet.ToString() });
        }
    }
}
