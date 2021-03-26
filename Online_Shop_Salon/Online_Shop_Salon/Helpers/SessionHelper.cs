using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Online_Shop_Salon.Helpers
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key,object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session,string key){
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

     
    }
}