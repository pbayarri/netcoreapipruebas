﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Hateoas
{
    public class HateoaLink
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }
}
