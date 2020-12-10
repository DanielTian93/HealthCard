using Core.Extensions.DI;

namespace Core.Extensions
{
    public interface IMPService : ITransientDependency
    {
        ///// <summary>
        ///// 微信信息查询
        ///// </summary>
        ///// <param name="app"></param>
        ///// <returns></returns>
        //MpInfo GetMpInfo(string app);
        ///// <summary>
        ///// 阿里小程序信息
        ///// </summary>
        ///// <param name="aliapp"></param>
        ///// <returns></returns>
        //AliMpInfo GetAliMpInfo(string aliapp);
    }
}