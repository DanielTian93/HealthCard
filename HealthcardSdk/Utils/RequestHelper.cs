using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace com.tencent.healthcard.Utils
{
    class RequestHelper
    {

        public static Dictionary<string, object> call(string appSecret, string method, string appToken, string url, Dictionary<string, object> args)
        {
            var _params = new Dictionary<string, SortedDictionary<string, object>>();
            SortedDictionary<string, object> commonIn = null;
            foreach (var item in args)
            {
                var val = SignatureUtils.ToSortedDictionary(item.Value);
                if (item.Key.Equals("commonIn"))
                {
                    val.Add(
                    "timestamp",
                    ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString()
                    );
                    //val["appToken"] = appToken;
                    commonIn = val;
                }
                _params.Add(item.Key, val);
            }


            var sign = SignatureUtils.GenSiagnture(appSecret, _params.Values.ToArray());
            Console.Out.WriteLine("sign:" + sign);
            var content = "";

            if (commonIn != null)
            {
                commonIn.Add("sign", sign);
            }

            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                var dataString = JsonConvert.SerializeObject(_params);
                Console.Out.WriteLine("dataString:" + dataString);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                content = client.UploadString(new Uri(url), "POST", dataString);
            }
            Console.Out.WriteLine("content:" + content);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
        }
    }
}
