using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection;

/// <summary>
/// Xml序列化与反序列化
/// </summary>
public class XmlUtil
{
    #region 反序列化
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="xml">XML字符串</param>
    /// <returns></returns>
    public static object Deserialize(Type type, string xml)
    {
        try
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr);
            }
        }
        catch (Exception e)
        {

            return null;
        }
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="type"></param>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static object Deserialize(Type type, Stream stream)
    {
        XmlSerializer xmldes = new XmlSerializer(type);
        return xmldes.Deserialize(stream);
    }
    #endregion

    #region 序列化
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="obj">对象</param>
    /// <returns></returns>
    public static string Serializer(Type type, object obj)
    {
        MemoryStream Stream = new MemoryStream();
        XmlSerializer xml = new XmlSerializer(type);
        try
        {
            //序列化对象
            xml.Serialize(Stream, obj);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        Stream.Position = 0;
        StreamReader sr = new StreamReader(Stream);
        string str = sr.ReadToEnd();

        sr.Dispose();
        Stream.Dispose();

        return str;
    }

    #endregion
    /// <summary>

    ///  把XML字符串转换为LIST用于存储数据

    /// </summary>

    /// <typeparam name="T">任意实体类</typeparam>

    /// <param name="XML">XML字符串</param>

    /// <param name="tableName">表名</param>

    /// <returns></returns>
    public static List<T> ConvertXMLToList<T>(string XML, string tableName) where T : new()

    {

        //利用反射获得实体类的属性
        List<T> list = new List<T>();



        XML = XML.Replace("NewDataSet", "data");

        XDocument output = XDocument.Parse(XML);

        XElement root = output.Root;

        foreach (XElement elem in root.Elements(tableName))

        {

            T RowInstance = Activator.CreateInstance<T>();

            foreach (PropertyInfo Property in typeof(T).GetProperties())

            {

                if (elem.Element(Property.Name) != null)

                {

                    Property.SetValue(RowInstance, Convert.ChangeType(elem.Element(Property.Name).Value.Trim(), Property.PropertyType), null);

                }

            }

            list.Add(RowInstance);

        }

        return list;

    }
    /// <summary>
    /// XML文件字符串转化实体类
    /// </summary>
    /// <typeparam name="T">泛型类</typeparam>
    /// <param name="filename">XML字符串</param>
    /// <returns></returns>
    public static T DeserializeObject<T>(string filename)
    {

        // 创建指定类型和命名空间的XmlSerializer实例.
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        // 读取XML文档需要文件流。
        // FileStream fs = new FileStream(filename, FileMode.Open);
        //StreamReader str = new StreamReader(filename);
        StringReader str = new StringReader(filename);
        XmlReader reader = XmlReader.Create(str);

        
        T i;

        // Use the Deserialize method to restore the object's state.
        i = (T)serializer.Deserialize(reader);
        //fs.Close();
        return i;
        // Write out the properties of the object.
    }
}