using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Extensions.DI
{
    public static class InterfaceDIExtension
    {

        public static IServiceCollection RegisterInterface(this IServiceCollection services)
        {
            //1.得到当前程序的执行路径
            //dynamic type = (new Program()).GetType();
            //string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);

            //2.获取程序域中可执行文件
            var entry = Assembly.GetEntryAssembly();
            //返回执行路径里的目录
            var path = Path.GetDirectoryName(entry.Location);

            DirectoryInfo dir = new DirectoryInfo(path);
            //DirectoryInfo.GetFiles 方法 ()  给定的 searchPattern 匹配并返回当前目录的文件列表。
            //SearchOption.AllDirectories	在搜索操作中包括当前目录及其所有的子目录 SearchOption.TopDirectoryOnly	在搜索操作中包括仅当前目录。
            //查询以.exe或者以dll结尾的程序集
            var searchFiles = dir.GetFiles("*Service*", SearchOption.TopDirectoryOnly).
                                            Where(f => f.Name.EndsWith(".exe") || f.Name.EndsWith(".dll"));

            var TypeList = searchFiles.SelectMany(file => getAssemblyofTypes(loadAssembly(file))).ToList();
            //类型判断
            TypeList = TypeList.Where(a => a.IsNonAbstractClass(false)).ToList();
            return RegisterDecide(services, TypeList);
        }
        public static IServiceCollection RegisterDecide(IServiceCollection services, List<Type> TypeList)
        {
            //循环程序集里查询出来的实例
            foreach (var type in TypeList)
            {
                //得到实例

                var typeInfo = type.GetTypeInfo();
                //检索实现了IDependency接口的实例
                var interfaceTypes = typeInfo.ImplementedInterfaces.Where(instance => typeof(IDependency).GetTypeInfo().IsAssignableFrom(instance)).ToList();

                if (interfaceTypes.Count == 0)
                {
                    continue;
                }

                var lifeTime = ServiceLifetime.Scoped;
                lifeTime = interfaceTypes.Exists(a => a == typeof(ISingletonDependency)) ? ServiceLifetime.Singleton : lifeTime;
                lifeTime = interfaceTypes.Exists(a => a == typeof(ITransientDependency)) ? ServiceLifetime.Transient : lifeTime;
                foreach (var interfaceType in interfaceTypes)
                {
                    if (typeInfo.Namespace.Equals(interfaceType.Namespace) && interfaceType.IsAssignableFrom(typeInfo))
                    {
                        //判断是否注册过改实例  (同一个接口可注册多个实例,一个实例才能注册一次)
                        var registerType = services.FirstOrDefault(t => t.ImplementationType == type);
                        if (registerType == null)
                        {
                            var descriptor = new ServiceDescriptor(interfaceType, type, lifeTime);
                            services.Add(descriptor);
                        }
                    }
                    else
                    {
                        Console.WriteLine(interfaceType.Namespace);
                        Console.WriteLine(typeInfo);
                    }
                }
            }
            return services;
        }
        //根据文件路径得到程序集实例
        private static Assembly loadAssembly(FileInfo fileInfo)
        {
            try
            {
                return AppDomain.CurrentDomain.Load(
                       AssemblyName.GetAssemblyName(fileInfo.FullName)
                   );
            }
            catch { }
            return null;
        }
        //得到程序集里面的全部类型
        private static Type[] getAssemblyofTypes(Assembly assembly)
        {
            Type[] ret = new Type[0];
            try
            {
                if (assembly != null)
                {
                    //返回程序集里面的全部类型
                    ret = assembly?.GetTypes();
                }
            }
            catch { }

            return ret;
        }
    }
}
