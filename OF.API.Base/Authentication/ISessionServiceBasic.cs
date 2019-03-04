using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Authentication
{
    public interface ISessionServiceBasic<S> where S : ISessionAuthetication
    {
        IEnumerable<string> GetValidTokens(int userId);
        IEnumerable<S> GetValidSessions(int userId);
        void UpdateAccess(int sessionId, DateTime accessDate);
        void UpdateActive(int sessionId, bool active);
    }
}
