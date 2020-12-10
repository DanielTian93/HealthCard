namespace Core.Helpers
{
    public class WXOpenUserinfoDomainModel
    {
        /// <summary>
        /// 普通用户的标识，对当前开发者帐号唯一
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 普通用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 普通用户性别，1为男性，2为女性
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string Headimgurl { get; set; }
        /// <summary>
        /// 用户特权信息，json数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public string[] Privilege { get; set; }
        /// <summary>
        /// 用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
        /// </summary>
        public string Unionid { get; set; }
    }
}
