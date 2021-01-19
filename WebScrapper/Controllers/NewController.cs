using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebScrapper.Controllers
{
    public class NewController : Controller
    {
        private const string NewSessionn = "NewSession";
        // GET: New
        public ActionResult News()
        {
            if (Session[NewSessionn] != null)
            {
                var listsession = (List<blog>)Session[NewSessionn];
                return View(listsession);
            }
            else
            {
                return View();
            }
        }
    }
}