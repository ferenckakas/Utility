using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyWebAPI.Test
{
    public class ValidateSession
    {
        public static void Validate(HttpContext current, HttpResponse response)
        {
            if (current.Session.Count == 0)
                response.Redirect("~/Authentication/Default.aspx");

            try
            {
                if (current.Session["clientid"] == null || 
                    current.Session["clientsecret"] == null || 
                    current.Session["redirectUri"] == null || 
                    current.Session["clientid"] == null)
                    response.Redirect("~/Authentication/Default.aspx");
            }
            catch
            {
                response.Redirect("~/Authentication/Default.aspx");
            }
        }
    }
}