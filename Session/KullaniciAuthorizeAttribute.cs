using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AracSatis.Session
{
    public class KullaniciAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["NormalUserID"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}