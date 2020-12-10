using Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Core.Helpers
{
    public static class HISHelper
    {
        private static string SerializeOption(object option)
        {
            var json = JsonConvert.SerializeObject(option);
            return string.Concat("[", json.Replace("\"", "'"), "]");
        }
        /// <summary>
        /// 向HIS发起请求
        /// </summary>
        /// <param name="bizCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RequestHisInspectionApi(string bizCode, object data)
        {
            var hisUrl = ConfigExtensions.Configuration["AppSettings:HisInspectionURI"];
            var postData = new Dictionary<string, string>();
            postData.Add("as_token", null);
            postData.Add("as_code", bizCode);
            postData.Add("as_data", SerializeOption(data));
            var res = HttpHelper.PostWithFormAsync(url: hisUrl, postData).Result;
            //NLogHelper.InfoLog($"调用His接口：{bizCode}-[{JsonConvert.SerializeObject(data)}]==>{res}||请求地址{hisUrl}");
            return res;
        }

        /// <summary>
        /// 向HIS发起请求
        /// </summary>
        /// <param name="bizCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RequestHisApi(string bizCode, object data)
        {
            var hisUrl = ConfigExtensions.Configuration["AppSettings:HisURI"];
            var postData = new Dictionary<string, string>
            {
                { "as_token", null },
                { "as_code", bizCode },
                { "as_data", SerializeOption(data) }
            };
            var res = HttpHelper.PostWithFormAsync(url: hisUrl, postData).Result;
            NLogHelper.InfoLog($"调用His接口：{bizCode}-[{SerializeOption(data)}]==>{res}||请求地址{hisUrl}");
            return res;
        }
        /// <summary>
        /// 向HIS发起请求 http://10.10.210.218:8881/api/DataService
        /// </summary>
        /// <param name="bizCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RequestHisApiGet(string bizCode, object data)
        {
            var hisUrl = ConfigExtensions.Configuration["AppSettings:HisURIGet"];
            var postData = new Dictionary<string, string>
            {
                { "as_token", null },
                { "as_code", bizCode },
                { "as_data", SerializeOption(data) }
            };
            var res = HttpHelper.PostWithFormAsync(url: hisUrl, postData).Result;
#if DEBUG
            NLogHelper.InfoLog(res);
#endif

            return JsonConvert.DeserializeObject<HisApiGetDomainModel>(res).CONTENT;
        }
        public class HisApiGetDomainModel
        {
            /// <summary>
            /// 
            /// </summary>
            public string RESULTCODE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string RESULT { get; set; }

            public string CONTENT { get; set; }
        }
        public class HisResponse
        {
            /// <summary>
            /// 返回code 1成功
            /// </summary>
            public string RESULTCODE { get; set; }
            /// <summary>
            /// 成功
            /// </summary>
            public string RESULT { get; set; }
            /// <summary>
            /// [{"PatientNO":"1001711369","PatientName":"田园"}] [{\"PatientNO\":\"1001712153\",\"PatientName\":\"王逸飞\"}]
            /// </summary>
            public string CONTENT { get; set; }
        }

        public class PatientInfo
        {
            /// <summary>
            /// 就诊卡号
            /// </summary>
            public string PatientNO { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string PatientName { get; set; }
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        public class PATIENT003G1
        {
            /// <summary>
            /// 身份证
            /// </summary>
            public string IDCard { get; set; }
        }
        /// <summary>
        /// 向HIS新增用户
        /// </summary>
        public class PATIENT003G2
        {
            /// <summary>
            /// 身份证
            /// </summary>
            public string IDCard { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string PatientName { get; set; }
            /// <summary>
            /// 1男 2女
            /// </summary>
            public string Gender { get; set; }
            /// <summary>
            /// 手机
            /// </summary>
            public string Mobilephone { get; set; }
            /// <summary>
            /// 地址
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// 0否 1是
            /// </summary>
            public string lsNoChild { get; set; } = "0";

        }
        public class PATIENT003G2Child
        {
            /// <summary>
            /// 身份证
            /// </summary>
            public string IDCard { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string PatientName { get; set; }
            /// <summary>
            /// 1男 2女
            /// </summary>
            public string Gender { get; set; }
            /// <summary>
            /// 手机
            /// </summary>
            public string Mobilephone { get; set; }
            /// <summary>
            /// 地址
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// 0否 1是
            /// </summary>
            public string lsNoChild { get; set; } = "0";
            /// <summary>
            /// 监护人身份证号
            /// </summary>
            public string GuaranteeIdCard { get; set; }
            /// <summary>
            /// 监护人姓名
            /// </summary>
            public string GuaranteeName { get; set; }
            /// <summary>
            /// 与监护人的关系
            /// </summary>
            public string GuaranteeType { get; set; }
        }

        public class PATIENT003G3
        {
            /// <summary>
            /// 用户就诊卡号
            /// </summary>
            public string PatientNO { get; set; }
            /// <summary>
            /// 健康卡卡号
            /// </summary>
            public string VUID { get; set; }
        }
        public class HISQueryOrRegisterRequestDomainModel
        {
            public string IdNumber { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }
            public string Phone1 { get; set; }
            public string Address { get; set; }
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
            public string GuaranteeName { get; set; }
            /// <summary>
            /// 与监护人的关系①、配偶：1；②、父母：2；③、子女：3④、祖父母：4⑤、孙子女：5⑥、兄弟姐妹：6⑦、叔叔阿姨：7⑧、侄子侄女：8⑨、同事同学：9⑩、其他：0

            /// </summary>
            public string GuaranteeType { get; set; }
        }
        public class HISQueryDomainModel
        {
            public string IdNumber { get; set; }
        }
        public class HISBindDomainModel
        {
            public string CardNo { get; set; }
            public string HealthCardId { get; set; }
        }

        public class HISJZKQueryResponseDomainModel
        {
            public List<PatientInfo> JZKInfo { get; set; }
            public string JZKBind { get; set; }
        }
    }

}
