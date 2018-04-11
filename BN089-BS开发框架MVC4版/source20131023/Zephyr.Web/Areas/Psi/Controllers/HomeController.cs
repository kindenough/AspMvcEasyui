using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zephyr.Areas.Psi.Controllers
{
    public class HomeController : Controller
    {
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("~/Login/psi");

            return Content("");
        }

    }
}
