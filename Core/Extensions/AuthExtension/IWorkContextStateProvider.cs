using System;

namespace Core.Extensions.AuthExtension
{
    /// <summary>
    /// 实现此接口存储工作上下文中的状态。
    /// </summary>
    public interface IWorkContextStateProvider
    {
        /// <summary>
        /// 根据给定上下文获取存储对象。
        /// </summary>
        /// <param name="name">状态名称。</param>
        /// <returns>一个状态对象构造器。</returns>
        Func<WorkContext, object> Get(string name);
    }
}
