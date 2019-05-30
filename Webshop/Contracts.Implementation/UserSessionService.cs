using System.Web;
using Microsoft.AspNet.Identity;

namespace Contracts.Implementation
{
    public class UserSessionService : IUserSessionService
    {
        private readonly HttpContextBase _httpContextBase;

        public UserSessionService(HttpContextBase httpContextBase)
        {
            _httpContextBase = httpContextBase;
        }

        public string GetUserID()
        {
            if (_httpContextBase.User.Identity.IsAuthenticated)
            {
                return _httpContextBase.User.Identity.GetUserId();
            }
            else
            {
                return _httpContextBase.Session.SessionID;
            }
        }
    }
}