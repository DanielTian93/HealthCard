using AutoMapper;
using com.tencent.healthcard.HealthCardOpenAPI;
using Core;
using Core.Enum;
using Core.Extensions;
using Core.Helpers;
using Core.Models.DBModels;
using Core.UnifiedResponseMessage;
using HealthCardServices.DomainModel;
using Newtonsoft.Json;
using NLog;
using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Core.Helpers.HISHelper;
using static Core.UnifiedResponseMessage.ResponseObjectExtention;

namespace HYSoft.Wechat.HealthCardServices.Services
{
    public class HealthCardService : IHealthCardService
    {
        private readonly EFContext _eFContext;
        private readonly IdWorker _IdWorker;
        private readonly IMapper _mapper;
        private readonly Logger _logger;

        public HealthCardService(
            EFContext eFContext,
            IMapper mapper)
        {
            _eFContext = eFContext;
            _IdWorker = SnowflakeHelper.IdWorker;
            _mapper = mapper;
            _logger = LogManager.GetLogger("healthCard");
        }
        /// <summary>
        /// 身份证OCR
        /// </summary>
        /// <param name="imageContent"></param>
        /// <returns></returns>
        public OcrInfoRsp IDCardOCR(string imageContent)
        {
            var appToken = HealthCardHelper.GetAppToken();
            var res = HealthCardHelper.OcrInfo(imageContent, appToken);
            return res;
        }
        //ISV获取用户信息，调用开放平台 注册健康卡接口，提交用户身份信息、wechatCode参数，开放平台会对身份证号码和姓名进行实名核身
        /// <summary>
        /// 注册健康卡
        /// </summary>
        /// <param name="req"></param>
        public async Task<ResponseObjectDomainModel<bool>> RegisterHealthCardAsync(RegisterHealthCardRequestDomainModel req)
        {
            var register = req.RegisterHealthCardReq;
            if (string.IsNullOrEmpty(register.idNumber))
            {
                RegisterHealthCardErrorLog(new RegisterHealthCardErrorLog { From = "RegisterHealthCardAsync", Data = JsonConvert.SerializeObject(req), Info = "身份证为空，注册失败" });
                _logger.Info($"注册健康卡：=》{JsonConvert.SerializeObject(req)};身份证为空，注册失败");
                return new ResponseObjectDomainModel<bool> { Code = (int)ErrCodeCommon.IdCardNullError };
            }
            if (req.RegisterHealthCardReq.nation.Contains("族"))
                req.RegisterHealthCardReq.nation = req.RegisterHealthCardReq.nation;
            else
                req.RegisterHealthCardReq.nation = req.RegisterHealthCardReq.nation + "族";
            req.RegisterHealthCardReq.patid = register.idNumber;
            req.RegisterHealthCardReq.registeredType = RegisteredType.SINGLE_REGISTERED;
            var appToken = HealthCardHelper.GetAppToken();
            //向腾讯提交信息 人证合一
            var res = HealthCardHelper.RegisterHealthCard(req.RegisterHealthCardReq, appToken);
            if (res.Code != 0)
            {
                _logger.Info($"注册健康卡：=》{JsonConvert.SerializeObject(req)};腾讯提交注册失败:{res.Code};{res.Msg}");
                RegisterHealthCardErrorLog(new RegisterHealthCardErrorLog { From = "RegisterHealthCardAsync", Data = JsonConvert.SerializeObject(req), Info = $"腾讯提交注册失败:{res.Code};{res.Msg}" });
                return new ResponseObjectDomainModel<bool>
                {
                    Data = false,
                    Code = res.Code,
                    Msg = res.Msg
                };
            }

            //此处向HIS提交注册用户信息 在HIS中创建用户 获取用户信息
            var info = HISQueryOrRegister(new HISQueryOrRegisterRequestDomainModel { });
            //向腾讯提交绑定关系
            HealthCardHelper.BindCardRelation(new BindCardRelationReq { patid = "医院内用户ID", qrCodeText = res.Data.qrCodeText }, appToken);
            //向His提交绑定关系

            HISBind(new HISBindDomainModel { CardNo = req.CardNo, HealthCardId = res.Data.healthCardId });

            return new ResponseObjectDomainModel<bool> { Data = true };
        }
        /// <summary>
        /// His查询就诊卡
        /// </summary>
        /// <returns></returns>
        public ResponseObjectDomainModel<HISJZKQueryResponseDomainModel> HISJZKQuery(string idnumber)
        {
            //此处查询用户纠正卡
            return default;
        }

