using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Artist
{
    public partial class GetTopTracks : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // get alter bridge's top tracks
            var output = await SpotifyWebAPI.Artist.GetTopTracks("4DWX7u8BV0vZIQSpJQQDWU", "US");
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}