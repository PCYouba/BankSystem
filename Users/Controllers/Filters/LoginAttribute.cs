using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BankSystem.Users.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class LoginAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            BankSystem.Core.Model.LoginInfo CurrUser = Core.Code.UserState.GetUserState();
            if (CurrUser == null)
            {
                filterContext.Result = new RedirectResult("/Users/");
            }
        }
    }
}
