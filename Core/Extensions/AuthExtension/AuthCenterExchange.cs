using Core.Helpers;
using Newtonsoft.Json;
using static Core.UnifiedResponseMessage.ResponseObjectExtention;

namespace Core.Extensions.AuthExtension
{
    public static class AuthCenterExchange
    {
        /// <summary>
        /// code换取用户信息
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ResponseObject<UserSessionDomainModel> CodeExchangeUserInfo(string redirectUri, string code)
        {
            {
                //Code换取AccessToken
                var accessTokenStr = HttpHelper.PostWithJson($@"{ConfigurationManager.AppSettings["AuthCenterUrl"]}/oauth/codeExchangeAccessToken?clientId={ConfigurationManager.AppSettings["ClientId"]}&&ClientSecret={ConfigurationManager.AppSettings["ClientSecret"]}&&grantType=authorization_code&&redirectUrl={redirectUri}&&code={code}", "");
                NLogHelper.InfoLog($"{ConfigurationManager.AppSettings["AuthCenterUrl"]}_{code}/{redirectUri}/{accessTokenStr}");
                var accessData = JsonConvert.DeserializeObject<ResponseObject<CodeExchangeAccessTokenResponseDomainModel>>(accessTokenStr);
                if (accessData.Code != "0")
                {
                    return new ResponseObject<UserSessionDomainModel> { Code = accessData.Code, Msg = accessData.Msg, Data = null };
                }
                //accessToken换取用户信息
                var userInfoStr = HttpHelper.PostWithJson($"{ConfigurationManager.AppSettings["AuthCenterUrl"]}/oauth/accessTokenExchangeUserInfo?accessToken={accessData.Data.AccessToken}", "");
                var userInfoData = JsonConvert.DeserializeObject<ResponseObject<UserSessionDomainModel>>(userInfoStr);
                //判断是否有错误信息
                if (userInfoData.Code != "0")
                {
                    return new ResponseObject<UserSessionDomainModel> { Code = accessData.Code, Msg = accessData.Msg, Data = null };

                }
                var user = userInfoData.Data;
                return new ResponseObject<UserSessionDomainModel> { Code = "0", Msg = "", Data = user };
            }
        }
        /// <summary>
        /// Code换取AccessToken
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ResponseObject<string> CodeExchangeAccessToken(string redirectUri, string code)
        {
            {
                //Code换取AccessToken
                var accessTokenStr = HttpHelper.PostWithJson($@"{ConfigurationManager.AppSettings["AuthCenterUrl"]}/oauth/codeExchangeAccessToken?clientId={ConfigurationManager.AppSettings["ClientId"]}&&ClientSecret={ConfigurationManager.AppSettings["ClientSecret"]}&&grantType=authorization_code&&redirectUrl={redirectUri}&&code={code}", "");
                NLogHelper.InfoLog($"{ConfigurationManager.AppSettings["AuthCenterUrl"]}_{code}/{redirectUri}/{accessTokenStr}");
                var accessData = JsonConvert.DeserializeObject<ResponseObject<CodeExchangeAccessTokenResponseDomainModel>>(accessTokenStr);
                if (accessData.Code != "0")
                {
                    return new ResponseObject<string> { Code = accessData.Code, Msg = accessData.Msg, Data = null };
                }
                return new ResponseObject<string> { Code = "0", Msg = "", Data = accessData.Data.AccessToken };
            }
        }
        /// <summary>
        /// ACCESSToken换取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ResponseObject<UserSessionDomainModel> AccessTokenExchangeUserInfo(string accessToken)
        {
            {
                //accessToken换取用户信息
                var userInfoStr = HttpHelper.PostWithJson($"{ConfigurationManager.AppSettings["AuthCenterUrl"]}/oauth/accessTokenExchangeUserInfo?accessToken={accessToken}", "");
                if (string.IsNullOrEmpty(userInfoStr))
                {
                    return new ResponseObject<UserSessionDomainModel> { Code = "1046", Msg = "", Data = null };
                }

                var userInfoData = JsonConvert.DeserializeObject<ResponseObject<UserSessionDomainModel>>(userInfoStr);
                //判断是否有错误信息
                if (userInfoData.Code != "0")
                {
                    return new ResponseObject<UserSessionDomainModel> { Code = userInfoData.Code, Msg = userInfoData.Msg, Data = null };
                }
                var user = userInfoData.Data;
                return new ResponseObject<UserSessionDomainModel> { Code = "0", Msg = "", Data = user };
            }
        }
    }
}