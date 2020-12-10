using Core.Extensions;
using Core.Helpers.WeiXin.MP;
using Core.Helpers.WeiXin.woa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class WeiXinHelper
    {
        #region 微信开放平台
        /// <summary>
        /// 微信开放平台根据code 获取微信用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WXOpenUserinfoDomainModel WxOpenInfo(string code, string appId, string secret)
        {
            var accessinfo = GetOpenAccessToken(code, appId, secret);
            if (accessinfo.Access_Token == null)
            {
                return null;
            }
            string userinfo_url = $@"https://api.weixin.qq.com/sns/userinfo?access_token={accessinfo.Access_Token}&openid={accessinfo.Openid}";
            string data = HttpHelper.Get(userinfo_url);
            return JsonConvert.DeserializeObject<WXOpenUserinfoDomainModel>(data);
        }
        /// <summary>
        /// 微信开放平台通过code获取access
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WXOpenAccessTokenDomainModel GetOpenAccessToken(string code, string appId = "", string secret = "")
        {
            //var appId = ConfigurationManager.AppSettings["OpenAppId"];
            //var secret = ConfigurationManager.AppSettings["OpenSecret"];

            if (string.IsNullOrEmpty(appId))
            {
                appId = ConfigExtensions.Configuration["WX:WeixinAppId"];
            }
            if (string.IsNullOrEmpty(secret))
            {
                secret = ConfigExtensions.Configuration["WX:WeixinAppSecret"];
            }

            var url = $@"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appId}&secret={secret}&code={code}&grant_type=authorization_code";
            var data = HttpHelper.Get(url);
            var res = JsonConvert.DeserializeObject<WXOpenAccessTokenDomainModel>(data);
            return res;
        }
        #endregion

        #region 微信小程序
        /// <summary>
        /// 微信小程序根据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WXMPCodeToSessionDomainModel WXMPCodeToSession(string code, string appId, string secret)
        {
            //var appId = ConfigurationManager.AppSettings["MPAppId"];
            //var secret = ConfigurationManager.AppSettings["MPSecret"];
            var url = $@"https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={secret}&js_code={code}&grant_type=authorization_code";
            var data = HttpHelper.Get(url);
            var res = JsonConvert.DeserializeObject<WXMPCodeToSessionDomainModel>(data);
            if (res.Errcode != 0)
            {
                return null;
            }
            return res;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedDataStr"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AES_decrypt(string encryptedDataStr, string key, string iv)
        {
            try
            {
                RijndaelManaged rijalg = new RijndaelManaged();
                //-----------------    
                //设置 cipher 格式 AES-128-CBC    

                rijalg.KeySize = 128;
                rijalg.Padding = PaddingMode.PKCS7;
                rijalg.Mode = CipherMode.CBC;
                rijalg.Key = Convert.FromBase64String(key);
                rijalg.IV = Convert.FromBase64String(iv);

                byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
                //解密    
                ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

                string result;

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {

                return AES_decrypt(encryptedDataStr, key, iv);
            }

        }
        /// <summary>
        /// 小程序获取AccessToken 缓存1.5小时
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string GetMPAccessToken(string appid, string appSecret)
        {
            if (RedisHelper.Exists($"mpAccessToken:{appid}"))
            {
                return RedisHelper.Get<string>($"mpAccessToken:{appid}");
            }

            var json = HttpHelper.Get($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={appSecret}");
            var res = JsonConvert.DeserializeObject<MPAccessTokenDomainModel>(json);
            RedisHelper.Set($"mpAccessToken:{appid}", res.Access_Token, TimeSpan.FromHours(1.5));
            return res.Access_Token;
        }
        #region 二维码
        /// <summary>
        /// 二维码base64
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="page"></param>
        /// <param name="width"></param>
        /// <param name="auto_color"></param>
        /// <param name="line_color"></param>
        /// <param name="is_hyaline"></param>
        /// <param name="appid"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string MPQrCode(string scene, string page, string appid, string appSecret, int width = 430, bool auto_color = false, bool is_hyaline = false)
        {
            var accessToken = GetMPAccessToken(appid, appSecret);//获取接口AccessToken
            var url = string.Format("https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={0}", accessToken);
            var postData = new
            {
                scene,
                page,
                width,
                auto_color,
                //line_color,
                is_hyaline,
            };
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData));
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            Stream stream;
            stream = response.GetResponseStream();

            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }
            byte[] result = bytes.ToArray();
            ////在文件名前面加上时间，以防重名
            //string imgName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
            ////文件存储相对于当前应用目录的虚拟目录
            //string path = "/image/";
            ////获取相对于应用的基目录,创建目录
            //string imgPath = System.AppDomain.CurrentDomain.BaseDirectory + path;     //通过此对象获取文件名
            //File.WriteAllBytes(Path.Combine(path + imgName), result);//讲byte[]存储为图片
            return Convert.ToBase64String(result); ;
        }
        #endregion

        #region 信息推送
        /// <summary>
        /// 小程序信息推送
        /// </summary>
        /// <param name="req"></param>
        public static void TemplateMessageSend(TemplateMessageSendReuestDomainModel req)
        {
            var url = $" https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={req.access_token}";
            var res = HttpHelper.PostWithJsonAsync<TemplateMessageSendReuestDomainModel, TemplateMessageSendResponseDomainModel>(url, req);
            NLogHelper.InfoLog($"小程序推送信息：{JsonConvert.SerializeObject(req)}");
        }
        #endregion

        #endregion

        #region 公众号
        /// <summary>
        ///  公众号获取AccessToken 缓存1.5小时
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string GetWOAAccessToken(string appid, string appSecret)
        {
            if (RedisHelper.Exists($"WOAAccessToken:{appid}"))
            {
                return RedisHelper.Get<string>($"WOAAccessToken:{appid}");
            }

            var json = HttpHelper.Get($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={appSecret}");
            var res = JsonConvert.DeserializeObject<MPAccessTokenDomainModel>(json);
            if (res.Access_Token != null)
            {
                RedisHelper.Set($"WOAAccessToken:{appid}", res.Access_Token, TimeSpan.FromHours(1.5));
            }

            return res.Access_Token;
        }
        /// <summary>
        /// 获取订阅用户的openid
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="nextOpenid"></param>
        /// <returns></returns>
        public static List<string> GetWOAUserOpenIdList(string accessToken, string nextOpenid = "")
        {
            var list = new List<string>();
            if (string.IsNullOrEmpty(nextOpenid))
            {
                var json = HttpHelper.Get($"https://api.weixin.qq.com/cgi-bin/user/get?access_token={accessToken}");
                var resList = JsonConvert.DeserializeObject<WOAUserOpenidDoaminModel>(json);
                list.AddRange(resList.data.openid);
                if (list.Count == resList.count)
                {
                    return list;
                }

                if (!string.IsNullOrEmpty(resList.next_openid))
                {
                    list.AddRange(GetWOAUserOpenIdList(accessToken, resList.next_openid));
                }
            }
            else
            {
                var json = HttpHelper.Get($"https://api.weixin.qq.com/cgi-bin/user/get?access_token={accessToken}&next_openid={nextOpenid}");
                var resList = JsonConvert.DeserializeObject<WOAUserOpenidDoaminModel>(json);
                list.AddRange(resList.data.openid);
                if (list.Count == resList.count)
                {
                    return list;
                }

                if (!string.IsNullOrEmpty(resList.next_openid))
                {
                    list.AddRange(GetWOAUserOpenIdList(accessToken, resList.next_openid));
                }
            }
            return list;
        }
        /// <summary>
        /// 获取用户的详细信息
        /// </summary>
        /// <returns></returns>
        public static async Task<List<WOAUserInfoDoaminModel>> GetWOAUserInfoListAsync(string accessToken, List<string> openidList)
        {
            var requestList = new WOAUserListRequest();
            requestList.user_list = new List<WOAUserListItemRequest>();
            foreach (var item in openidList)
            {
                requestList.user_list.Add(new WOAUserListItemRequest
                {
                    openid = item,
                    lang = "zh_CN"
                });
            }
            var res = await HttpHelper.PostWithJsonAsync<WOAUserListRequest, WOAUserInfoListResponsDoaminModel>($"https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token={accessToken}", requestList);
            return res.user_info_list;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="temId"></param>
        /// <param name="data"> "data":{"first": {"value":"您收到新的通知","color":"#173177"},"keyword1":{"value":"采购部","color":"#173177"},"remark":{"value":"期待您的参与！","color":"#173177"}}</param>
        /// <param name="openId"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public static void SendTemplate(string accessToken, string temId, object data, List<string> openId, Miniprogram mp = null, string url = null)
        {
            if (openId.Any())
            {
                foreach (var item in openId)
                {
                    var sendData = new TemplateRequestDomainModel
                    {
                        touser = item,
                        miniprogram = mp,
                        template_id = temId,
                        data = data,
                        url = url
                    };
                    var jsonSetting = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    };
                    var res = HttpHelper.PostWithJson($"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={accessToken}", JsonConvert.SerializeObject(sendData, jsonSetting));
                    //Console.WriteLine(JsonConvert.SerializeObject(res));
                }
            }
            #endregion

        }
    }
}
