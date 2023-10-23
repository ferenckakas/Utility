using Entities;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using Services.Old;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Services.Youtube.Mvc
{
    public class DBDataStore : IDataStore
    {
        private string _prefix;
        //
        // Summary:
        //     Constructs a new db data store.
        //
        // Parameters:
        //   prefix:
        //     prefix.
        public DBDataStore(string prefix)
        {
            Application.Trace("YouTube Auth", "DBDataStore.DBDataStore");
            _prefix = prefix;
        }

        //
        // Summary:
        //     Asynchronously clears all values in the data store.
        public async Task ClearAsync()
        {
            Application.Trace("YouTube Auth", "DBDataStore.ClearAsync");
            using (var entities = new MusicEntities())
            {
                await entities.Sessions.Where(s => s.Key.StartsWith(_prefix)).ForEachAsync<Session>(s => entities.Sessions.Remove(s));
                await entities.SaveChangesAsync();
            }
        }
        //
        // Summary:
        //     Asynchronously deletes the given key. The type is provided here as well because
        //     the "real" saved key should contain type information as well, so the data store
        //     will be able to store the same key for different types.
        //
        // Parameters:
        //   key:
        //     The key to delete.
        //
        // Type parameters:
        //   T:
        //     The type to delete from the data store.
        public async Task DeleteAsync<T>(string key)
        {
            using (var entities = new MusicEntities())
            {
                string type = typeof(T).ToString();
                key = type + "-" + key;
                Application.Trace("YouTube Auth", $"DBDataStore.DeleteAsync: {key}");
                Session session = await entities.Sessions.Where(s => s.Key == _prefix + key).FirstOrDefaultAsync();
                if (session != null)
                {
                    entities.Sessions.Remove(session);
                    await entities.SaveChangesAsync();
                }
            }
        }
        //
        // Summary:
        //     Asynchronously returns the stored value for the given key or null if not found.
        //
        // Parameters:
        //   key:
        //     The key to retrieve its value.
        //
        // Type parameters:
        //   T:
        //     The type to retrieve from the data store.
        //
        // Returns:
        //     The stored object.
        public async Task<T> GetAsync<T>(string key)
        {
            using (MusicEntities entities = new MusicEntities())
            {
                string type = typeof(T).ToString();
                key = type + "-" + key;
                Session session = await entities.Sessions.Where(s => s.Key == _prefix + key).FirstOrDefaultAsync();
                if (session != null)
                {
                    var jsSerializer = new JavaScriptSerializer();
                    T result = jsSerializer.Deserialize<T>(session.Value);
                    Application.Trace("YouTube Auth", $"DBDataStore.GetAsync: {key}, out:{result}");

                    if (true) //HttpContext.Current.Request != null && HttpContext.Current.Request.IsLocal
                    {
                        if (result is TokenResponse)
                        {
                            TokenResponse res = result as TokenResponse;
                            res.Issued = res.Issued.AddHours(2);
                            result = (T)Convert.ChangeType(res, typeof(T));
                        }
                    }
                    return result;
                }
                else
                {
                    Application.Trace("YouTube Auth", $"DBDataStore.GetAsync: {key}, out: null");
                    return default(T);
                }
            }
        }
        //
        // Summary:
        //     Asynchronously stores the given value for the given key (replacing any existing
        //     value).
        //
        // Parameters:
        //   key:
        //     The key.
        //
        //   value:
        //     The value to store.
        //
        // Type parameters:
        //   T:
        //     The type to store in the data store.
        public async Task StoreAsync<T>(string key, T value)
        {
            using (var entities = new MusicEntities())
            {
                var jsSerializer = new JavaScriptSerializer();
                string svalue = jsSerializer.Serialize(value);

                string type = typeof(T).ToString();
                key = type + "-" + key;
                Application.Trace("YouTube Auth", $"DBDataStore.StoreAsync: {key} value:{svalue}");
                Session session = await entities.Sessions.Where(s => s.Key == _prefix + key).FirstOrDefaultAsync();
                if (session == null)
                {
                    session = new Session();
                    session.Key = _prefix + key;
                    entities.Sessions.Add(session);
                }
                session.Value = svalue;
                await entities.SaveChangesAsync();
            }
        }
    }
}