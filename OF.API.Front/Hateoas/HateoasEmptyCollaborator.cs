using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Front.Hateoas
{
    /// <summary>
    /// Clase base de todos los colaboradores de HATEOAS para que las instancias finales sólo tengan que implementar aquellos métodos que quieran 
    /// que devuelvan links propios
    /// </summary>
    public class HateoasEmptyCollaborator : IHateoasCollaborator
    {
        public virtual void CompleteLinks(UsersListDto users, List<string> allowedFunctionalities)
        {
        }

        public virtual void CompleteLinks(UserDto user, List<string> allowedFunctionalities)
        {
        }
    }
}
