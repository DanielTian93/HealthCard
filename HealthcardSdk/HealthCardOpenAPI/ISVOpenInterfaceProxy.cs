using com.tencent.healthcard.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace com.tencent.healthcard.HealthCardOpenAPI
{
    public class ISVOpenInterfaceProxy
    {
        public string domain { get; set; }
        public string appSecret { get; set; }


        public Dictionary<string, object> RegisterHealthCard(RegisterHealthCardReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/registerHealthCard", dic
            );
            var result = new Dictionary<string, object>();

            RegisterHealthCardRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<RegisterHealthCardRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new RegisterHealthCardRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> GetHealthCardByQRCode(GetHealthCardByQRCodeReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/getHealthCardByQRCode", dic
            );
            var result = new Dictionary<string, object>();

            GetHealthCardByQRCodeRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<GetHealthCardByQRCodeRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new GetHealthCardByQRCodeRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> GetHealthCardByHealthCode(GetHealthCardByHealthCodeReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/getHealthCardByHealthCode", dic
            );
            var result = new Dictionary<string, object>();

            GetHealthCardByHealthCodeRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<GetHealthCardByHealthCodeRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new GetHealthCardByHealthCodeRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> OcrInfo(OcrInfoReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/ocrInfo", dic
            );
            var result = new Dictionary<string, object>();

            OcrInfoRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<OcrInfoRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new OcrInfoRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> BindCardRelation(BindCardRelationReq req, CommonIn commonIn)
        {
            try
            {


                var dic = new Dictionary<string, object>();

                dic.Add("req", req);

                dic.Add("commonIn", commonIn);

                var rpcResult = RequestHelper.call(
                    this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/bindCardRelation", dic
                );
                var result = new Dictionary<string, object>();

                BindCardRelationRsp rsp;
                if (rpcResult.ContainsKey("rsp"))
                {
                    rsp = JsonConvert.DeserializeObject<BindCardRelationRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
                }
                else
                {
                    rsp = new BindCardRelationRsp();
                }
                result["rsp"] = rsp;

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

                return result;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public Dictionary<string, object> GetOrderIdByOutAppId(GetOrderIdByOutAppIdReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/getOrderIdByOutAppId", dic
            );
            var result = new Dictionary<string, object>();

            GetOrderIdByOutAppIdRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<GetOrderIdByOutAppIdRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new GetOrderIdByOutAppIdRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> VerifyFaceIdentity(VerifyFaceIdentityReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/verifyFaceIdentity", dic
            );
            var result = new Dictionary<string, object>();

            VerifyFaceIdentityRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<VerifyFaceIdentityRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new VerifyFaceIdentityRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> RegisterBatchHealthCard(RegisterBatchHealthCardReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/registerBatchHealthCard", dic
            );
            var result = new Dictionary<string, object>();

            RegisterBatchHealthCardRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<RegisterBatchHealthCardRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new RegisterBatchHealthCardRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> ReportHISData(ReportHISDataReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/reportHISData", dic
            );
            var result = new Dictionary<string, object>();

            ReportHISDataRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<ReportHISDataRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new ReportHISDataRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> GetDynamicQRCode(GetDynamicQRCodeReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/getDynamicQRCode", dic
            );
            var result = new Dictionary<string, object>();

            GetDynamicQRCodeRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<GetDynamicQRCodeRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new GetDynamicQRCodeRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> VerifyQRCode(VerifyQRCodeReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/verifyQRCode", dic
            );
            var result = new Dictionary<string, object>();

            VerifyQRCodeRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<VerifyQRCodeRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new VerifyQRCodeRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> UpdateHealthCardId(UpdateHealthCardIdReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/updateHealthCardId", dic
            );
            var result = new Dictionary<string, object>();

            UpdateHealthCardIdRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<UpdateHealthCardIdRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new UpdateHealthCardIdRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

        public Dictionary<string, object> RegisterOrder(RegisterOrderReq req, CommonIn commonIn)
        {

            var dic = new Dictionary<string, object>();

            dic.Add("req", req);

            dic.Add("commonIn", commonIn);

            var rpcResult = RequestHelper.call(
                this.appSecret, "getToken", "", "https://" + this.domain + "/rest/auth/HealthCard/HealthOpenPlatform/ISVOpenObj/registerOrder", dic
            );
            var result = new Dictionary<string, object>();

            RegisterOrderRsp rsp;
            if (rpcResult.ContainsKey("rsp"))
            {
                rsp = JsonConvert.DeserializeObject<RegisterOrderRsp>(JsonConvert.SerializeObject(rpcResult["rsp"]));
            }
            else
            {
                rsp = new RegisterOrderRsp();
            }
            result["rsp"] = rsp;

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

            return result;
        }

    }
}
