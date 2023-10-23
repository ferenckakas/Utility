using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Track
{
    public partial class GetTracks : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            List<string> tracks = new List<string>();
            tracks.Add("0eGsygTp906u18L0Oimnem");
            tracks.Add("1lDWb6b6ieDQ2xT7ewTC3G");

            // get a track
            var output = await SpotifyWebAPI.Track.GetTracks(tracks);
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}