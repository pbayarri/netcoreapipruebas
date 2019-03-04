using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Utils
{
    public static class Util
    {
        public static bool IsTrue(string value) => value.ToLowerInvariant().Equals("true");
        public static int ToInt(string value) => int.Parse(value, System.Globalization.NumberStyles.Integer);
    }
}
