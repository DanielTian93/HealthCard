namespace Core.Extensions.DI
{
    public interface IDependency
    {

    }
    /// <summary>
    /// 标记一个类型为依赖项（使用此接口的依赖项生命周期为 singleton）。
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }

    /// <summary>
    /// 标记一个类型为依赖项（使用此接口的依赖项生命周期为 per usage）。 
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }
}
