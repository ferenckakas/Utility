using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyWebAPI.MVCTest.Controllers
{
    public class TrackController : Controller
    {
        // GET: Track
        public ActionResult GetTrack()
        {
            return View();
        }
    }
}