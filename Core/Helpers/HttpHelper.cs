using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class HttpHelper
    {

        public static string Get(string url)
        {
            try
            {
                var response = (new HttpClient()).GetAsync(new Uri(url)).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                //LogHelper.WriteError("HttpHelper发起Get请求时发生异常", ex, new { Url = url });
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetStream(string url)
        {
            var response = (new HttpClient()).GetAsync(new Uri(url)).Result;
            return response.Content.ReadAsStreamAsync().Result;
        }



        public static string PostWithJson(string url, string data = "")
        {
            try
            {
                HttpContent content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return new HttpClient().PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteError("HttpHelper发起PostWithJson请求时发生异常", ex, new { Url = url, Data = data });
                return null;
            }
        }

        public static async Task<string> GetAsync(string url, List<Headers> headers = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.HeadersName, item.HeadersValue);
                        }
                    }
                    var response = await client.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();
                    return json;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<T> GetAsync<T>(string url, List<Headers> headers = null) where T : class
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.HeadersName, item.HeadersValue);
                        }
                    }
                    var response = await client.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<K> PostWithJsonAsync<T, K>(string url, T data, List<Headers> headers = null, string SSLCERT_PATH = "", string SSLCERT_PASSWORD = "")
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                if (!string.IsNullOrEmpty(SSLCERT_PATH))
                {
                    X509Certificate2 certificate = new X509Certificate2(SSLCERT_PATH, SSLCERT_PASSWORD);
                    handler.ClientCertificates.Add(certificate);
                }
                using (var client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.HeadersName, item.HeadersValue);
                        }
                    }
                    var response = await client.PostAsync(url, content);
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<K>(json);
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteError("HttpHelper发起PostWithJson请求时发生异常", ex, new { Url = url, Data = data });
                throw ex;
            }
        }
        public static async Task<string> PostWithJsonAsync<T>(string url, T data, List<Headers> headers = null, string SSLCERT_PATH = "", string SSLCERT_PASSWORD = "")
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                if (!string.IsNullOrEmpty(SSLCERT_PATH))
                {
                    X509Certificate2 certificate = new X509Certificate2(SSLCERT_PATH, SSLCERT_PASSWORD);
                    handler.ClientCertificates.Add(certificate);
                }
                using (var client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.HeadersName, item.HeadersValue);
                        }
                    }
                    var response = await client.PostAsync(url, content);
                    var json = await response.Content.ReadAsStringAsync();
                    return json;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteError("HttpHelper发起PostWithJson请求时发生异常", ex, new { Url = url, Data = data });
                throw ex;
            }
        }

        public class Headers
        {
            /// <summary>
            /// 添加头的名字
            /// </summary>
            public string HeadersName { get; set; }
            /// <summary>
            /// 添加头的值
            /// </summary>
            public string HeadersValue { get; set; }
        }

        public static async Task<string> PostWithXmlAsync<T, K>(string url, T data, string SSLCERT_PATH = "", string SSLCERT_PASSWORD = "")
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                if (!string.IsNullOrEmpty(SSLCERT_PATH))
                {
                    //X509Certificate2 certificate = new X509Certificate2(SSLCERT_PATH, SSLCERT_PASSWORD);
                    X509Certificate2 certificate = new X509Certificate2(SSLCERT_PATH.ToString(), SSLCERT_PASSWORD.ToString(), X509KeyStorageFlags.MachineKeySet);
                    handler.ClientCertificates.Add(certificate);
                }
                using (var client = new HttpClient(handler))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/xml");
                    var response = await client.PostAsync(url, content);
                    var res = await response.Content.ReadAsStringAsync();
                    //NLogHelper.InfoLog(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.ErrorLog("HttpHelper发起PostWithJson请求时发生异常", ex);
                throw ex;
            }
        }

        public static async Task<string> PostWithFormAsync(string url, Dictionary<string, string> data, List<Headers> headers = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(data);
                    //var jsonString = JsonConvert.SerializeObject(data);
                    //var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.HeadersName, item.HeadersValue);
                        }
                    }
                    var response = await client.PostAsync(url, content);
                    var json = await response.Content.ReadAsStringAsync();
                    return json;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        public class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
        public static async Task<string> PostWithFormAsync(string url, List<KeyValue> formData, List<Headers> headers = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var items = formData.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));
                    var content = new StringContent(string.Join("&", items), null, "application/x-www-form-urlencoded");
                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.HeadersName, item.HeadersValue);
                        }
                    }
                    var response = await client.PostAsync(url, content);
                    var json = await response.Content.ReadAsStringAsync();
                    return json;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }




        public static string PostWithText(string url, string data)
        {
            try
            {
                HttpContent content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return new HttpClient().PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                //LogHelper.WriteError("HttpHelper发起PostWithText请求时发生异常", ex, new { Url = url, Data = data });
                return null;
            }
        }

        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null)
                {
                    reader.Dispose();
                }

                if (stream != null)
                {
                    stream.Dispose();
                }

                if (rsp != null)
                {
                    rsp.Dispose();
                }
            }
        }
    }
}
