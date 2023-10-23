using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Artist
{
    public partial class GetArtistAlbums : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            var artist = await SpotifyWebAPI.Artist.GetArtist("4DWX7u8BV0vZIQSpJQQDWU");
            var output = await artist.GetAlbums();
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}