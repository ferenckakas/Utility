using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Album
{
    public partial class GetArtistAlbums : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // get alter bridge's blackbird album
            var output = await SpotifyWebAPI.Album.GetArtistAlbums("4DWX7u8BV0vZIQSpJQQDWU");
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}