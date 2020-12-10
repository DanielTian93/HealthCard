
namespace com.tencent.healthcard.HealthCardOpenAPI
{
    public class ReportHISDataReq
    {
        /// <summary>
        /// 二维码
        /// </summary>
        public string qrCodeText { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string idCardNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 医院ID 文档无此参数 调用时默认设置
        /// </summary>
        public string hospitalId { get; set; }
        /// <summary>
        /// 用卡环节
        /// </summary>
        public string scene { get; set; }
        /// <summary>
        /// 用卡科室
        /// </summary>
        public string cardType { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string cardChannel { get; set; }
        /// <summary>
        /// 用卡渠道
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 用卡费别
        /// </summary>
        public string cardCostTypes { get; set; }
        /// <summary>
        /// 医院ID 调用时默认设置
        /// </summary>
        public string hospitalCode { get; set; }
    }
}
