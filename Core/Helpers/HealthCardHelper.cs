using com.tencent.healthcard.HealthCardOpenAPI;
using com.tencent.healthcard.HealthOpenAuth;
using Core.Extensions;
using NUnit.Framework;
using System;
using static Core.UnifiedResponseMessage.ResponseObjectExtention;

namespace Core.Helpers
{
    public static class HealthCardHelper
    {
        private readonly static string defaultAppId = ConfigExtensions.Configuration["HealthCard:APPID"];
        public readonly static string defaultDomain = ConfigExtensions.Configuration["HealthCard:Domain"];
        public readonly static string defaultAPPSECRET = ConfigExtensions.Configuration["HealthCard:APPSECRET"];
        public readonly static string defaultHospitalID = ConfigExtensions.Configuration["HealthCard:HospitalID"];
        /// <summary>
        /// 获取APPTOKEN
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="domain"></param>
        /// <param name="appSecret"></param>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public static string GetAppToken(string appId = "", string domain = "", string appSecret = "", string hospitalID = "")
        {
            //存到redis
            if (RedisHelper.Exists("AppToken"))
            {
                return RedisHelper.Get<string>("AppToken");
            }

            if (string.IsNullOrEmpty(appId))
            {
                appId = defaultAppId;
            }

            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }

            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthOpenAuth.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.GetAppToken(new com.tencent.healthcard.HealthOpenAuth.CommonIn
            {
                hospitalId = hospitalID,
                requestId = Guid.NewGuid().ToString(),

            }, new GetAppTokenReq
            {
                appId = appId
            });
            var commonOut = (com.tencent.healthcard.HealthOpenAuth.CommonOut)tokenResult["commonOut"];
            Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            var rsp = (GetAppTokenRsp)tokenResult["rsp"];
            var appToken = rsp.appToken;
            //APPTOKEN2小时有效 缓存时间1.8小时更换
            RedisHelper.Set("AppToken", appToken, TimeSpan.FromHours(1.8));
            return appToken;
        }
        /// <summary>
        /// 注册健康卡
        /// </summary>
        /// <param name="registerHealthCardReq"></param>
        /// <param name="appToken"></param>
        /// <returns></returns>
        public static ResponseObjectDomainModel<RegisterHealthCardRsp> RegisterHealthCard(RegisterHealthCardReq registerHealthCardReq, string appToken, string domain = "", string appSecret = "", string hospitalID = "")
        {
            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }

