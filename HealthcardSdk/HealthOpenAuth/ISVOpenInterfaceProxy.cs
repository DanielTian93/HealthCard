using com.tencent.healthcard.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace com.tencent.healthcard.HealthOpenAuth
{
    public class ISVOpenInterfaceProxy
    {
        public string domain { get; set; }
        public string appSecret { get; set; }


        public Dictionary<string, object> GetAppToken(CommonIn commonIn, GetAppTokenReq req)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("commonIn", commonIn);

            dic.Add("req", req);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenAuth/AuthObj/getAppToken", dic
            );
            var result = new Dictionary<string, object>();

            CommonOut commonOut;
            if (rpcResult.ContainsKey("commonOut"))
            {
                commonOut = JsonConvert.DeserializeObject<CommonOut>(JsonConvert.SerializeObject(rpcResult["commonOut"]));
            }
            else
            {
                commonOut = new CommonOut();
            }
            result["commonOut"] = commonOut;

            GetAppTokenRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<GetAppTokenRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new GetAppTokenRsp();
            }
            result["rsp"] = rsp;

            return result;
        }

    }
}
