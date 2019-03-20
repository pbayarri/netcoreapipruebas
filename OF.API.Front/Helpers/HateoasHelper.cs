using Microsoft.Extensions.DependencyInjection;
using OF.API.Front.Hateoas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OF.API.Front.Helpers
{
    public interface IHateoasHelper : IHateoasCollaborator
    {
        void AddCollaborator(int type, string baseHref);
    }

    public class HateoasHelper : IHateoasHelper
    {
        public enum CollaboratorType
        {
            Core = 0,
            Reporting = 1,
            Advisor = 2,
            Crm = 3
            // y los demás...
        }

        private List<IHateoasCollaborator> _collaborators;
        public HateoasHelper()
        {
            _collaborators = new List<IHateoasCollaborator>();
        }

        internal const string GET = "GET";
        internal const string POST = "POST";
        internal const string PUT = "PUT";
        internal const string DELETE = "DELETE";
        internal const string PATCH = "PATCH";

        public void AddCollaborator(int type, string baseHref)
        {
            switch ((CollaboratorType)type)
            {
                case CollaboratorType.Core:
                    _collaborators.Add(new HateoasCoreCollaborator(baseHref));
                    break;
                case CollaboratorType.Reporting:
                    _collaborators.Add(new HateoasReportingCollaborator(baseHref));
                    break;
                case CollaboratorType.Advisor:
                    _collaborators.Add(new HateoasAdvisorCollaborator(baseHref));
                    break;
                default:
                    throw new NotImplementedException($"Hateoas type {type} is not implemented");
            }
        }

        public void CompleteLinks(UsersListDto users, List<string> allowedFunctionalities)
        {
            users.Users.ForEach(user => _collaborators.ForEach(collaborator => collaborator.CompleteLinks(user, allowedFunctionalities)));
            _collaborators.ForEach(collaborator => collaborator.CompleteLinks(users, allowedFunctionalities));
        }
        public void CompleteLinks(UserDto user, List<string> allowedFunctionalities)
        {
            _collaborators.ForEach(collaborator => collaborator.CompleteLinks(user, allowedFunctionalities));
        }
    }
}