            int channelNum = 0;
            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthCardOpenAPI.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.RegisterHealthCard(
                registerHealthCardReq,
                new com.tencent.healthcard.HealthCardOpenAPI.CommonIn
                {
                    appToken = appToken,
                    hospitalId = hospitalID,
                    requestId = Guid.NewGuid().ToString(),
                    channelNum = channelNum
                });
            var commonOut = (com.tencent.healthcard.HealthCardOpenAPI.CommonOut)tokenResult["commonOut"];
            //Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            if (commonOut.resultCode == 0)
            {
                var rsp = (RegisterHealthCardRsp)tokenResult["rsp"];
                return new ResponseObjectDomainModel<RegisterHealthCardRsp> { Data = rsp };
            }
            return new ResponseObjectDomainModel<RegisterHealthCardRsp> { Code = commonOut.resultCode, Msg = commonOut.errMsg };
        }


        /// <summary>
        /// 批量注册健康卡
        /// </summary>
        /// <param name="registerBatchHealthCardReq"></param>
        /// <param name="appToken"></param>
        /// <returns></returns>
        public static RegisterBatchHealthCardRsp RegisterBatchHealthCard(RegisterBatchHealthCardReq registerBatchHealthCardReq, string appToken, string domain = "", string appSecret = "", string hospitalID = "")
        {
            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }

            int channelNum = 0;
            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthCardOpenAPI.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.RegisterBatchHealthCard(
                registerBatchHealthCardReq,
                new com.tencent.healthcard.HealthCardOpenAPI.CommonIn
                {
                    appToken = appToken,
                    hospitalId = hospitalID,
                    requestId = Guid.NewGuid().ToString(),
                    channelNum = channelNum
                });
            var commonOut = (com.tencent.healthcard.HealthCardOpenAPI.CommonOut)tokenResult["commonOut"];
            Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            var rsp = (RegisterBatchHealthCardRsp)tokenResult["rsp"];
            return rsp;
        }

        /// <summary>
        /// 身份证OCR
        /// </summary>
        /// <param name="imageContent"></param>
        /// <param name="domain"></param>
        /// <param name="appSecret"></param>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public static OcrInfoRsp OcrInfo(string imageContent, string appToken, string domain = "", string appSecret = "", string hospitalID = "")
        {

            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }

            int channelNum = 0;
            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthCardOpenAPI.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.OcrInfo(
                new com.tencent.healthcard.HealthCardOpenAPI.OcrInfoReq
                {
                    imageContent = imageContent
                },
                new com.tencent.healthcard.HealthCardOpenAPI.CommonIn
                {
                    appToken = appToken,
                    hospitalId = hospitalID,
                    requestId = Guid.NewGuid().ToString(),
                    channelNum = channelNum
                });
            var commonOut = (com.tencent.healthcard.HealthCardOpenAPI.CommonOut)tokenResult["commonOut"];
            Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            var rsp = (OcrInfoRsp)tokenResult["rsp"];
            return rsp;
        }

        /// <summary>
        /// 用卡数据监测接口
        /// </summary>
        /// <param name="reportHISDataReq"></param>
        /// <param name="appToken"></param>
        /// <param name="domain"></param>
        /// <param name="appSecret"></param>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public static ReportHISDataRsp ReportHISData(ReportHISDataReq reportHISDataReq, string appToken, string domain = "", string appSecret = "", string hospitalID = "")
        {
            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }

            reportHISDataReq.hospitalCode = defaultHospitalID;
            reportHISDataReq.hospitalId = defaultHospitalID;

            int channelNum = 0;
            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthCardOpenAPI.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.ReportHISData(
                reportHISDataReq,
                new com.tencent.healthcard.HealthCardOpenAPI.CommonIn
                {
                    appToken = appToken,
                    hospitalId = hospitalID,
                    requestId = Guid.NewGuid().ToString(),
                    channelNum = channelNum
                });
            var commonOut = (com.tencent.healthcard.HealthCardOpenAPI.CommonOut)tokenResult["commonOut"];
            Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            var rsp = (ReportHISDataRsp)tokenResult["rsp"];
            return rsp;
        }
        /// <summary>
        /// 绑定院内ID和健康卡关系
        /// </summary>
        /// <returns></returns>
        public static bool BindCardRelation(BindCardRelationReq bindCardRelationReq, string appToken, string domain = "", string appSecret = "", string hospitalID = "")
        {
            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }
            int channelNum = 0;
            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthCardOpenAPI.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.BindCardRelation(
                bindCardRelationReq,
                new com.tencent.healthcard.HealthCardOpenAPI.CommonIn
                {
                    appToken = appToken,
                    hospitalId = hospitalID,
                    requestId = Guid.NewGuid().ToString(),
                    channelNum = channelNum
                });
            var commonOut = (com.tencent.healthcard.HealthCardOpenAPI.CommonOut)tokenResult["commonOut"];
            Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            var rsp = (BindCardRelationRsp)tokenResult["rsp"];
            return rsp.result;
        }
        /// <summary>
        /// 通过健康卡授权码获取健康卡接口
        /// </summary>
        /// <param name="getHealthCardByHealthCodeReq"></param>
        /// <param name="appToken"></param>
        /// <param name="domain"></param>
        /// <param name="appSecret"></param>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public static GetHealthCardByHealthCodeRsp GetHealthCardByHealthCode(GetHealthCardByHealthCodeReq getHealthCardByHealthCodeReq, string appToken, string domain = "", string appSecret = "", string hospitalID = "")
        {
            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }
            int channelNum = 0;
            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthCardOpenAPI.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.GetHealthCardByHealthCode(
                getHealthCardByHealthCodeReq,
                new com.tencent.healthcard.HealthCardOpenAPI.CommonIn
                {
                    appToken = appToken,
                    hospitalId = hospitalID,
                    requestId = Guid.NewGuid().ToString(),
                    channelNum = channelNum
                });
            var commonOut = (com.tencent.healthcard.HealthCardOpenAPI.CommonOut)tokenResult["commonOut"];
            Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            var rsp = (GetHealthCardByHealthCodeRsp)tokenResult["rsp"];
            return rsp;
        }
        /// <summary>
        /// 获取卡包订单ID接口
        /// </summary>
        /// <param name="getOrderIdByOutAppIdReq"></param>
        /// <param name="appToken"></param>
        /// <param name="domain"></param>
        /// <param name="appSecret"></param>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public static GetOrderIdByOutAppIdRsp GetOrderIdByOutAppId(GetOrderIdByOutAppIdReq getOrderIdByOutAppIdReq, string appToken, string domain = "", string appSecret = "", string hospitalID = "")
        {
            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }
            int channelNum = 0;
            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthCardOpenAPI.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.GetOrderIdByOutAppId(
                getOrderIdByOutAppIdReq,
                new com.tencent.healthcard.HealthCardOpenAPI.CommonIn
                {
                    appToken = appToken,
                    hospitalId = hospitalID,
                    requestId = Guid.NewGuid().ToString(),
                    channelNum = channelNum
                });
            var commonOut = (com.tencent.healthcard.HealthCardOpenAPI.CommonOut)tokenResult["commonOut"];
            Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            var rsp = (GetOrderIdByOutAppIdRsp)tokenResult["rsp"];
            return rsp;
        }
        /// <summary>
        /// 获取动态健康卡二维码
        /// </summary>
        /// <param name="getDynamicQRCodeReq"></param>
        /// <param name="appToken"></param>
        /// <param name="domain"></param>
        /// <param name="appSecret"></param>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public static ResponseObjectDomainModel<GetDynamicQRCodeRsp> GetDynamicQRCode(GetDynamicQRCodeReq getDynamicQRCodeReq, string appToken, string domain = "", string appSecret = "", string hospitalID = "")
        {
            if (string.IsNullOrEmpty(domain))
            {
                domain = defaultDomain;
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = defaultAPPSECRET;
            }

            if (string.IsNullOrEmpty(hospitalID))
            {
                hospitalID = defaultHospitalID;
            }
            int channelNum = 0;
            var ISVOpenInterfaceProxy = new com.tencent.healthcard.HealthCardOpenAPI.ISVOpenInterfaceProxy { domain = domain, appSecret = appSecret };
            var tokenResult = ISVOpenInterfaceProxy.GetDynamicQRCode(
                getDynamicQRCodeReq,
                new com.tencent.healthcard.HealthCardOpenAPI.CommonIn
                {
                    appToken = appToken,
                    hospitalId = hospitalID,
                    requestId = Guid.NewGuid().ToString(),
                    channelNum = channelNum
                });
            var commonOut = (com.tencent.healthcard.HealthCardOpenAPI.CommonOut)tokenResult["commonOut"];
            //Assert.IsTrue(commonOut.resultCode == 0, "resultCode must be 0, but found " + commonOut.resultCode.ToString());
            if (commonOut.resultCode == 0)
            {
                var rsp = (GetDynamicQRCodeRsp)tokenResult["rsp"];
                return new ResponseObjectDomainModel<GetDynamicQRCodeRsp> { Data = rsp };
            }
            return new ResponseObjectDomainModel<GetDynamicQRCodeRsp> { Code = commonOut.resultCode, Msg = commonOut.errMsg };
        }


    }
}
