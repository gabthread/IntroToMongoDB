using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDbDemo.App_Start;
using MongoDbDemo.Properties;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbDemo.Controllers
{
    public class HomeController : Controller
    {
       public RealEstateContext Context = new RealEstateContext();

        public ActionResult Index()
        {
            return Json(Context.MongoDatabase.Client.Settings.ToJson(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}