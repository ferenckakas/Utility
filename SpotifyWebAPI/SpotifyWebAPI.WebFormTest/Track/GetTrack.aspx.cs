using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Track
{
    public partial class GetTrack : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // get a track
            var output = await SpotifyWebAPI.Track.GetTrack("0eGsygTp906u18L0Oimnem");
            ((Literal)this.Master.FindControl("output")).Text = VarDump.Dump(output, 0);
        }
    }
}