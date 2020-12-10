using System.ComponentModel;

namespace Core.Enum
{
    public enum ErrCodeCommon : long
    {
        /// <summary>
        /// 帐号或密码错误
        /// </summary>
        [Description("帐号或密码错误")]
        LoginError = 1002,
        /// <summary>
        /// 添加失败
        /// </summary>
        [Description("添加失败")]
        InsertError = 1003,
        [Description("身份证为空，请输入身份证")]
        IdCardNullError = 1031,
        [Description("绑定失败")]
        BindWxErr = 1012,

        [Description("注册健康卡失败,请检查信息是否正确")]
        registerHealthCardError = 1200,
        [Description("OpenId或Code不能为空")]
        OpenIdOrCodeIsNullError = 1201,
        [Description("此电话建档数已超限 请更换手机号")]
        PhoneUsedError = 1202,
        [Description("就诊卡号为空")]
        CardIsNull = 1203,
        [Description("HIS注册失败、无就诊卡号请联系客服")]
        HisError = 1204,
        [Description("最多只能绑定5个成员")]
        FllowUpSubIdError = 1205,
        [Description("用户不存在")]
        PatientInfoError = 1205,
    }
    public class ErrDetail
    {
        public string Code { get; set; }

        public string Msg { get; set; }
    }
}
