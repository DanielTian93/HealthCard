using Core;
using Core.Enum;
using Core.Extensions;
using Core.Extentions;
using Core.Filter;
using Core.Helpers;
using Core.Models.DBModels;
using Core.UnifiedResponseMessage;
using HealthCardServices.DomainModel;
using HYSoft.Wechat.HealthCardServices.Services;
using Microsoft.AspNetCore.Mvc;
using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebService9090;
using static Core.UnifiedResponseMessage.ResponseObjectExtention;

namespace HealthCardApi.Controllers
{
    public class QueryController : ControllerBase
    {
        private readonly IHealthCardService _healthCardService;
        private readonly IdWorker _IdWorker;
        private readonly EFContext _eFContext;
        public QueryController(IHealthCardService healthCardService, EFContext eFContext)
        {
            _IdWorker = SnowflakeHelper.IdWorker;
            _healthCardService = healthCardService;
            _eFContext = eFContext;
        }
        
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [NoToken]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseObject<LoginResponseDomainModel>), (int)HttpStatusCode.OK)]
        public object Login([FromBody] LoginRequestDomainModel req)
        {
            var userName = ConfigExtensions.Configuration["UserInfo:UserName"];
            var userPassword = ConfigExtensions.Configuration["UserInfo:UserPassword"];
            if (req.UserName == userName && req.Password == userPassword)
            {
                var token = Guid.NewGuid().ToString("N");
                RedisHelper.Set(token, new UserSession { LoginTime = DateTime.Now }, TimeSpan.FromDays(1), slide: true);
                return this.Success(new LoginResponseDomainModel { Token = token });
            }
            return this.Error(ErrCodeCommon.LoginError);
        }

        /// <summary>
        /// 错误信息分页查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("RegisterHealthCardErrorLogQueryPage")]
        [Login]
        [SwaggerToken]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseObject<QueryPageResponse<RegisterHealthCardErrorLog>>), (int)HttpStatusCode.OK)]
        public object RegisterHealthCardErrorLogQueryPage([FromBody] QueryPageRequest<RegisterHealthCardErrorLogQueryRequestDomainModel> req)
        {
            var res = _healthCardService.RegisterHealthCardErrorLogQueryPage(req);
            return this.Success(res);
        }
        /// <summary>
        /// 健康卡分页查询列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("HealthCardInfoQueryPage")]
        [Login]
        [SwaggerToken]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseObject<QueryPageResponse<QueryPageResponse<HealthCardInfoQueryResponse>>>), (int)HttpStatusCode.OK)]
        public object HealthCardInfoQueryPage([FromBody] QueryPageRequest<HealthCardInfoQueryRequestDomainModel> req)
        {
            var res = _healthCardService.HealthCardInfoQueryPage(req);
            return this.Success(res);
        }
    }
}
