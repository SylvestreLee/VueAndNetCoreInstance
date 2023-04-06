using GCP.Business.Cache;
using GCP.Entity.SystemManage;
using GCP.Model.Result;
using GCP.Service;
using GCP.Util;
using GCP.Util.Extension;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static GCP.Model.CommonModel;

namespace GCP.Business
{
    public class CommonBLL
    {
        private CommonService commonService = new CommonService();

        #region 关联枚举

        #region 根据字符串表名关联枚举

        /// <summary>
        /// 根据枚举值设置枚举表值
        /// </summary>
        /// <typeparam name="T">主表实体类泛型</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="Entitys">主表实体类列表</param>
        /// <param name="Prop">主表枚举字段</param>
        /// <param name="PropEnumName">主表枚举值返回字段</param>
        /// <param name="EnumTableName">枚举表名称</param>
        /// <param name="SqlWhere">枚举表查询条件</param>
        /// <param name="EnumIDColumnName">枚举表ID字段别名</param>
        /// <param name="EnumNameColumnName">枚举表Name字段别名</param>
        /// <returns></returns>
        public async Task<List<T>> SetJoinTable<T, T1, T2>(List<T> Entitys, Expression<Func<T, T1>> Prop, Expression<Func<T, T2>> PropEnumName, string EnumTableName, string SqlWhere = "", string EnumIDColumnName = "ID", string EnumNameColumnName = "Name")
        {
            try
            {
                Type type = typeof(T);
                PropertyInfo[] properties = ReflectionHelper.GetProperties(type);

                List<string> ListInValue = new List<string>();
                StringBuilder strSql = new StringBuilder(" " + EnumIDColumnName + " in (");

                string strPropEnumName = GetPropertyName(PropEnumName);
                string strProp = GetPropertyName(Prop);

                //获取列名对应的PropertyInfo
                var pProp = properties.Where(p => p.Name.ToUpper() == strProp.ToUpper()).Single();
                var pPropEnumName = properties.Where(p => p.Name.ToUpper() == strPropEnumName.ToUpper()).Single();

                //根据join的ID获取where in查询条件
                for (int columnIndex = 0; columnIndex < Entitys.Count; columnIndex++)
                {
                    var v = pProp.GetValue(Entitys[columnIndex], null).ParseToString();
                    var vs = v.Split(',');
                    foreach (var thisv in vs)
                    {
                        if (!ListInValue.Contains(thisv) && thisv != string.Empty)
                        {
                            ListInValue.Add(thisv);
                            strSql.Append("'" + thisv + "',");
                        }
                    }
                }
                if (properties.Length > 0)
                {
                    strSql = strSql.Remove(strSql.Length - 1, 1);
                }
                strSql.Append(")");

                if (SqlWhere != "")
                {
                    SqlWhere = " and " + SqlWhere;
                }

                //根据缓存等查询枚举表
                List<EnumIdNameModel> list = new List<EnumIdNameModel>();
                if (EnumCache.CanGetCache(EnumTableName))
                {
                    list = await EnumCache.Table(EnumTableName, EnumIDColumnName, EnumNameColumnName).GetList();
                    list = list.Where(ex => ListInValue.Contains(ex.ID)).ToList();
                }
                else
                {
                    list = await commonService.GetEnumList(EnumTableName, strSql.ToString() + SqlWhere, EnumIDColumnName, EnumNameColumnName);
                }

                for (int columnIndex = 0; columnIndex < Entitys.Count; columnIndex++)
                {
                    switch (pPropEnumName.PropertyType.ToString())
                    {
                        case "System.String":
                            var entity = Entitys[columnIndex];
                            try
                            {
                                var value = pProp.GetValue(entity).ToString();

                                var vs = value.Split(',');
                                StringBuilder retValue = new StringBuilder();
                                foreach (var thisv in vs)
                                {
                                    retValue.Append(list.Where(p => p.ID == thisv).Select(t => t.Name).FirstOrDefault() + ",");
                                }
                                if (retValue.ToString() != string.Empty)
                                {
                                    retValue = retValue.Remove(retValue.Length - 1, 1);
                                }

                                pPropEnumName.SetValue(Entitys[columnIndex], retValue.ToString());
                            }
                            catch (Exception err)
                            { }
                            break;
                        default:

                            break;
                    }
                }
            }
            catch (Exception err)
            { }
            return Entitys;
        }

