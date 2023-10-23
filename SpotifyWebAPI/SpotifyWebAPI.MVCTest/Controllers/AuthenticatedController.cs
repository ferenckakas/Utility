using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyWebAPI.MVCTest.Controllers
{
    public class AuthenticatedController : Controller
    {
        // GET: Authenticated
        public ActionResult Index()
        {
            return View();
        }
    }
}