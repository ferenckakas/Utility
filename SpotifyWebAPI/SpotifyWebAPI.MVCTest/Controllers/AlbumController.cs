using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyWebAPI.MVCTest.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Album
        public ActionResult GetAlbum()
        {
            return View();
        }
    }
}