using System.Collections.Generic;

namespace Core.Helpers.WeiXin.woa
{




    public class OpenDoorTemplateRequestDomainModel
    {
        public string PersonId { get; set; }
        public string DeviceNo { get; set; }
        public Miniprogram Miniprogram { get; set; } = null;
    }




    public class OpenIdList
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> openid { get; set; }
    }
    public class WOAUserListItemRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lang { get; set; }
    }

    public class WOAUserListRequest
    {
        public List<WOAUserListItemRequest> user_list { get; set; }
    }
    public class WOAUserOpenidDoaminModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OpenIdList data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string next_openid { get; set; }
    }
    public class WOAUserInfoListResponsDoaminModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<WOAUserInfoDoaminModel> user_info_list { get; set; }
    }
    public class WOAUserInfoDoaminModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Subscribe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 深圳
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 广东
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 中国
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Headimgurl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Subscribe_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Groupid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Tagid_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Subscribe_scene { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Qr_scene { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Qr_scene_str { get; set; }
        public string Unionid { get; set; }
    }
    public class TemplateDataItem
    {
        public string value { get; set; }
        public string color { get; set; }
    }

    public class TemplateResponseDomainModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Errcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Errmsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Msgid { get; set; }
    }

    public class Miniprogram
    {
        /// <summary>
        /// 
        /// </summary>
        public string appid { get; set; } = "wx2df7a87feb6b6ed9";
        /// <summary>
        /// 
        /// </summary>
        public string pagepath { get; set; } = "pages/index/index";
    }

    public class TemplateRequestDomainModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string touser { get; set; }
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Miniprogram miniprogram { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object data { get; set; }
    }
}
