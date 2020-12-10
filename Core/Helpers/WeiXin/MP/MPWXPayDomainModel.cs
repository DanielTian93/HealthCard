namespace Core.Helpers
{
    public class MPWXPayDomainModel
    {
        /// <summary>
        /// appid
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        public string Package { get; set; }
        /// <summary>
        /// 签名字符串
        /// </summary>
        public string PaySign { get; set; }
        /// <summary>
        /// 签名加密类型
        /// </summary>
        public string SignType { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public string BillId { get; set; }
    }
    public class MPWXPayResponseDomainModel
    {
        public MPWXPayDomainModel MPWXPayDomainModel { get; set; }
        public int Code { get; set; } = 0;
    }

}
