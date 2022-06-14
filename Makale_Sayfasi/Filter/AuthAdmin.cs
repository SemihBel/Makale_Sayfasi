using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makale_Sayfasi.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Kullanici kullanici = filterContext.HttpContext.Session["login"] as Kullanici;

            if ( kullanici!= null && kullanici.AdminMi==false)
            {
                filterContext.Result = new RedirectResult("/Home/YetkisizErisim");
            }
        }
    }
}