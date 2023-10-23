using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Album
{
    public partial class GetAlbumTracks : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // get alter bridge's blackbird album tracks
            var output = await SpotifyWebAPI.Album.GetAlbumTracks("0Fk4lWAADmFMmuW6jp6xyE");
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}