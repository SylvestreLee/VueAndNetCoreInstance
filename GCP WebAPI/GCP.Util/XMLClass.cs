using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Util
{
    public class XMLClass
    {
        #region 取得返回值中的参数

        /// <summary>
        /// 取得返回值中的参数
        /// </summary>
        /// <param name="xml">xml格式文件</param>
        /// <param name="NodeName">节点名称</param>
        public static string GetXmlNodeValue(string xml, string NodeName)
        {
            int m, n;
            string sValue = "";
            string xml0 = xml;
            //转换成大写
            xml = xml.ToUpper();
            NodeName = NodeName.ToUpper();
            n = xml.IndexOf("<" + NodeName + ">");
            //找到字符串的开始符"<"和结束符"/>"，再取出其值
            if (n >= 0)
            {
                for (int i = n + NodeName.Length + 1; i < xml.Length; i++)
                {
                    string c = xml.Substring(i, 1);
                    if (c == ">")
                    {
                        m = xml.IndexOf("</" + NodeName + ">", i + 1);
                        if (m >= 0)
                        {
                            sValue = xml0.Substring(i + 1, m - i - 1);
                            break;
                        }

                    }
                    else if (c == "/")
                    {
                        sValue = "";
                        break;
                    }
                    else if (c == " ")
                    {

                    }
                    else
                    {
                        //不正常的结束符号
                        sValue = "";
                        break;
                    }
                }
            }
            //除去空格
            return sValue.Trim();
        }

        #endregion
    }
}
