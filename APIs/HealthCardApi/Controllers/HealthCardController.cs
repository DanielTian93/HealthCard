using com.tencent.healthcard.HealthCardOpenAPI;
using Core;
using Core.Enum;
using Core.Extensions;
using Core.Filter;
using Core.Helpers;
using Core.Models.DBModels;
using Core.UnifiedResponseMessage;
using HealthCardServices.DomainModel;
using HYSoft.Wechat.HealthCardServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Core.Helpers.HISHelper;
using static Core.UnifiedResponseMessage.ResponseObjectExtention;

namespace HealthCardApi.Controllers
{
    [Route("HealthCard")]
    public class HealthCardController : ControllerBase
    {
        private readonly IHealthCardService _healthCardService;
        private readonly IdWorker _IdWorker;
        private readonly EFContext _eFContext;
        public HealthCardController(IHealthCardService healthCardService, EFContext eFContext)
        {
            _IdWorker = SnowflakeHelper.IdWorker;
            _healthCardService = healthCardService;
            _eFContext = eFContext;
        }

        ///// <summary>
        ///// 测试
        ///// </summary>
        ///// <param name="Code"></param>
        ///// <returns></returns>
        //[HttpPost("Test")]
        //[NoToken]
        //public object Test([FromQuery]string code)
        //{
        //    var wxInfo = WeiXinHelper.GetOpenAccessToken(code);
        //    return wxInfo;
        //}
        /// <summary>
        /// 通过微信Code获取Openid
        /// </summary>
        /// <param name="code">微信Code</param>
        /// <returns></returns>
        [HttpGet("GetOpenAccessToken")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<WXOpenAccessTokenDomainModel>), (int)HttpStatusCode.OK)]
        public object GetOpenAccessToken([FromQuery] string code)
        {
            var wxInfo = WeiXinHelper.GetOpenAccessToken(code);
            return this.Success(wxInfo);
        }

        /// <summary>
        /// 身份证OCR
        /// </summary>
        /// <returns></returns>
        [HttpPost("IDCardOCR")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<OcrInfoRsp>), (int)HttpStatusCode.OK)]
        public object IDCardOCR([FromBody] IDCardOCRRequestDomainModel req)
        {
            var res = _healthCardService.IDCardOCR(req.ImageContent);
            return this.Success(res);
        }


        /// <summary>
        /// 注册健康卡
        /// </summary>
        /// <param name="req"></param>
        [HttpPost("RegisterHealthCard")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<bool>), (int)HttpStatusCode.OK)]
        public async Task<object> RegisterHealthCardAsync([FromBody] RegisterHealthCardRequestDomainModel req)
        {
            if (string.IsNullOrEmpty(req.RegisterHealthCardReq.openId))
            {
                if (string.IsNullOrEmpty(req.Code))
                {
                    return this.Error(ErrCodeCommon.OpenIdOrCodeIsNullError);
                }
                var wxInfo = WeiXinHelper.GetOpenAccessToken(req.Code);
                req.RegisterHealthCardReq.openId = wxInfo.Openid;
            }
            if (string.IsNullOrEmpty(req.CardNo))
            {
                _healthCardService.RegisterHealthCardErrorLog(new RegisterHealthCardErrorLog { From = "RegisterHealthCardAsync", Data = JsonConvert.SerializeObject(req), Info = $"就诊卡为空" });
                return this.Error(ErrCodeCommon.CardIsNull);
            }

            var res = await _healthCardService.RegisterHealthCardAsync(req);

            return res.Code == 0 ? this.Success(res.Data) : this.Error(code: res.Code.ToString(), msg: res.Msg, null);
        }
        /// <summary>
        /// His就诊卡注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("HISJZKRegister")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<List<PatientInfo>>), (int)HttpStatusCode.OK)]
        public object HISJZKRegister([FromBody] HISQueryOrRegisterRequestDomainModel req)
        {
            var query = _healthCardService.HISJZKQuery(req.IdNumber);
            if (query.Data != null)
                return this.Success(query.Data.JZKInfo);
            var info = _healthCardService.HISQueryOrRegister(req);
            return info.Code != 0 ? this.Error(code: info.Code.ToString(), msg: info.Msg, null) : this.Success(info.Data);
        }
        /// <summary>
        /// His就诊卡查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("HISJZKQuery")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<HISJZKQueryResponseDomainModel>), (int)HttpStatusCode.OK)]
        public object HISJZKQuery([FromBody] HISQueryDomainModel req)
        {
            var query = _healthCardService.HISJZKQuery(req.IdNumber);
            return this.Success(query.Data);
        }

        /// <summary>
        /// His就诊卡绑定
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("HISJZKBind")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<List<PatientInfo>>), (int)HttpStatusCode.OK)]
        public object HISJZKBind([FromBody] HISBindDomainModel req)
        {
            var res = _healthCardService.HISBind(req, "HISJZKBind");
            return res ? this.Success(res) : this.Error(ErrCodeCommon.BindWxErr);
        }

        /// <summary>
        /// 批量注册健康卡 老用户升级 未开放权限不可使用
        /// </summary>
        /// <returns></returns>
        [HttpPost("RegisterBatchHealthCard")]
        [NoToken]
        public async Task<object> RegisterBatchHealthCard()
        {
            await _healthCardService.RegisterBatchHealthCardAsync();
            return this.Success(true);
        }
        /// <summary>
        /// 就诊卡升级健康卡
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("OldCardRegisterHealthCard")]
        [NoToken]
        public object OldCardRegisterHealthCard([FromBody] OldCardRegisterHealthCardRequestDomainModel req)
        {
            var res = _healthCardService.OldCardRegisterHealthCard(req);
            return res.Code == 0 ? this.Success(res.Data) : this.Error(code: res.Code.ToString(), msg: res.Msg, null);
        }
        /// <summary>
        /// 查询已注册健康卡数据列表(此接口只查询了OpenId对应的健康卡 无就诊卡)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("HealthCardQuery")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<List<HealthCardQueryResponseDomainModel>>), (int)HttpStatusCode.OK)]
        public object HealthCardQuery([FromBody] HealthCardQueryRequestDomainModel req)
        {
            var res = _healthCardService.HealthCardQuery(req).AsNoTracking().ToList();
            return this.Success(res);
        }

        /// <summary>
        /// 查询 就诊卡 以及 健康卡信息（如果没有健康卡 健康卡信息为空）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("AllCardQuery")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<List<AllCardQueryResponseDomainModel>>), (int)HttpStatusCode.OK)]
        public object AllCardQuery([FromBody] AllCardQueryRequestDomainModel req)
        {
            var res = _healthCardService.AllCardQuery(req);
            return this.Success(res);
        }
        /// <summary>
        /// 用卡数据监测接口 提供MEMID获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReportHISDataMem")]
        [NoToken]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void ReportHISDataMem([FromBody] ReportHISDataReuestDomainModel req)
        {
            //NLogHelper.InfoLog($"用卡数据({req.From})：" + JsonConvert.SerializeObject(req));
            _healthCardService.ReportHisDataMem(req);
        }
        /// <summary>
        /// 用卡数据监测接口 提供所需所有数据
        /// </summary>
        /// <param name="req"></param>
        [HttpPost("ReportHisData")]
        [NoToken]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void ReportHisData([FromBody] ReportHISDataReq req)
        {
            _healthCardService.ReportHISData(req);
        }

        /// <summary>
        /// 通过健康卡授权码获取健康卡接口(后台完成建档绑定)
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHealthCardByHealthCode")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<RegisterHealthCardInfo>), (int)HttpStatusCode.OK)]
        public async Task<object> GetHealthCardByHealthCodeAsync([FromBody] GetHealthCardByHealthCodeRequestDomainModel req)
        {
            try
            {
                //通过HealthCode获取健康卡信息
                var info = _healthCardService.GetHealthCardByHealthCode(new GetHealthCardByHealthCodeReq { healthCode = req.HealthCode });

                if (string.IsNullOrEmpty(req.OpenId))
                {
                    if (string.IsNullOrEmpty(req.Code))
                    {
                        return this.Error(ErrCodeCommon.OpenIdOrCodeIsNullError);
                    }
                    var wxInfo = WeiXinHelper.GetOpenAccessToken(req.Code);
                    req.OpenId = wxInfo.Openid;
                }
                var hisRes = _healthCardService.HISQueryOrRegister(new HISHelper.HISQueryOrRegisterRequestDomainModel
                {
                    IdNumber = info.card.idNumber,
                    Address = info.card.address,
                    Gender = info.card.gender,
                    Name = info.card.name,
                    Phone1 = info.card.phone1
                });
                if (hisRes.Code != 0)
                    return this.Error(code: hisRes.Code.ToString(), msg: hisRes.Msg, false);
                ////向His提交绑定
                ////向His提交绑定关系
                //var hisJsonG3 = HISHelper.RequestHisApi("PATIENT003G3", new PATIENT003G3 { PatientNO = hisRes.Data, VUID = info.card.healthCardId });
                //var hisResG3 = JsonConvert.DeserializeObject<HisResponse>(hisJsonG3);
                _healthCardService.HisBind(hisRes.Data, info.card.healthCardId, "GetHealthCardByHealthCodeAsync");
                //FollowUp注册数据
                var register = new MedBindInputViewModel
                {
                    MedName = string.IsNullOrEmpty(info.card.name) ? "无名字" : info.card.name,
                    MedCardid = string.IsNullOrEmpty(info.card.idNumber) ? _IdWorker.NextId().ToString() : info.card.idNumber,
                    MedAddress = string.IsNullOrEmpty(info.card.address) ? "无地址" : info.card.address,
                    MedLinkMobile = string.IsNullOrEmpty(info.card.phone1) ? "1" : info.card.phone1,
                    OpenId = req.OpenId,
                    CardNo = hisRes.Data,
                    Birthday = Convert.ToDateTime(info.card.birthday),
                    Sex = info.card.gender == "男" ? 0 : 1,
                };

                var appToken = HealthCardHelper.GetAppToken();
                //向腾讯提交关系绑定
                var res = HealthCardHelper.BindCardRelation(new BindCardRelationReq { patid = register.MedCardid, qrCodeText = info.card.qrCodeText }, appToken);
                //存储注册信息
                var insertData = new RegisterHealthCardInfo
                {
                    Id = _IdWorker.NextId(),
                    HealthCardId = info.card.healthCardId,
                    AdminExt = info.card.adminExt,
                    CreateTime = DateTime.Now,
                    IdNumber = info.card.idNumber,
                    IdType = info.card.idType,
                    Openid = req.OpenId,
                    Patid = info.card.patid,
                    Phid = info.card.phid,
                    QrcodeText = info.card.qrCodeText,
                    Type = 1,
                    MemId = 12,//"就诊卡号",
                    Phone = info.card.phone1
                };
                return this.Success(insertData);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取卡包订单ID接口
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetOrderIdByOutAppId")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<GetHealthCardByHealthCodeRsp>), (int)HttpStatusCode.OK)]
        public object GetOrderIdByOutAppId([FromQuery] GetOrderIdByOutAppIdReq req)
        {
            var res = _healthCardService.GetOrderIdByOutAppId(req);
            return this.Success(res);
        }
        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="memid">用户id</param>
        /// <param name="openId">微信openid</param>
        /// <returns></returns>
        [HttpPost("UnBindlingHealthCard")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<bool>), (int)HttpStatusCode.OK)]
        public object UnBindlingHealthCard([FromQuery] int memid, [FromQuery] string openId)
        {
            var res = _healthCardService.UnBindlingHealthCard(memid, openId);
            return this.Success(res);
        }
        /// <summary>
        /// 升级已有健康卡的历史数据 绑定关系
        /// </summary>
        [HttpPost("FixHistoryInfo")]
        [NoToken]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void FixHistoryInfo()
        {
            _healthCardService.FixHistoryInfo();
        }
        /// <summary>
        /// 绑定修复
        /// </summary>
        [HttpPost("FixBind")]
        [NoToken]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void FixBind()
        {
            _healthCardService.FixBind();
        }

        /// <summary>
        /// HIS注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("HisRegister")]
        [NoToken]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public object HisRegister([FromBody] HISQueryOrRegisterRequestDomainModel req)
        {
            var hisRes = _healthCardService.HISQueryOrRegister(req);
            return JsonConvert.SerializeObject(hisRes);
        }

        /// <summary>
        /// 获取动态健康卡二维码
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDynamicQRCode")]
        [NoToken]
        [ProducesResponseType(typeof(ResponseObject<GetDynamicQRCodeRsp>), (int)HttpStatusCode.OK)]
        public object GetDynamicQRCode([FromBody] GetDynamicQRCodeReq req)
        {

            var res = _healthCardService.GetDynamicQRCode(req);
            return this.Success(res);
        }

    }
}
