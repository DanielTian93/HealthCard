namespace Core.Extensions
{
    public class MPInfoService : IMPService
    {
        private readonly EFContext _mallContext;

        public MPInfoService(
            EFContext mallContext
            )
        {
            _mallContext = mallContext;
        }
        /// <summary>
        /// 微信信息查询
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        //public MpInfo GetMpInfo(string app)
        //{
        //    return _mallContext.MpInfo.FirstOrDefault(x => x.Mark == app);
        //}
        ///// <summary>
        ///// 阿里小程序信息
        ///// </summary>
        ///// <param name="aliapp"></param>
        ///// <returns></returns>
        //public AliMpInfo GetAliMpInfo(string aliapp)
        //{
        //    return _mallContext.AliMpInfo.Where(x => x.Mark == aliapp).FirstOrDefault();
        //}
    }
}
