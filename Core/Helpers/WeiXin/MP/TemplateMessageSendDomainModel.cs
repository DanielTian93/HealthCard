namespace Core.Helpers.WeiXin.MP
{
    public class TemplateMessageSendReuestDomainModel
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 接收者（用户）的 openid
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 所需下发的模板消息的id
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
        /// </summary>
        public string page { get; set; }
        /// <summary>
        /// 表单提交场景下，为 submit 事件带上的 formId；支付场景下，为本次支付的 prepay_id
        /// </summary>
        public string form_id { get; set; }
        /// <summary>
        /// 模板内容，不填则下发空模板。具体格式请参考示例。
        /// </summary>
        public TemplateData data { get; set; }
        /// <summary>
        /// 模板需要放大的关键词，不填则默认无放大
        /// </summary>
        public string emphasis_keyword { get; set; }
    }
    public class TemplateMessageSendResponseDomainModel
    {
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }
    public class TemplateData
    {
        public TemplateValue keyword1 { get; set; }
        public TemplateValue keyword2 { get; set; }
        public TemplateValue keyword3 { get; set; }
        public TemplateValue keyword4 { get; set; }
        public TemplateValue keyword5 { get; set; }
        public TemplateValue keyword6 { get; set; }
        public TemplateValue keyword7 { get; set; }
        public TemplateValue keyword8 { get; set; }
        public TemplateValue keyword9 { get; set; }
        public TemplateValue keyword10 { get; set; }
    }
    public class TemplateValue
    {
        public string value { get; set; }
    }
}
