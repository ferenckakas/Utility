using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Album
{
    public partial class GetAlbums : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            var albums = new List<string>();
            albums.Add("0Fk4lWAADmFMmuW6jp6xyE"); // blackbird
            albums.Add("14XXkmq6rjlKTxRkelMtZx"); // fortress
            albums.Add("0ocWLhnTiUKSq8Ksa3FX4Z"); // AB III

            var output = await SpotifyWebAPI.Album.GetAlbums(albums);
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}