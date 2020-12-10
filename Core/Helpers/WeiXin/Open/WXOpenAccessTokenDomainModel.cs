namespace Core.Helpers
{
    public class WXOpenAccessTokenDomainModel
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string Access_Token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public string Expires_In { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string Refresh_Token { get; set; }
        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 当且仅当该网站应用已获得该用户的userinfo授权时，才会出现该字段。
        /// </summary>
        public string Unionid { get; set; }
    }
}
