using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Artist
{
    public partial class GetArtists : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            List<string> artists = new List<string>();
            artists.Add("4DWX7u8BV0vZIQSpJQQDWU"); // alter bridge

            var output = await SpotifyWebAPI.Artist.GetArtists(artists);
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}