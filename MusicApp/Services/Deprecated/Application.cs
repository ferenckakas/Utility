using Entities;
using SpotifyWebAPI;
using System;
using System.Web;
using System.Web.Script.Serialization;

namespace Services.Old
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Application
    {
        public static string CorePath;
        public static string LogPath;

        public static bool DBSession()
        {
            //return ConfigurationManager.AppSettings["DBSession"].ToLower() == "true";
            return true;
        }

        public static T Session<T>(string key)
        {
            if (DBSession())
            {
                using (MusicEntities entities = new MusicEntities())
                {
                    string value = entities.Session(key);
                    if (value != null)
                    {
                        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                        T result = jsSerializer.Deserialize<T>(value);
                        return result;
                    }
                    else
                        return default(T);
                }
            }
            else
                return (T)HttpContext.Current.Session[key];
        }

        public static void Session<T>(string key, T value)
        {
            if (DBSession())
            {
                using (MusicEntities entities = new MusicEntities())
                {
                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    entities.Session(key, jsSerializer.Serialize(value));
                }
            }
            else
                HttpContext.Current.Session[key] = value;
        }

        public static void TraceClear(string key)
        {
            if (DBSession())
            {
                using (MusicEntities entities = new MusicEntities())
                {
                    entities.TraceClear(key);
                }
            }
            else
                HttpContext.Current.Session[key] = "";
        }
        
        public static void Trace(string key, string value)
        {
            if (DBSession())
            {
                using (MusicEntities entities = new MusicEntities())
                {
                    entities.Trace(key, value);
                }
            }
            else
                HttpContext.Current.Session[key] += value;
        }

        public static string CurrentController
        {
            get
            {
                return Session<string>("CurrentController");
            }

            set
            {
                Session("CurrentController", value);
            }
        }

        public static string CurrentAction
        {
            get
            {
                return Session<string>("CurrentAction");
            }

            set
            {
                Session("CurrentAction", value);
            }
        }

        public static AuthenticationToken AuthenticationToken
        {
            get
            {
                return new AuthenticationToken()
                {
                    AccessToken = Session<string>("AccessToken"),
                    ExpiresOn = Session<DateTime>("ExpiresOn"),
                    RefreshToken = Session<string>("RefreshToken"),
                    TokenType = Session<string>("TokenType"),
                    //Now = Session<DateTime>("Now"),
                };
            }
            set
            {
                Session("AccessToken", value.AccessToken);
                Session("ExpiresOn", value.ExpiresOn);
                Session("RefreshToken", value.RefreshToken);
                Session("TokenType", value.TokenType);
                //Session("Now", DateTime.Now);

                //HttpContext.Current.Session["AccessToken"] = value.AccessToken;
                //HttpContext.Current.Session["ExpiresOn"] = value.ExpiresOn;
                //HttpContext.Current.Session["RefreshToken"] = value.RefreshToken;
                //HttpContext.Current.Session["TokenType"] = value.TokenType;
            }
        }

        //private MySessionState<string> _session2;

        //public class MySessionState<T>
        //{
        //    public T this[string key]
        //    {
        //        get
        //        {
        //            if (DBSession())
        //            {
        //                using (Entities entities = new Entities())
        //                {
        //                    string value = entities.Session(key);
        //                    if (value != null)
        //                    {
        //                        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        //                        return jsSerializer.Deserialize<T>(value);
        //                    }
        //                    else
        //                        return default(T);
        //                }
        //            }
        //            else
        //                return (T)HttpContext.Current.Session[key];
        //        }

        //        set
        //        {
        //            if (DBSession())
        //            {
        //                using (Entities entities = new Entities())
        //                {
        //                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        //                    entities.Session(key, jsSerializer.Serialize(value));
        //                }
        //            }
        //            else
        //                HttpContext.Current.Session[key] = value;
        //        }
        //    }
        //}
    }
}