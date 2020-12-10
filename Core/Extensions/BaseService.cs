using AutoMapper;
using Core.Extensions.AuthExtension;
using Core.Helpers;
using Microsoft.AspNetCore.Hosting;
using Snowflake.Core;
using System;

namespace Core.Extensions
{
    public class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IdWorker _idWorker;
        protected readonly EFContext _mallContext;
        protected readonly IHostingEnvironment _hostingEnv;
        protected readonly WorkContext _workContext;
        protected readonly DateTime now = DateTime.Now;
        //protected readonly AuthCenterContext _authCenterContext;
        public BaseService(
            IMapper mapper,
            IHostingEnvironment hostingEnv,
            EFContext mallContext,
            //AuthCenterContext authCenterContext,
            WorkContext workContext
            )
        {
            _mapper = mapper;
            _idWorker = SnowflakeHelper.IdWorker;
            _hostingEnv = hostingEnv;
            _mallContext = mallContext;
            //_authCenterContext = authCenterContext;
            _workContext = workContext;
        }
    }
}