        /// <summary>
        /// 根据枚举值设置枚举表值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="Entity">主表实体类</param>
        /// <param name="Prop">主表枚举字段</param>
        /// <param name="PropEnumName">主表枚举值返回字段</param>
        /// <param name="EnumTableName">枚举表名称</param>
        /// <param name="SqlWhere">枚举表查询条件</param>
        /// <param name="EnumIDColumnName">枚举表ID字段别名</param>
        /// <param name="EnumNameColumnName">枚举表Name字段别名</param>
        /// <returns></returns>
        public async Task<T> SetJoinTable<T, T1, T2>(T Entity, Expression<Func<T, T1>> Prop, Expression<Func<T, T2>> PropEnumName, string EnumTableName, string SqlWhere = "", string EnumIDColumnName = "ID", string EnumNameColumnName = "Name")
        {
            List<T> Entitys = new List<T>();
            Entitys.Add(Entity);
            try
            {
                Entity = (await SetJoinTable(Entitys, Prop, PropEnumName, EnumTableName, SqlWhere, EnumIDColumnName, EnumNameColumnName))[0];
            }
            catch (Exception err)
            { }
            return Entity;
        }

        #endregion

        #region 根据字段表名关联枚举

        public async Task<List<T>> SetJoinTable<T, T1, T2>(List<T> Entitys, Expression<Func<T, T1>> Prop, Expression<Func<T, T2>> PropEnumName, Expression<Func<T, string>> TableName, string SqlWhere = "", string EnumIDColumnName = "ID", string EnumNameColumnName = "Name")
        {
            try
            {
                Type type = typeof(T);
                PropertyInfo[] properties = ReflectionHelper.GetProperties(type);

                Dictionary<string, List<string>> DicIdWhere = new Dictionary<string, List<string>>();
                Dictionary<string, List<EnumIdNameModel>> DicTable = new Dictionary<string, List<EnumIdNameModel>>();

                string strPropEnumName = GetPropertyName(PropEnumName);
                string strProp = GetPropertyName(Prop);
                string strTableName = GetPropertyName(TableName);

                //获取列名对应的PropertyInfo
                var pProp = properties.Where(p => p.Name.ToUpper() == strProp.ToUpper()).Single();
                var pPropEnumName = properties.Where(p => p.Name.ToUpper() == strPropEnumName.ToUpper()).Single();
                var pTableName = properties.Where(p => p.Name.ToUpper() == strTableName.ToUpper()).Single();

                //获取所有table
                foreach (var entity in Entitys)
                {
                    try
                    {
                        string table = pTableName.GetValue(entity, null).ToString();
                        if (!string.IsNullOrEmpty(table))
                        {
                            string value = pProp.GetValue(entity, null).ToString();
                            if (!string.IsNullOrEmpty(value))
                            {
                                var values = value.Split(',').ToList();
                                if (!DicIdWhere.ContainsKey(table))
                                {
                                    DicIdWhere.Add(table, values);
                                }
                                else
                                {
                                    foreach (var v in values)
                                    {
                                        if (!DicIdWhere[table].Contains(v))
                                        {
                                            DicIdWhere[table].Add(v);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception err)
                    { }
                }

                //根据条件查询所有table
                foreach (var t in DicIdWhere)
                {
                    string EnumTableName = t.Key;
                    //根据缓存等查询枚举表
                    List<EnumIdNameModel> list = new List<EnumIdNameModel>();
                    if (EnumCache.CanGetCache(EnumTableName))
                    {
                        list = await EnumCache.Table(EnumTableName, EnumIDColumnName, EnumNameColumnName).GetList();
                        list = list.Where(ex => t.Value.Contains(ex.ID)).ToList();
                    }
                    else
                    {
                        //拼接查询条件in
                        StringBuilder strSql = new StringBuilder(" " + EnumIDColumnName + " in (");
                        foreach (var v in t.Value)
                        {
                            strSql.Append("'" + v + "',");
                        }
                        if (t.Value.Count > 0)
                        {
                            strSql = strSql.Remove(strSql.Length - 1, 1);
                        }
                        strSql.Append(")");
                        if (SqlWhere != "")
                        {
                            SqlWhere = " and " + SqlWhere;
                        }
                        list = await commonService.GetEnumList(EnumTableName, strSql.ToString() + SqlWhere, EnumIDColumnName, EnumNameColumnName);
                    }
                    DicTable.Add(EnumTableName, list);
                }

                for (int columnIndex = 0; columnIndex < Entitys.Count; columnIndex++)
                {
                    switch (pPropEnumName.PropertyType.ToString())
                    {
                        case "System.String":
                            var entity = Entitys[columnIndex];
                            try
                            {
                                var value = pProp.GetValue(entity).ToString();

                                var vs = value.Split(',');
                                StringBuilder retValue = new StringBuilder();
                                foreach (var thisv in vs)
                                {
                                    retValue.Append(DicTable[pTableName.GetValue(entity).ToString()].Where(p => p.ID == thisv).Select(t => t.Name).FirstOrDefault() + ",");
                                }
                                if (retValue.ToString() != string.Empty)
                                {
                                    retValue = retValue.Remove(retValue.Length - 1, 1);
                                }

                                pPropEnumName.SetValue(Entitys[columnIndex], retValue.ToString());
                            }
                            catch (Exception err)
                            { }
                            break;
                        default:

                            break;
                    }
                }
            }
            catch (Exception err)
            { }
            return Entitys;
        }

        public async Task<T> SetJoinTable<T, T1, T2>(T Entity, Expression<Func<T, T1>> Prop, Expression<Func<T, T2>> PropEnumName, Expression<Func<T, string>> TableName, string SqlWhere = "", string EnumIDColumnName = "ID", string EnumNameColumnName = "Name")
        {
            List<T> Entitys = new List<T>();
            Entitys.Add(Entity);
            try
            {
                Entity = (await SetJoinTable(Entitys, Prop, PropEnumName, TableName, SqlWhere, EnumIDColumnName, EnumNameColumnName))[0];
            }
            catch (Exception err)
            { }
            return Entity;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 根据属性获取对应的属性名称
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="expr">需要获取的属性</param>
        /// <returns>属性名</returns>
        public string GetPropertyName<T, T1>(Expression<Func<T, T1>> expr)
        {
            string propertyName = string.Empty;    //返回的属性名
                                                   //对象是不是一元运算符
            if (expr != null)
            {
                if (expr.Body is UnaryExpression)
                {
                    propertyName = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
                }
                //对象是不是访问的字段或属性
                else if (expr.Body is MemberExpression)
                {
                    propertyName = ((MemberExpression)expr.Body).Member.Name;
                }
                //对象是不是参数表达式
                else if (expr.Body is ParameterExpression)
                {
                    propertyName = ((ParameterExpression)expr.Body).Type.Name;
                }
            }
            return propertyName;
        }

        #endregion

        #endregion

        #region 下拉树打包成element树结构

        /// <summary>
        /// 传入ZtreeInfo对象，根据内容递归转成对应的ElementTreeInfo格式
        /// </summary>
        /// <param name="ztree">Ztree列表对象</param>
        /// <param name="topid">父级ID，默认0</param>
        /// <returns></returns>
        public static List<ElementTreeInfo> GetElementTreeInfo(List<ZtreeInfo> ztree, string topid = "0")
        {
            List<ElementTreeInfo> elementTreeInfos = new List<ElementTreeInfo>();

            //拿出顶点数据放到字典
            var topInfo = ztree.Where(x => x.pId == topid).ToList();
            foreach (var t in topInfo)
            {
                ElementTreeInfo TreeInfo = new ElementTreeInfo();
                TreeInfo.id = t.id;
                TreeInfo.name = t.name;
                TreeInfo = AddChildren(TreeInfo, ztree);
                elementTreeInfos.Add(TreeInfo);
            }

            return elementTreeInfos;
        }

        /// <summary>
        /// 给ElementTree递归添加子项
        /// </summary>
        /// <param name="TreeInfo">ElementTree对象</param>
        /// <param name="ztree">ZTree列表对象</param>
        /// <returns></returns>
        private static ElementTreeInfo AddChildren(ElementTreeInfo TreeInfo, List<ZtreeInfo> ztree)
        {
            var child = ztree.Where(x => x.pId == TreeInfo.id);
            foreach (var c in child)
            {
                ElementTreeInfo cTreeInfo = new ElementTreeInfo();
                cTreeInfo.id = c.id;
                cTreeInfo.name = c.name;
                cTreeInfo = AddChildren(cTreeInfo, ztree);
                TreeInfo.children.Add(cTreeInfo);
            }
            return TreeInfo;
        }

        #endregion

        #region 菜单下拉树打包成element树结构

        /// <summary>
        /// 传入ZtreeInfo对象，根据内容递归转成对应的ElementTreeInfo格式
        /// </summary>
        /// <param name="ztree">Ztree列表对象</param>
        /// <param name="topid">父级ID，默认0</param>
        /// <returns></returns>
        public static List<ElementTreeInfo> GetMenuElementTreeInfo(List<ZtreeInfo> ztree, List<SysMenuEntity> menus, string topid = "0")
        {
            List<ElementTreeInfo> elementTreeInfos = new List<ElementTreeInfo>();

            //拿出顶点数据放到字典
            var topInfo = ztree.Where(x => x.pId == topid).ToList();
            foreach (var t in topInfo)
            {
                ElementTreeInfo TreeInfo = new ElementTreeInfo();
                TreeInfo.id = t.id;
                TreeInfo.name = t.name;
                var m = menus.Where(ex => ex.ID.ToString() == t.id).Single();
                if (m != null)
                {
                    TreeInfo.authorize = m.Authorize;
                    TreeInfo.icon = m.MenuIcon;
                    TreeInfo.sort = m.MenuSort;
                    TreeInfo.target = m.MenuTarget;
                    TreeInfo.type = m.MenuType;
                    TreeInfo.url = m.MenuUrl;
                }
                TreeInfo = AddMenuChildren(TreeInfo, ztree, menus);
                elementTreeInfos.Add(TreeInfo);
            }

            return elementTreeInfos;
        }

        /// <summary>
        /// 给ElementTree递归添加子项
        /// </summary>
        /// <param name="TreeInfo">ElementTree对象</param>
        /// <param name="ztree">ZTree列表对象</param>
        /// <returns></returns>
        private static ElementTreeInfo AddMenuChildren(ElementTreeInfo TreeInfo, List<ZtreeInfo> ztree, List<SysMenuEntity> menus)
        {
            var child = ztree.Where(x => x.pId == TreeInfo.id);
            foreach (var c in child)
            {
                ElementTreeInfo cTreeInfo = new ElementTreeInfo();
                cTreeInfo.id = c.id;
                cTreeInfo.name = c.name;
                var m = menus.Where(ex => ex.ID.ToString() == c.id).Single();
                if (m != null)
                {
                    cTreeInfo.authorize = m.Authorize;
                    cTreeInfo.icon = m.MenuIcon;
                    cTreeInfo.sort = m.MenuSort;
                    cTreeInfo.target = m.MenuTarget;
                    cTreeInfo.type = m.MenuType;
                    cTreeInfo.url = m.MenuUrl;
                }
                cTreeInfo = AddMenuChildren(cTreeInfo, ztree, menus);
                TreeInfo.children.Add(cTreeInfo);
            }
            return TreeInfo;
        }

        #endregion

        #region 二进制枚举遍历取多个值

        /// <summary>
        /// 二进制枚举遍历取多个值
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <typeparam name="T1">二进制字段</typeparam>
        /// <param name="Entities">实体类</param>
        /// <param name="EnumTableName">枚举表名称</param>
        /// <param name="BinaryProp">二进制字段</param>
        /// <param name="NameProp">枚举名称字段</param>
        /// <param name="ListProp">枚举数字列表字段</param>
        /// <param name="IDColumnName">枚举表ID字段名称</param>
        /// <param name="NameColumnName">枚举表Name字段名称</param>
        /// <returns></returns>
        public async Task<List<T>> GetBinaryValue<T, T1>
            (
                List<T> Entities, 
                string EnumTableName, 
                Expression<Func<T, T1>> BinaryProp,
                Expression<Func<T, string>> NameProp,
                Expression<Func<T, List<long>>> ListProp,
                string IDColumnName = "ID",
                string NameColumnName = "Name"
            ) 
            where T : class, new()
        {
            Type type = typeof(T);
            PropertyInfo[] properties = ReflectionHelper.GetProperties(type);

            string strBinaryProp = GetPropertyName(BinaryProp);
            string strNameProp = GetPropertyName(NameProp);
            string strListProp = GetPropertyName(ListProp);

            //获取列名对应的PropertyInfo
            var pBinaryProp = properties.Where(p => p.Name.ToUpper() == strBinaryProp.ToUpper()).Single();
            var pNameProp = properties.Where(p => p.Name.ToUpper() == strNameProp.ToUpper()).Single();
            var pListProp = properties.Where(p => p.Name.ToUpper() == strListProp.ToUpper()).Single();

            var list = await EnumCache.Table(EnumTableName, IDColumnName, NameColumnName).GetList();
            foreach (var entity in Entities)
            {
                try
                {
                    var post = Convert.ToInt64(pBinaryProp.GetValue(entity));
                    List<string> names = new List<string>();
                    List<long> ids = new List<long>();
                    foreach (var ev in list)
                    {
                        var v = Convert.ToInt64(ev.ID);
                        if ((post & v) == v)
                        {
                            ids.Add(v);
                            names.Add(ev.Name);
                        }
                    }
                    pNameProp.SetValue(entity, string.Join(",", names));
                    pListProp.SetValue(entity, ids);
                }
                catch (Exception err)
                { }
            }
            return Entities;
        }

        /// <summary>
        /// 二进制枚举遍历取多个值
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <typeparam name="T1">二进制字段</typeparam>
        /// <param name="Entity">实体类</param>
        /// <param name="EnumTableName">枚举表名称</param>
        /// <param name="BinaryProp">二进制字段</param>
        /// <param name="NameProp">枚举名称字段</param>
        /// <param name="ListProp">枚举数字列表字段</param>
        /// <param name="IDColumnName">枚举表ID字段名称</param>
        /// <param name="NameColumnName">枚举表Name字段名称</param>
        /// <returns></returns>
        public async Task<T> GetBinaryValue<T, T1>
            (
                T Entity,
                string EnumTableName,
                Expression<Func<T, T1>> BinaryProp,
                Expression<Func<T, string>> NameProp,
                Expression<Func<T, List<long>>> ListProp,
                string IDColumnName = "ID",
                string NameColumnName = "Name"
            )
            where T : class, new()
        {
            var ret = await GetBinaryValue(new List<T>() { Entity }, EnumTableName, BinaryProp, NameProp, ListProp, IDColumnName, NameColumnName);
            return ret[0];
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 若字符串长度不够2位，使字符串长度变更为2位，在前面补0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string StrLengthTo2(string str)
        {
            while (str.Length < 2)
            {
                str = "0" + str;
            }
            return str;
        }

        /// <summary>
        /// 不区分大小写判断两个字符串是否相同
        /// </summary>
        /// <param name="a">第一个字符串</param>
        /// <param name="b">第二个字符串</param>
        /// <returns></returns>
        public bool StringUpperEqual(string a, string b)
        {
            if (a.ToUpper() == b.ToUpper())
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
