namespace Core.Extensions.AuthExtension
{
    public class AuthorityService : IAuthorityService
    {
        private readonly EFContext _mallContext;
        //private readonly AuthCenterContext _authCenterContext;
        private readonly WorkContext _workContext;
        public AuthorityService(
            EFContext mallContext,
            //AuthCenterContext authCenterContext,
            WorkContext workContext
            )
        {
            _mallContext = mallContext;
            //_authCenterContext = authCenterContext;
            _workContext = workContext;
        }
        ///// <summary>
        ///// 验证是否有权限
        ///// </summary>
        ///// <param name="method">请求方式</param>
        ///// <param name="rule">接口地址</param>
        ///// <param name="userId">用户Id</param>
        ///// <returns></returns>
        //public int GetAuth(string method, string rule, long userId)
        //{
        //    //验证是否有这个接口
        //    var authkey = _mallContext.Auth.Where(a =>
        //          _mallContext.AuthInterface.Where(ai =>
        //              _mallContext.InterfaceInfo.Where(ii => ii.Interface.ToLower() == rule && ii.Method == method
        //              ).Any(ii => ii.Id == ai.InterfaceId)
        //          ).Any(ai => ai.AuthId == a.Id)).Select(x => x.AuthKey).ToList();
        //    if (authkey.Count > 0)
        //    {
        //        //验证用户是否有权限
        //        var keyList = (from ur in _mallContext.UserRole
        //                       join r in _mallContext.Role on ur.RoleId equals r.Id
        //                       join ra in _mallContext.RoleAuth on r.Id equals ra.RoleId
        //                       join a in _mallContext.Auth on ra.AuthId equals a.Id
        //                       where ur.UserId == userId && a.Status == 1 && r.Status == 1
        //                       select a.AuthKey).ToList();
        //        var check = _authCenterContext.AuthCenterSystemsAdmin.Where(x => x.ClientId == ConfigurationManager.AppSettings["ClientId"] && x.UserInfoId == userId && x.OrganizationsCode == _workContext.CurrentUser.OrganizationCode).Any();
        //        if (check)
        //        {
        //            var addAuthkey = _mallContext.Auth.Where(x => x.Type == 0).Select(x => x.AuthKey).ToList();
        //            keyList.AddRange(addAuthkey);
        //        }
        //        if (keyList.Any(x => authkey.Any(p => p == x)))
        //        {
        //            return 1;
        //        }
        //    }
        //    return -1;
        //}
        ///// <summary>
        ///// 获取用户信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public AuthCenterUserInfo GetUserInfo(long id)
        //{
        //    return _authCenterContext.AuthCenterUserInfo.FirstOrDefault(x => x.Id == id);
        //}

        //ACCESSToken换取用户信息
    }
}
