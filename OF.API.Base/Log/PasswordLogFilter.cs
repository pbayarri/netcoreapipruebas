using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OF.API.Base.Log
{
    public class PasswordLogFilter : ILogFilter
    {
        private const string PasswordKey = "\"password\"";
        private const string Pattern = "\"password\": \"(.*)\"";

        public string Filter(string text)
        {
            if (text.Contains(PasswordKey, StringComparison.InvariantCultureIgnoreCase))
            {
                string pass;
                MatchCollection matches = Regex.Matches(text, Pattern);
                foreach (Match match in matches)
                {
                    pass = match.Groups[1].Value;
                    pass = pass.Length > 3 ? pass.Substring(0, 2) : string.Empty;
                    pass = pass.PadRight(8, '*');
                    text = Regex.Replace(text, Pattern, $"\"password\": \"{pass}\"");
                }
            }

            return text;
        }
    }
}
