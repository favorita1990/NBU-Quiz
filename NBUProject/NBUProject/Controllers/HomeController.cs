using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBUProject.Contex;

namespace NBUProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public DBContext db = new DBContext();


        public ActionResult Index()
        {
            //TempData["da"] = Guid.NewGuid().ToString();
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}