using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpotifyWebAPI.Test.Authentication
{
    public partial class Default : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// starts the log in process, builds the correct query string and redirects the user to spotify to log in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void startLoginProcess_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clientIdTextBox.Text))
                return;

            if (string.IsNullOrWhiteSpace(clientSecretTextBox.Text))
                return;

            string clientId = clientIdTextBox.Text;
            string clientSecret = clientSecretTextBox.Text;
            string redirectUri = redirectUriTextBox.Text; 
            string scope = string.Empty;
            foreach(ListItem item in scopeCheckBoxList.Items)
            {
                if (item.Selected)
                    scope += item.Value + "%20";
            }
            scope = scope.Substring(0, scope.Length - 3);

            // save the values for later, these would normally be stored in your web config in this type of project
            Session["clientid"] = clientId;
            Session["clientsecret"] = clientSecret;
            Session["redirectUri"] = redirectUri;
            Session["scope"] = scope;

            // redirect to spotify to authenticate
            Response.Redirect("https://accounts.spotify.com/authorize/?" +
                "client_id=" + clientId +
                "&response_type=code" +
                "&redirect_uri=" + redirectUri +
                "&scope=" + scope);
        }
    }
}