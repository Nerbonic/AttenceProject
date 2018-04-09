using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AttenceProject.App_Start
{
    public static class JsonTool
    {
        public static string LI2J(IList list)
        {
            string result = "";
            result = JsonConvert.SerializeObject(list);
            return result;

        }
        public static string EN2J(object obj)
        {
            string result = "";
            result = JsonConvert.SerializeObject(obj);
            return result;
        }

        internal static string LI2J(object p)
        {
            throw new NotImplementedException();
        }
    }
    
}