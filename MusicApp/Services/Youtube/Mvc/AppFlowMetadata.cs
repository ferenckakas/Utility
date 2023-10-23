using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.YouTube.v3;
using Services.Old;
//using System.Web.Mvc;

namespace Services.Youtube.Mvc
{
    public class AppFlowMetadata //: FlowMetadata
    {
        private static readonly IAuthorizationCodeFlow _flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "512750602887-gioq4d6o9d794si28gcigrg03lft0nhf.apps.googleusercontent.com",
                    ClientSecret = "ridkrLb-y1ib2GGD9MCqFCg3"
                },
                Scopes = new[] { YouTubeService.Scope.Youtube },
                DataStore = new DBDataStore("YouTube.Api.Auth.Store-")
            });

        //public override string GetUserId(Controller controller)
        //{
        //    // In this sample we use the session to store the user identifiers.
        //    // That's not the best practice, because you should have a logic to identify
        //    // a user. You might want to use "OpenID Connect".
        //    // You can read more about the protocol in the following link:
        //    // https://developers.google.com/accounts/docs/OAuth2Login.

        //    if (MvcApplication.DBSession())
        //    {
        //        using (MusicEntities entities = new MusicEntities())
        //        {
        //            string userValue = entities.Session("user");
        //            if (userValue == null)
        //            {
        //                MvcApplication.Trace("YouTube Auth", "AppFlowMetadata.GetUserId: if (userValue == null)");

        //                Guid user = Guid.NewGuid();
        //                userValue = user.ToString();
        //                entities.Session("user", userValue);
        //            }

        //            MvcApplication.Trace("YouTube Auth", "AppFlowMetadata.GetUserId: userValue=" + userValue);

        //            return userValue;
        //            //string userValue = entities.Session("user");
        //            //if (userValue == null)
        //            //{
        //            //    Guid user = Guid.NewGuid();
        //            //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        //            //    userValue = jsSerializer.Serialize(user);
        //            //    entities.Session("user", userValue);
        //            //    return user.ToString();
        //            //}
        //            //else
        //            //{
        //            //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        //            //    var user = jsSerializer.Deserialize<Guid>(userValue);
        //            //    return user.ToString();
        //            //}
        //        }
        //    }
        //    else
        //    {
        //        Guid user = controller.Session["user"];
        //        if (user == null)
        //        {
        //            user = Guid.NewGuid();
        //            controller.Session["user"] = user;
        //        }
        //        return user.ToString();
        //    }
        //}

        public IAuthorizationCodeFlow Flow //override
        {
            get {
                Application.Trace("YouTube Auth", "AuthCallbackController.Flow: Get");
                return _flow;
            }
        }
    }
}