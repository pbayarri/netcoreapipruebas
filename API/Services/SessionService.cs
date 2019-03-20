using API.Entities;
using API.Helpers;
using OF.API.Base.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface ISessionService : ISessionServiceBasic<Session>
    {
        Session CreateSession(Session newSession, bool allowMultipleSessions);
    }

    public class SessionService : ISessionService
    {
        private readonly DataContext _context;

        public SessionService(DataContext context)
        {
            _context = context;
        }

        public Session CreateSession(Session newSession, bool allowMultipleSessions)
        {
            if (!allowMultipleSessions)
            {
                var query = from session in _context.Sessions
                            where session.UserId == newSession.UserId
                            select session;
                foreach (var session in query) session.IsActive = false;
                _context.SaveChanges();
            }

            _context.Sessions.Add(newSession);
            _context.SaveChanges();

            return newSession;
        }

        public IEnumerable<Session> GetValidSessions(int userId)
        {
            return _context.Sessions.Where(session => session.UserId == userId && session.IsActive);
        }

        public IEnumerable<string> GetValidTokens(int userId)
        {
            return _context.Sessions.Where(s => s.UserId == userId && s.IsActive).Select(s => s.Token);
        }

        public void UpdateAccess(int sessionId, DateTime accessDate)
        {
            Session session = _context.Sessions.Find(sessionId);
            session.LastAccess = accessDate;

            _context.Sessions.Update(session);
            _context.SaveChanges();
        }

        public void UpdateActive(int sessionId, bool active)
        {
            Session session = _context.Sessions.Find(sessionId);
            session.IsActive = active;

            _context.Sessions.Update(session);
            _context.SaveChanges();
        }
    }
}
