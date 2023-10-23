using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Entities;

namespace ConsoleApp
{
    public static class Global
    {
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
            //else
            //    return (T)HttpContext.Current.Session[key];
            else
                return default(T);
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
            //else
            //    HttpContext.Current.Session[key] = value;
        }
    }
}
