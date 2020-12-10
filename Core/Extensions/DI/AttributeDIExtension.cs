using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Extensions.DI
{
    public static class AttributeDIExtension
    {

        public static IServiceCollection RegisterAttribute(this IServiceCollection services)
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
                //检索有ServiceDescriptorAttribute特性的类型
                var attributes = typeInfo.GetCustomAttributes<ServiceDescriptorAttribute>().ToArray();
                foreach (var attribute in attributes)
                {
                    var serviceTypes = GetServiceTypes(type, attribute);
                    foreach (var serviceType in serviceTypes)
                    {
                        //判断是否注册过改实例  (同一个接口可注册多个实例,一个实例才能注册一次)
                        var registerType = services.FirstOrDefault(t => t.ImplementationType == type);
                        if (registerType == null)
                        {
                            //(接口,实现,生命周期)
                            var descriptor = new ServiceDescriptor(serviceType, type, attribute.Lifetime);
                            services.Add(descriptor);
                        }
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
        /// <summary>
        /// 对程序集里的类型进行筛选
        /// </summary>
        /// <param name="type"></param>
        /// <param name="publicOnly"></param>
        /// <returns></returns>
        public static bool IsNonAbstractClass(this Type type, bool publicOnly)
        {
            var typeInfo = type.GetTypeInfo();
            //判断类型是否是一个类以及该类型不能是抽象类  
            if (typeInfo.IsClass && !typeInfo.IsAbstract)
            {
                //判断该类型是否是泛型  ContainsGenericParameters=true 对象本身是泛型类型形参或者具有尚未提供特定类型的类型形参
                if (typeInfo.IsGenericType && typeInfo.ContainsGenericParameters)
                {
                    return false;
                }

                if (publicOnly)
                {
                    return typeInfo.IsPublic || typeInfo.IsNestedPublic;
                }

                return true;
            }

            return false;
        }
        /// <summary>
        ///判断接口和实例是否一致能够分配
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetServiceTypes(Type type, ServiceDescriptorAttribute attribute)
        {
            //实现实例
            var typeInfo = type.GetTypeInfo();
            //接口实例
            var serviceType = attribute.ServiceType;

            if (serviceType == null)
            {
                yield return type;

                foreach (var implementedInterface in typeInfo.ImplementedInterfaces)
                {
                    yield return implementedInterface;
                }

                if (typeInfo.BaseType != null && typeInfo.BaseType != typeof(object))
                {
                    yield return typeInfo.BaseType;
                }
                //yield 定义迭代器
                yield break;
            }
            //接口实例
            var serviceTypeInfo = serviceType.GetTypeInfo();

            //确定指定类型的实例是否可以分配给当前类型
            if (!serviceTypeInfo.IsAssignableFrom(typeInfo))
            {
                throw new InvalidOperationException($@"Type ""{typeInfo.FullName}"" is not assignable to ""${serviceTypeInfo.FullName}"".");
            }

            yield return serviceType;
        }
    }
}
