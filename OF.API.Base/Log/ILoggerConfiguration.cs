using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Log
{
    public interface ILogFilter
    {
        string Filter(string text);
    }
    public interface ILoggerFilters
    {
        IEnumerable<ILogFilter> GetFilters();
    }
}
