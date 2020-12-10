
namespace com.tencent.healthcard.HealthCardOpenAPI
{
    public class RegisterHealthCardReq
    {
        /// <summary>
        /// 微信身份码
        /// </summary>
        public string wechatCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 性别 男、女
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 民族 	汉族、满族等
        /// </summary>
        public string nation { get; set; }
        /// <summary>
        /// 出生年月日 格式：yyyy-MM-dd
        /// </summary>
        public string birthday { get; set; }
        ///// <summary>
        ///// 证件号码
        ///// </summary>
        //public string idCard { get; set; }
        ///// <summary>
        ///// 证件类型
        ///// </summary>
        //public string cardType { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 联系方式1
        /// </summary>
        public string phone1 { get; set; }
        /// <summary>
        /// 联系方式2
        /// </summary>
        public string phone2 { get; set; }
        /// <summary>
        /// 院内ID
        /// </summary>
        public string patid { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string idNumber { get; set; }
        /// <summary>
        /// 证件类型 01-居民身份证，其他参考证件类型表，核身失败的用户不会显示在开放平台官网上面。
        /// </summary>
        public string idType { get; set; }
        /// <summary>
        /// 用户openId
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 0单个注册 1 批量注册
        /// </summary>
        public RegisteredType registeredType { get; set; }
    }
}
