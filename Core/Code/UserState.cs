using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BankSystem.Core.Code
{
   public class UserState
    {
        public static void SaveUserState(BankSystem.Core.Model.LoginInfo CurrUser)
        {
            HttpContext.Current.Session["LoginUser"] = CurrUser;
        }

        public static BankSystem.Core.Model.LoginInfo GetUserState()
        {
            BankSystem.Core.Model.LoginInfo CurrUser = (BankSystem.Core.Model.LoginInfo)HttpContext.Current.Session["LoginUser"];
            return CurrUser;
        }
    }
}
