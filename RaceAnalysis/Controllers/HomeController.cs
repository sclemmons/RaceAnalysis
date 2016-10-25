﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "TBD....";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Information...";

            return View();
        }
    }
}