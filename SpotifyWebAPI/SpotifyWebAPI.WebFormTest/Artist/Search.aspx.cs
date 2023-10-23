using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Artist
{
    public partial class Search : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // get search for alter bridge 
            var output = await SpotifyWebAPI.Artist.Search("Alter Bridge");
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}