        /// <summary>
        /// 向HIS提交健康卡绑定院内就诊卡
        /// </summary>
        /// <param name="req"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public bool HISBind(HISBindDomainModel req, string from = "")
        {
            //向His提交绑定关系
            var healthCardInfo = _eFContext.RegisterHealthCardInfo.FirstOrDefault(x => x.HealthCardId == req.HealthCardId && x.Status == 1);
            var chatUser = _eFContext.ChatMember.Where(x => x.MemCard == healthCardInfo.IdNumber);
            if (chatUser != null)
            {
                HisBind(req.CardNo, req.HealthCardId, $"HISBind-{from}");

                foreach (var item in chatUser)
                {
                    item.MemCheckCard = req.CardNo;
                }
                _eFContext.UpdateRange(chatUser);
                _eFContext.SaveChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 注册HIS用户 如果存在返回查询信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResponseObjectDomainModel<string> HISQueryOrRegister(HISQueryOrRegisterRequestDomainModel req)
        {
            string cardNo = "";
            var hisG1 = new PATIENT003G1
            {
                IDCard = req.IdNumber
            };
            //查询
            var hisResG1 = new HisResponse { };
            if (hisResG1.RESULTCODE == "1")
            {

                var hisG1Info = JsonConvert.DeserializeObject<List<PatientInfo>>(hisResG1.CONTENT.Replace("\\", "")).FirstOrDefault();
                cardNo = hisG1Info.PatientNO;
            }
            else
            {

                //注册
                var hisResG2 = new HisResponse { };

                //注册成功
                if (hisResG2.RESULTCODE == "1")
                {
                    var hisG2Info = JsonConvert.DeserializeObject<List<PatientInfo>>(hisResG2.CONTENT.Replace("\\", "")).FirstOrDefault();
                    cardNo = hisG2Info.PatientNO;
                }
                else if (hisResG2.RESULTCODE != "1")
                {
                    _logger.Info($"注册健康卡：=》{JsonConvert.SerializeObject(req)};HIS提交注册失败，且查询不到用户:注册返回信息（{hisResG2.RESULTCODE};{hisResG2.RESULT}）；");
                    RegisterHealthCardErrorLog(new RegisterHealthCardErrorLog { From = "HISQueryOrRegister", Data = JsonConvert.SerializeObject(req), Info = $"HIS提交注册失败，且查询不到用户:注册返回信息（{hisResG2.RESULTCODE};{hisResG2.RESULT}）" });
                    return new ResponseObjectDomainModel<string> { Code = Convert.ToInt32(hisResG2.RESULTCODE), Msg = hisResG2.RESULT };
                }
                if (string.IsNullOrEmpty(cardNo))
                {
                    _logger.Info($"注册健康卡：=》{JsonConvert.SerializeObject(req)};His就诊卡卡号为空{cardNo}");
                    RegisterHealthCardErrorLog(new RegisterHealthCardErrorLog { From = "HISQueryOrRegister", Data = JsonConvert.SerializeObject(req), Info = $"His就诊卡卡号为空{cardNo}" });
                    return new ResponseObjectDomainModel<string>
                    {
                        Code = (int)ErrCodeCommon.HisError,
                        Msg = EnumDescriptionHelper.GetEnumDescription(ErrCodeCommon.HisError)
                    };
                }
            }
            return new ResponseObjectDomainModel<string> { Data = cardNo };
        }
        /// <summary>
        /// 向His提交绑定
        /// </summary>
        /// <returns></returns>
        public void HisBind(string patientNO, string healthCardId, string from)
        {
            //向His提交绑定关系

            var hisRes = new { RESULTCODE = "1", RESULT = "" };
            if (hisRes.RESULTCODE != "1")
            {
                _logger.Info($"注册健康卡：=》{JsonConvert.SerializeObject(hisRes)};HIS提交绑定失败：返回信息（{hisRes.RESULTCODE};{hisRes.RESULT}）；");
                RegisterHealthCardErrorLog(new RegisterHealthCardErrorLog { From = $"{from}HisBind", Data = $"{patientNO}/{healthCardId}", Info = $"HIS提交绑定失败：返回信息（{hisRes.RESULTCODE};{hisRes.RESULT}）；" });
            }
        }
        /// <summary>
        /// 根据OpenId获取就诊卡
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private List<ChatMember> GetMemberList(string openId)
        {
            return default;
        }
        /// <summary>
        /// 根据就诊卡获取用户相关信息
        /// </summary>
        /// <param name="memid"></param>
        /// <returns></returns>
        private RegisterHealthCardReq GetMemberInfo(int memid)
        {
            //var memres = HttpHelper.Get($"{ConfigExtensions.Configuration["AppSettings:FollowUpURI"]}/Book/QueryCardInfo?memid={memid}");
            //return JsonConvert.DeserializeObject<RegisterHealthCardReq>(memres);
            return default;
        }

        /// <summary>
        /// 解绑Openid相关的就诊卡
        /// </summary>
        /// <param name="memid"></param>
        /// <returns></returns>
        public bool UnBindlingHealthCard(int memid, string openId)
        {
            //解绑就诊卡
            return default;
        }
        /// <summary>
        /// 查询 就诊卡 以及 健康卡信息（如果没有健康卡 健康卡信息为空）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<AllCardQueryResponseDomainModel> AllCardQuery(AllCardQueryRequestDomainModel req)
        {
            var memList = GetMemberList(req.OpenId);
            if (memList.Any())
            {
                var res = _mapper.Map<List<AllCardQueryResponseDomainModel>>(memList);
                //查询健康卡
                //foreach (var item in res)
                //{
                //    //查询是否有健康卡 赋值
                //    item.RegisterHealthCardInfo = ;
                //}
                return res;
            }
            return null;
        }
        /// <summary>
        /// 查询已注册健康卡数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<HealthCardQueryResponseDomainModel> HealthCardQuery(HealthCardQueryRequestDomainModel req)
        {
            var query = from hi in _eFContext.RegisterHealthCardInfo
                        join cm in _eFContext.ChatMember on hi.MemId equals cm.MemId
                        select new HealthCardQueryResponseDomainModel
                        {
                            Id = hi.Id,
                            Openid = hi.Openid,
                            Patid = hi.Patid,
                            QrcodeText = hi.QrcodeText,
                            HealthCardId = hi.HealthCardId,
                            Phid = hi.Phid,
                            AdminExt = hi.AdminExt,
                            IdNumber = hi.IdNumber,
                            IdType = hi.IdType,
                            Type = hi.Type,
                            CreateTime = hi.CreateTime,
                            MemId = hi.MemId,
                            Phone = hi.Phone,
                            Status = hi.Status,
                            Name = cm.MemName,
                        };

            //如果只有Code 没有OpenId 则通过Code获取Openid
            if (!string.IsNullOrEmpty(req.Code) && string.IsNullOrEmpty(req.OpenId))
            {
                var wxInfo = WeiXinHelper.GetOpenAccessToken(req.Code);
                req.OpenId = wxInfo.Openid;
            }

            if (!string.IsNullOrEmpty(req.OpenId))
            {
                //var openid = WeiXinHelper.GetOpenAccessToken(req.Code).Openid;
                query = query.Where(x => x.Openid == req.OpenId);
            }
            query = query.WhereIF(!(req.Status == -99 || req.Status == null), x => x.Status == req.Status);
            query = query.WhereIF(!string.IsNullOrEmpty(req.Name), x => x.Openid == req.Name);
            query = query.WhereIF(!string.IsNullOrEmpty(req.IdNumber), x => x.Openid == req.IdNumber);
            return query;
        }
        /// <summary>
        /// 批量注册健康卡 老用户升级 无效
        /// </summary>
        /// <returns></returns>
        public async Task RegisterBatchHealthCardAsync()
        {
            //获取用户基础信息
            var memList = (from cu in _eFContext.ChatUser
                           join cm in _eFContext.ChatMember on cu.SubId equals cm.SubId
                           where cm.IsUse == 1 && cm.MemId == 358175
                           select new BatchHealthCardItem
                           {
                               wechatCode = "",
                               name = cm.MemName,
                               gender = cm.Sex == 0 ? "男" : "女",
                               nation = "未知",
                               birthday = ((DateTime)cm.Birthday).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo),
                               idCard = cm.MemCard,
                               cardType = "01",
                               address = cm.MedAddress,
                               phone1 = cm.MemTel,
                               patid = cm.MemId.ToString(),
                               openId = cu.Openid,
                               wechatUrl = "",
                               idNumber = cm.MemCard,
                               idType = "01"
                           }).ToList();
            var appToken = HealthCardHelper.GetAppToken();
            //向腾讯提交信息
            var res = HealthCardHelper.RegisterBatchHealthCard(new RegisterBatchHealthCardReq
            {
                healthCardItems = memList.ToArray()
            }, appToken);
            //存储注册信息
            var insertList = new List<RegisterHealthCardInfo>();
            foreach (var item in res.rspItems)
            {
                if (!string.IsNullOrEmpty(item.qrCodeText))
                {
                    var meminfo = memList.First(x => x.idNumber == item.idNumber);
                    var insertData = new RegisterHealthCardInfo
                    {
                        Id = _IdWorker.NextId(),
                        HealthCardId = item.healthCardId,
                        AdminExt = "",
                        CreateTime = DateTime.Now,
                        IdNumber = item.idNumber,
                        IdType = item.idType,
                        Openid = meminfo.openId,
                        Patid = meminfo.patid,
                        Phid = "",
                        QrcodeText = item.qrCodeText,
                        Type = 1,
                        Phone = meminfo.phone1
                    };
                    insertList.Add(insertData);
                }
            }
            await _eFContext.AddRangeAsync(insertList);
            _eFContext.SaveChanges();
        }

        /// <summary>
        /// 用卡数据监测接口
        /// </summary>
        /// <param name="reportHISDataReq"></param>
        /// <returns></returns>
        public void ReportHISData(ReportHISDataReq reportHISDataReq)
        {
            var appToken = HealthCardHelper.GetAppToken();
            HealthCardHelper.ReportHISData(reportHISDataReq, appToken);
        }
        /// <summary>
        /// 用卡数据监测接口 提供MemId
        /// </summary>
        /// <param name="req"></param>
        public void ReportHisDataMem(ReportHISDataReuestDomainModel req)
        {
            //根据MEMID获取用户信息
            var memInfo = _eFContext.ChatMember.Find(req.MemId);
            if (memInfo == null)
                return;
            var healthCard = _eFContext.RegisterHealthCardInfo.FirstOrDefault(x => x.MemId == req.MemId);
            if (healthCard == null)
                return;
            var reportHISDataReq = new ReportHISDataReq
            {
                scene = req.Scene,
                cardType = req.CardType,
                department = req.Department,
                cardCostTypes = req.CardCostTypes,
                qrCodeText = healthCard.QrcodeText,
                idCardNumber = healthCard.IdNumber,
                name = memInfo.MemName,
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                cardChannel = "0401",
            };

            var appToken = HealthCardHelper.GetAppToken();
            HealthCardHelper.ReportHISData(reportHISDataReq, appToken);
        }

        /// <summary>
        /// 通过健康卡授权码获取健康卡接口
        /// </summary>
        /// <returns></returns>
        public GetHealthCardByHealthCodeRsp GetHealthCardByHealthCode(GetHealthCardByHealthCodeReq req)
        {
            var appToken = HealthCardHelper.GetAppToken();
            var res = HealthCardHelper.GetHealthCardByHealthCode(req, appToken);
            return res;
        }
        /// <summary>
        /// 老卡升级健康卡
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResponseObjectDomainModel<bool> OldCardRegisterHealthCard(OldCardRegisterHealthCardRequestDomainModel req)
        {
            NLogHelper.DebugInfoLog($"升级健康卡(入参)：{JsonConvert.SerializeObject(req)}");

            //获取就诊卡信息
            var registerHealthCardReq = GetMemberInfo(req.Memid);
            registerHealthCardReq.registeredType = RegisteredType.SINGLE_REGISTERED;
            registerHealthCardReq.openId = req.OpenId;
            registerHealthCardReq.wechatCode = req.WechatCode;
            var appToken = HealthCardHelper.GetAppToken();
            //向腾讯提交信息
            var res = HealthCardHelper.RegisterHealthCard(registerHealthCardReq, appToken);
            NLogHelper.DebugInfoLog($"升级健康卡(腾讯提交信息返回)：{JsonConvert.SerializeObject(res)}");

            if (res.Code != 0)
            {
                RegisterHealthCardErrorLog(new RegisterHealthCardErrorLog { From = "OldCardRegisterHealthCard", Data = $"{JsonConvert.SerializeObject(req)}||{JsonConvert.SerializeObject(registerHealthCardReq)}", Info = $"腾讯提交注册失败:{res.Code};{res.Msg}" });
                _logger.Info($"老卡升级：=》{JsonConvert.SerializeObject(req)};腾讯提交注册失败:{res.Code};{res.Msg}");
                return new ResponseObjectDomainModel<bool> { Code = res.Code, Msg = res.Msg };
            }
         
            //就诊卡号
            string cardNo;
            var hisRes = HISQueryOrRegister(new HISQueryOrRegisterRequestDomainModel
            {
                IdNumber = registerHealthCardReq.idNumber,
                Address = registerHealthCardReq.address,
                Gender = registerHealthCardReq.gender,
                Name = registerHealthCardReq.name,
                Phone1 = registerHealthCardReq.phone1,

            });
            if (hisRes.Code != 0)
                return new ResponseObjectDomainModel<bool> { Code = hisRes.Code, Msg = hisRes.Msg };
            else
            {
                cardNo = hisRes.Data;
            }
            //向腾讯提交绑定关系
            HealthCardHelper.BindCardRelation(new BindCardRelationReq { patid = registerHealthCardReq.patid, qrCodeText = res.Data.qrCodeText }, appToken);
            //向His提交绑定关系
            HisBind(cardNo, res.Data.healthCardId, "OldCardRegisterHealthCard");

            //存储注册信息
         
            return new ResponseObjectDomainModel<bool> { Data = true };
        }

        /// <summary>
        /// 升级已有健康卡的历史数据 绑定关系
        /// </summary>
        public void FixHistoryInfo()
        {
            //获取需要升级的用户
            var chatMemberQuery = from cm in _eFContext.ChatMember
                                  join r in _eFContext.RegisterHealthCardInfo on cm.MemId equals r.MemId
                                  where cm.MemCheckCard == ""
                                  select new
                                  {
                                      cm.MemId,
                                      cm.MemCard,
                                      cm.MemName,
                                      cm.Sex,
                                      cm.MemTel,
                                      cm.MedAddress,
                                      cm.MemCheckCard,
                                      r.HealthCardId
                                  };
            var list = chatMemberQuery.ToList();
            Console.WriteLine($"即将升级{list.Count}条数据");
            if (list.Any())
            {
                foreach (var item in list)
                {
                    try
                    {

                        var hisRes = HISQueryOrRegister(new HISQueryOrRegisterRequestDomainModel
                        {
                            IdNumber = item.MemCard,
                            Address = item.MedAddress,
                            Gender = item.Sex == 0 ? "1" : "2",
                            Name = item.MemName,
                            Phone1 = item.MemTel
                        });
                        if (hisRes.Code == 0)
                        {
                            HisBind(hisRes.Data, item.HealthCardId, "FixHistoryInfo");
                        }
                        if (string.IsNullOrEmpty(item.MemCheckCard))
                        {
                            var info = _eFContext.ChatMember.Find(item.MemId);
                            if (info != null)
                            {
                                info.MemCheckCard = hisRes.Data;
                                _eFContext.Update(info);
                                _eFContext.SaveChanges();
                            }
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item);
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// 获取卡包订单ID接口
        /// </summary>
        /// <returns></returns>
        public GetOrderIdByOutAppIdRsp GetOrderIdByOutAppId(GetOrderIdByOutAppIdReq req)
        {
            var appToken = HealthCardHelper.GetAppToken();
            var res = HealthCardHelper.GetOrderIdByOutAppId(req, appToken);
            return res;
        }
        /// <summary>
        /// 添加失败的信息
        /// </summary>
        /// <param name="req"></param>
        public void RegisterHealthCardErrorLog(RegisterHealthCardErrorLog req)
        {
            try
            {
                req.Id = _IdWorker.NextId();
                req.CreateTime = DateTime.Now;
                _eFContext.Add(req);
                _eFContext.SaveChanges();
            }
            catch (Exception ex)
            {
                NLogHelper.ErrorLog(ex.Message, ex);
            }
        }

        /// <summary>
        /// 错误信息查询
        /// </summary>
        /// <returns></returns>
        public IQueryable<RegisterHealthCardErrorLog> RegisterHealthCardErrorLogQuery(RegisterHealthCardErrorLogQueryRequestDomainModel req)
        {
            var query = _eFContext.RegisterHealthCardErrorLog.AsQueryable();
            query = query.WhereIF(!string.IsNullOrEmpty(req.Info), x => x.Info.Contains(req.Info));
            query = query.WhereIF(!string.IsNullOrEmpty(req.Data), x => x.Data.Contains(req.Data));
            return query;
        }
        /// <summary>
        /// 错误信息分页查询
        /// </summary>
        /// <returns></returns>
        public QueryPageResponse<RegisterHealthCardErrorLog> RegisterHealthCardErrorLogQueryPage(QueryPageRequest<RegisterHealthCardErrorLogQueryRequestDomainModel> req)
        {
            var param = req.condition;
            var query = RegisterHealthCardErrorLogQuery(param);
            //排序
            var data = query.OrderByDescending(x => x.CreateTime).Skip(req.paging._mySqlLimitStart).Take(req.paging._mySqlLimitRows).ToList();
            var res = new QueryPageResponse<RegisterHealthCardErrorLog>(data, query.Count(), req.paging.pageIndex, req.paging.pageSize);
            return res;
        }
        /// <summary>
        /// 健康卡查询
        /// </summary>
        /// <returns></returns>
        public IQueryable<HealthCardInfoQueryResponse> HealthCardInfoQuery(HealthCardInfoQueryRequestDomainModel req)
        {
            var query = from h in _eFContext.RegisterHealthCardInfo
                        join cm in _eFContext.ChatMember on h.MemId equals cm.MemId
                        select new HealthCardInfoQueryResponse
                        {
                            CreateTime = h.CreateTime,
                            HealthCardId = h.HealthCardId,
                            QrcodeText = h.QrcodeText,
                            Status = h.Status,
                            Openid = h.Openid,
                            MemName = cm.MemName,
                            MemCard = cm.MemCard,
                            MemCheckCard = cm.MemCheckCard,
                            IsUse = (int)cm.IsUse,
                        }
                        ;
            query = query.WhereIF(!string.IsNullOrEmpty(req.MemCard), x => x.MemCard.Contains(req.MemCard));
            query = query.WhereIF(!string.IsNullOrEmpty(req.MemCheckCard), x => x.MemCheckCard.Contains(req.MemCheckCard));
            query = query.WhereIF(!string.IsNullOrEmpty(req.MemName), x => x.MemName.Contains(req.MemName));
            return query;
        }
        /// <summary>
        /// 健康卡查询
        /// </summary>
        /// <returns></returns>
        public QueryPageResponse<HealthCardInfoQueryResponse> HealthCardInfoQueryPage(QueryPageRequest<HealthCardInfoQueryRequestDomainModel> req)
        {
            var param = req.condition;
            var query = HealthCardInfoQuery(param);
            //排序
            var data = query.OrderByDescending(x => x.CreateTime).Skip(req.paging._mySqlLimitStart).Take(req.paging._mySqlLimitRows).ToList();
            var res = new QueryPageResponse<HealthCardInfoQueryResponse>(data, query.Count(), req.paging.pageIndex, req.paging.pageSize);
            return res;
        }
        /// <summary>
        /// 动态健康卡二维码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public GetDynamicQRCodeRsp GetDynamicQRCode(GetDynamicQRCodeReq req)
        {
            var appToken = HealthCardHelper.GetAppToken();
            var res = HealthCardHelper.GetDynamicQRCode(req, appToken);
            if (res.Code != 0)
                RegisterHealthCardErrorLog(new RegisterHealthCardErrorLog { From = "GetDynamicQRCode", Data = $"{JsonConvert.SerializeObject(req)}", Info = $"获取动态健康卡二维码错误:{res.Code};{res.Msg}" });
            return res.Data;
        }
        /// <summary>
        /// 绑定修复
        /// </summary>
        public void FixBind()
        {
            var list = HealthCardInfoQuery(new HealthCardInfoQueryRequestDomainModel { MemCard = "", MemCheckCard = "", MemName = "" }).Where(x => x.Status == 1 && x.IsUse == 1);
            foreach (var item in list)
            {
                HisBind(item.MemCheckCard, item.HealthCardId, "FixBind");
            }
        }
    }
}
