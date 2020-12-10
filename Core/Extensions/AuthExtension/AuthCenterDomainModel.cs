namespace Core.Extensions.AuthExtension
{
    public class CodeExchangeAccessTokenResponseDomainModel
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// AccessToken有效时长（秒）
        /// </summary>
        public int ExpiresIn { get; set; }
        /// <summary>
        /// RefreshToken有效期限30分钟
        /// </summary>
        public string RefreshToken { get; set; }
    }
    public class UserSessionDomainModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long? OrganUserId { get; set; }
        public string OrganUserName { get; set; }
        public string OrganizationCode { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
    public class AuthorizationCodeResponseDomainModel
    {
        public string Code { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string SSOToken { get; set; }
        public string OrganizationCode { get; set; }
        public string UserName { get; set; }
    }
}
