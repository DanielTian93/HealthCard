using com.tencent.healthcard.HealthCardOpenAPI;
using Core.Extensions.DI;
using Core.Models.DBModels;
using Core.UnifiedResponseMessage;
using HealthCardServices.DomainModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Core.Helpers.HISHelper;
using static Core.UnifiedResponseMessage.ResponseObjectExtention;

namespace HYSoft.Wechat.HealthCardServices.Services
{
    public interface IHealthCardService : IDependency
    {
        /// <summary>
        /// His查询就诊卡
        /// </summary>
        /// <param name="Idnumber"></param>
        /// <returns></returns>
        ResponseObjectDomainModel<HISJZKQueryResponseDomainModel> HISJZKQuery(string idnumber);
        /// <summary>
        /// 身份证OCR
        /// </summary>
        /// <param name="imageContent"></param>
        /// <returns></returns>
        OcrInfoRsp IDCardOCR(string imageContent);
        /// <summary>
        /// 注册健康卡
        /// </summary>
        /// <param name="req"></param>
        Task<ResponseObjectDomainModel<bool>> RegisterHealthCardAsync(RegisterHealthCardRequestDomainModel req);

        /// <summary>
        /// 批量注册健康卡 老用户升级
        /// </summary>
        /// <returns></returns>
        Task RegisterBatchHealthCardAsync();
        /// <summary>
        /// 查询已注册健康卡数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        IQueryable<HealthCardQueryResponseDomainModel> HealthCardQuery(HealthCardQueryRequestDomainModel req);
        /// <summary>
        /// 用卡数据监测接口
        /// </summary>
        /// <param name="reportHISDataReq"></param>
        /// <returns></returns>
        void ReportHISData(ReportHISDataReq reportHISDataReq);
        /// <summary>
        /// 用卡数据监测接口 提供MemId
        /// </summary>
        /// <param name="req"></param>
        void ReportHisDataMem(ReportHISDataReuestDomainModel req);
        /// <summary>
        /// 通过健康卡授权码获取健康卡接口
        /// </summary>
        /// <returns></returns>
        GetHealthCardByHealthCodeRsp GetHealthCardByHealthCode(GetHealthCardByHealthCodeReq req);
  
        /// <summary>
        /// 查询 就诊卡 以及 健康卡信息（如果没有健康卡 健康卡信息为空）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        List<AllCardQueryResponseDomainModel> AllCardQuery(AllCardQueryRequestDomainModel req);
        /// <summary>
        /// 老卡升级健康卡
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        ResponseObjectDomainModel<bool> OldCardRegisterHealthCard(OldCardRegisterHealthCardRequestDomainModel req);
        /// <summary>
        /// 获取卡包订单ID接口
        /// </summary>
        /// <returns></returns>
        GetOrderIdByOutAppIdRsp GetOrderIdByOutAppId(GetOrderIdByOutAppIdReq req);
        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="memid"></param>
        /// <returns></returns>
        bool UnBindlingHealthCard(int memid, string openId);
        
        /// <summary>
        /// 升级已有健康卡的历史数据 绑定关系
        /// </summary>
        void FixHistoryInfo();
        /// <summary>
        /// 绑定修复
        /// </summary>
        void FixBind();
        /// <summary>
        /// 封装向His注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        ResponseObjectDomainModel<string> HISQueryOrRegister(HISQueryOrRegisterRequestDomainModel req);
      
        /// <summary>
        /// 向His提交绑定
        /// </summary>
        /// <returns></returns>
        void HisBind(string patientNO, string healthCardId, string from);
        /// <summary>
        /// 添加失败的信息
        /// </summary>
        /// <param name="req"></param>
        void RegisterHealthCardErrorLog(RegisterHealthCardErrorLog req);
        /// <summary>
        /// 错误信息分页查询
        /// </summary>
        /// <returns></returns>
        QueryPageResponse<RegisterHealthCardErrorLog> RegisterHealthCardErrorLogQueryPage(QueryPageRequest<RegisterHealthCardErrorLogQueryRequestDomainModel> req);
        /// <summary>
        /// 健康卡分页查询列表
        /// </summary>
        /// <returns></returns>
        QueryPageResponse<HealthCardInfoQueryResponse> HealthCardInfoQueryPage(QueryPageRequest<HealthCardInfoQueryRequestDomainModel> req);
        /// <summary>
        /// 动态健康卡二维码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        GetDynamicQRCodeRsp GetDynamicQRCode(GetDynamicQRCodeReq req);
        /// <summary>
        /// 更换绑定信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool HISBind(HISBindDomainModel req, string from = "");
    }
}