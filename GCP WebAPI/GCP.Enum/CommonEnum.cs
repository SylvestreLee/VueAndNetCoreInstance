using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Enum
{

    #region 内置枚举

    public enum Status
    {
        未审核 = 0,
        正常 = 1,
        删除 = 2,
        禁用 = 3
    }

    public enum StatusEnum
    {
        [Description("启用")]
        Yes = 1,

        [Description("禁用")]
        No = 0
    }

    public enum IsEnum
    {
        [Description("是")]
        Yes = 1,

        [Description("否")]
        No = 0
    }

    public enum NeedEnum
    {
        [Description("不需要")]
        NotNeed = 0,

        [Description("需要")]
        Need = 1
    }

    public enum OperateStatusEnum
    {
        [Description("失败")]
        Fail = 0,

        [Description("成功")]
        Success = 1
    }

    public enum UploadFileType
    {
        [Description("头像")]
        Portrait = 1,

        [Description("新闻图片")]
        News = 2,

        [Description("导入的文件")]
        Import = 10
    }

    public enum PlatformEnum
    {
        [Description("Web后台")]
        Web = 1,

        [Description("WebApi")]
        WebApi = 2
    }

    public enum PayStatusEnum
    {
        [Description("未知")]
        Unknown = 0,

        [Description("已支付")]
        Success = 1,

        [Description("转入退款")]
        Refund = 2,

        [Description("未支付")]
        NotPay = 3,

        [Description("已关闭")]
        Closed = 4,

        [Description("已撤销（付款码支付）")]
        Revoked = 5,

        [Description("用户支付中（付款码支付）")]
        UserPaying = 6,

        [Description("支付失败(其他原因，如银行返回失败)")]
        PayError = 7
    }

    #endregion

    #region 公共枚举

    public enum TreeOption
    { 
        单选 = 1,
        多选 = 2,
        单选不选父类 = 3,
        多选不选父类 = 4,
    }


    /// <summary>
    /// 枚举常用按钮供前台调用
    /// </summary>
    public enum OperateButtonList
    {
        [Description("查询")]
        btnSearch = 0,

        [Description("新增")]
        btnAdd = 1,

        [Description("编辑")]
        btnEdit = 2,

        [Description("删除")]
        btnDelete = 3,

        [Description("导出Excel")]
        btnExport = 6,

        [Description("权限分配")]
        btnRole = 10,

        [Description("启用")]
        btnEnable = 11,

        [Description("禁用")]
        btnDisable = 12,

        [Description("重置密码")]
        btnResetPwd = 21,



        [Description("跳过OBD检查")]
        btnSkipCheck = 50,

        [Description("终止检测")]
        btnTerminate = 51,

        [Description("取消报检")]
        btnNInspection = 52,
        //其他特殊按钮，按照顺序在50之后排列
        //一定遵循书写格式来，加上Description，按钮枚举名称btn(小写)+按钮作用的英语(这部分首字母大写)
    }

    #endregion

    #region 业务枚举--业务枚举都写在这里

    /// <summary>
    /// 有无枚举
    /// </summary>
    public enum HasNoEnum
    { 
        无 = 0,
        有 = 1
    }

    public enum IfNeedConfirmEnum
    { 
        不必读 = 0,
        必读 = 1
    }

    #endregion

}
