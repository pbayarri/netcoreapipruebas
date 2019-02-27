using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OF.API.Base.Log
{
    public class TokenLogFilter : ILogFilter
    {
        private const string TokenKey = "\"token\"";
        private const string Pattern = "\"token\":\"(.*)\"";

        public string Filter(string text)
        {
            if (text.Contains(TokenKey, StringComparison.InvariantCultureIgnoreCase))
            {
                MatchCollection matches = Regex.Matches(text, Pattern);
                foreach (Match match in matches)
                {
                    text = Regex.Replace(text, Pattern, $"\"token\":\"{match.Groups[1].Value.Substring(0, 6).PadRight(16, '*')}\"");
                }
            }

            return text;
        }
    }
}
