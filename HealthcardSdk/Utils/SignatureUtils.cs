using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace com.tencent.healthcard.Utils
{
    class SignatureUtils
    {
        public static SortedDictionary<string, object> ToSortedDictionary(object obj)
        {
            var dictionary = new SortedDictionary<string, object>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
            {
                object value = property.GetValue(obj);
                //System.Diagnostics.Debug.WriteLine("property.Name: " + property.Name + "|value: [" + value + "]" + "|type:["+ value.GetType() + "]");
                if (null == value)
                {
                    continue;
                }
                if (isBaseType(value))
                {
                    dictionary.Add(property.Name, value);
                }
                else
                {
                    if (value.GetType().IsEnum)
                    {
                        dictionary.Add(property.Name, (Enum)value);
                    }
                    else
                    {
                        if (value is Array)
                        {
                            ArrayList sortJsonList = new ArrayList();
                            foreach (var childValue in (Array)value)
                            {
                                string childJson = JsonConvert.SerializeObject(childValue, new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });

                                sortJsonList.Add(JsonConvert.DeserializeObject<SortedDictionary<string, object>>(childJson));
                            }
                            dictionary.Add(property.Name, sortJsonList);
                        }
                    }
                }
            }

            return dictionary;
        }

        public static SortedDictionary<string, object> MixSortedDictionary(params SortedDictionary<string, object>[] objs)
        {
            var result = new SortedDictionary<string, object>();
            foreach (var dic in objs)
            {
                foreach (var pair in dic)
                {
                    if (result.ContainsKey(pair.Key))
                    {

                    }
                    Console.Out.WriteLine(pair.Key);
                    // result.Add(pair.Key, pair.Value);
                    result[pair.Key] = pair.Value;
                }
            }
            return result;
        }

        public static string ToParamStr(SortedDictionary<string, object> dictionary)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                if (null == keyValuePair.Value)
                {
                    continue;
                }
                string val = "";
                if (null == keyValuePair.Value)
                {
                    continue;
                }
                if (isBaseType(keyValuePair.Value))
                {
                    val = keyValuePair.Value.ToString();
                }
                else
                {
                    if (keyValuePair.Value.GetType().IsEnum)
                    {
                        val = (Convert.ToInt32((Enum)keyValuePair.Value)).ToString();
                    }
                    else
                    {
                        val = JsonConvert.SerializeObject(keyValuePair.Value);
                    }

                }
                if (val.Trim().Equals(""))
                {
                    continue;
                }
                builder.Append(keyValuePair.Key);
                builder.Append("=");
                builder.Append(val);
                builder.Append("&");
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private static bool isBaseType(object obj)
        {
            Type type = obj.GetType();
            return type.IsPrimitive || type.Equals(typeof(string));
        }

        public static string GenSiagnture(string appSecret, params SortedDictionary<string, object>[] args)
        {
            var dic = MixSortedDictionary(args);
            var paramStr = ToParamStr(dic);
            Console.Out.WriteLine("paramStr:" + paramStr);
            byte[] bytes = Encoding.UTF8.GetBytes(paramStr + appSecret);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            string sign = Convert.ToBase64String(hash);
            return sign;
        }
    }
}
