using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Hateoas
{
    public class ObjectWithHateoas
    {
        public IList<HateoaLink> Links { get; set; }
    }
}
