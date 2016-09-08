using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace WMISKUWeb.Models
{
    public static class JSONHelper
    {
        public static string ObjectToJSON(object obj)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            js.WriteObject(ms, obj);
            ms.Position = 0;

            StreamReader sr = new StreamReader(ms);

            return sr.ReadToEnd();
        }

        public static object JSONToObject(object obj, string json)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json));
            ms.Position = 0;

            var p2 = Convert.ChangeType(js.ReadObject(ms), obj.GetType());

            return p2;
        }
    }
}
