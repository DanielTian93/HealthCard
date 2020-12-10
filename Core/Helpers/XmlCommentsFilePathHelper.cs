using Microsoft.DotNet.PlatformAbstractions;
using System.Collections.Generic;
using System.IO;

namespace Core.Helpers
{


    public static class XmlCommentsFilePathHelper
    {
        /// <summary>
        /// 获取xml
        /// </summary>
        public static List<string> XmlCommentsFilePath()
        {
            List<string> XmlComments = new List<string>();

            var basePath = ApplicationEnvironment.ApplicationBasePath;
            var files = Directory.GetFiles(basePath, "*.xml");
            foreach (var file in files)
            {
                //var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                var fileName = file;
                XmlComments.Add(Path.Combine(basePath, fileName));
            }
            return XmlComments;
        }
    }
}
