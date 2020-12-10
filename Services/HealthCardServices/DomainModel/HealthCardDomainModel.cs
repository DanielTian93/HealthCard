using com.tencent.healthcard.HealthCardOpenAPI;
using Core.Models.DBModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HealthCardServices.DomainModel
{
    public class RegisterHealthCardRequestDomainModel
    {
        public RegisterHealthCardReq RegisterHealthCardReq { get; set; }

        /// <summary>
        /// 是否小孩
        /// </summary>
        public string IsChild { get; set; } = "0";
        /// <summary>
        /// 监护人身份证
        /// </summary>
        public string GuaranteeIdCard { get; set; }
        /// <summary>
        /// 监护人姓名
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 与监护人的关系①、配偶：1；②、父母：2；③、子女：3④、祖父母：4⑤、孙子女：5⑥、兄弟姐妹：6⑦、叔叔阿姨：7⑧、侄子侄女：8⑨、同事同学：9⑩、其他：0

        /// </summary>
        public string GuaranteeType { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 微信Code
        /// </summary>
        [JsonIgnore]
        public string Code { get; set; }
    }

    public class IDCardOCRRequestDomainModel
    {
        /// <summary>
        /// 身份证Base64
        /// </summary>
        public string ImageContent { get; set; }
    }
    public class MedBindInputViewModel
    {
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string MedName { get; set; }

        private string medCardid = "";

        /// <summary>
        /// 身份证号
        /// </summary>
        public string MedCardid
        {
            get
            {
                return this.medCardid.ToUpper();
            }
            set
            {
                this.medCardid = value.ToUpper();
            }
        }

        /// <summary>
        /// 身份证地址
        /// </summary>
        public string MedAddress { get; set; }

        /// <summary>
        /// 关系人手机
        /// </summary>
        public string MedLinkMobile { get; set; }

        public string OpenId { get; set; }

        /// <summary>
        /// 病历号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 是否小孩
        /// </summary>
        public string IsChild { get; set; }

        /// <summary>
        /// 小孩生日
        /// </summary>

        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 监护人姓名
        /// </summary>
        public string ParentName { get; set; }
    }

    public class HealthCardQueryRequestDomainModel
    {
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }
        public int? Status { get; set; }
    }

    public class HealthCardQueryResponseDomainModel : RegisterHealthCardInfo
    {
        public string Name { get; set; }
    }
    public class ReportHISDataReuestDomainModel
    {

        /// <summary>
        /// 服务号用户Id
        /// </summary>
        public int MemId { get; set; }
        ///// <summary>
        ///// 时间
        ///// </summary>
        //public string Time { get; set; }
        /// <summary>
        /// 用卡环节
        /// </summary>
        public string Scene { get; set; }
        /// <summary>
        /// 用卡科室
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 用卡渠道
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 用卡费别
        /// </summary>
        public string CardCostTypes { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string From { get; set; }
    }
    public class GetHealthCardByHealthCodeRequestDomainModel
    {
        /// <summary>
        /// 跨院授权获取的Healthcode
        /// </summary>
        public string HealthCode { get; set; }
        /// <summary>
        /// 如果有Openid则不填
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 微信Openid
        /// </summary>
        public string OpenId { get; set; }
    }

    public class AllCardQueryRequestDomainModel
    {
        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
    }
    public class AllCardQueryResponseDomainModel : ChatMember
    {
        /// <summary>
        /// 健康卡信息
        /// </summary>
        public RegisterHealthCardInfo RegisterHealthCardInfo { get; set; }
    }
    public class OldCardRegisterHealthCardRequestDomainModel
    {
        /// <summary>
        /// 微信身份码
        /// </summary>
        public string WechatCode { get; set; }
        /// <summary>
        /// 用户openId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信公众号程序内用户id
        /// </summary>
        public int Memid { get; set; }
    }
    public class RegisterHealthCardErrorLogQueryRequestDomainModel
    {
        public string Data { get; set; }
        public string Info { get; set; }
    }

    public class HealthCardInfoQueryRequestDomainModel
    {
        /// <summary>
        /// 成员姓名
        /// </summary>
        public string MemName { get; set; }
        /// <summary>
        /// 成员身份证
        /// </summary>
        public string MemCard { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string MemCheckCard { get; set; }
    }

    public class HealthCardInfoQueryResponse
    {
        /// <summary>
        /// 健康卡ID
        /// </summary>
        public string HealthCardId { get; set; }
        /// <summary>
        /// 二维码文本
        /// </summary>
        public string QrcodeText { get; set; }
        /// <summary>
        /// 健康卡状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 微信Openid
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string MemName { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string MemCard { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string MemCheckCard { get; set; }
        /// <summary>
        /// 就诊卡使用状态
        /// </summary>
        public int IsUse { get; set; }
        /// <summary>
        /// 健康卡申领时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
    public class LoginRequestDomainModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
    public class LoginResponseDomainModel
    {

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }

}
