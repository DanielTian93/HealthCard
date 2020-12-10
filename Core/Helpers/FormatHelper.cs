using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Core.Helpers
{
    public static class FormatHelper
    {
        /// <summary>
        /// XML TO SortedDictionary
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static SortedDictionary<string, object> XMLToSortedDictionary(string xml)
        {
            var data = new SortedDictionary<string, object>();
            if (string.IsNullOrEmpty(xml))
            {
                throw new Exception("将空的xml串转换为WxData不合法!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                data[xe.Name] = xe.InnerText;//获取xml的键值对到WxData内部的数据中
            }
            return data;
        }
        /// <summary>
        /// JsonToXML
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static StringWriter JsonWriteToXML(string json)
        {
            XmlDocument xml = JsonConvert.DeserializeXmlNode(json);
            StringWriter res = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(res);
            xml.WriteTo(xmlTextWriter);
            return res;
        }
        /// <summary>
        /// ByteTo16进制数据
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string Byte2HexStr(byte[] b)
        {
            string stmp = "";
            StringBuilder sb = new StringBuilder("");
            for (int n = 0; n < b.Length; n++)
            {
                stmp = b[n].ToString("X");
                sb.Append((stmp.Length == 1) ? "0" + stmp : stmp);
                sb.Append(" ");
            }
            return sb.ToString().ToUpper().Trim();
        }
        public static string Byte2Str(byte[] b)
        {
            StringBuilder res = new StringBuilder("");
            for (int n = 0; n < b.Length; n++)
            {
                res.Append(b[n].ToString());
            }
            return res.ToString();
        }
        public static byte[] StrToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }

            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }
        public static byte[] GetByteArray(string shex)
        {
            string[] ssArray = shex.Split(' ');
            List<byte> bytList = new List<byte>();
            foreach (var s in ssArray)
            {                //将十六进制的字符串转换成数值  
                bytList.Add(Convert.ToByte(s, 16));
            }    //返回字节数组          
            return bytList.ToArray();
        }
        //public static String hexStr2Str(String hexStr)
        //{
        //    String str = "0123456789ABCDEF";
        //    char[] hexs = hexStr.ToCharArray();
        //    byte[] bytes = new byte[hexStr.Length / 2];
        //    int n;

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        n = str.IndexOf(hexs[2 * i]) * 16;
        //        n += str.IndexOf(hexs[2 * i + 1]);
        //        bytes[i] = (byte)(n & 0xff);
        //    }
        //    return Encoding.UTF8.GetString(bytes);
        //}

        public static byte[] IntToBytes(int value)
        {
            byte[] src = new byte[4];
            src[3] = (byte)((value >> 24) & 0xFF);
            src[2] = (byte)((value >> 16) & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);//高8位
            src[0] = (byte)(value & 0xFF);//低位
            return src;
        }
        public static byte[] HexStringToByteArray(string s)
        {
            if (s.Length == 0)
            {
                throw new Exception("将16进制字符串转换成字节数组时出错，错误信息：被转换的字符串长度为0。");
            }

            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }

            return buffer;
        }

        /// <summary>
        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string String2Unicode(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        ///// <summary>
        ///// Unicode转字符串
        ///// </summary>
        ///// <param name="source">经过Unicode编码的字符串</param>
        ///// <returns>正常字符串</returns>
        //public static string Unicode2String(string source)
        //{
        //    return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
        //                 source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        //}
        /// <summary>
        /// //
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string deUnicode(string content)
        {//每4位16进制Unicode编码转为一个字符
            string enUnicode = null;
            string deUnicode = null;
            for (int i = 0; i < content.Length; i++)
            {

                enUnicode += content[i];

                if (i % 4 == 3)
                {

                    deUnicode += (char)(Convert.ToInt32(enUnicode, 16));


                    enUnicode = null;
                }

            }
            return deUnicode;
        }
        /// <summary>
        /// 16进制转ASCCI对照表字符
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static string HexASCIIToSting(string hexStr)
        {
            hexStr = hexStr.Replace(" ", "");
            //test03
            byte[] tmpary = new byte[hexStr.Length / 2];
            int index = 0;
            for (int i = 0; i < hexStr.Length; i += 2)
            {
                tmpary[index] = Convert.ToByte(hexStr.Substring(i, 2), 16);
                index++;
            }
            return Encoding.ASCII.GetString(tmpary);
        }

        public static byte[] FCReceived(byte[] b)
        {
            var hex = Byte2HexStr(b).Replace("7F 01", "7E").Replace("7F 02", "7F");
            return HexStringToByteArray(hex);
        }
        public static byte[] FCSend(byte[] b)
        {
            var res = new List<byte>();
            var list = b.ToList();
            res.Add(list[0]);
            var newList = list.Skip(1).Take(list.Count - 2);
            var hex = Byte2HexStr(newList.ToArray());
            hex = hex.Replace("7F", "7F 02").Replace("7E", "7F 01");
            res.AddRange(HexStringToByteArray(hex).ToList());
            res.Add(list[list.Count() - 1]);
            return res.ToArray();
        }
    }
}
