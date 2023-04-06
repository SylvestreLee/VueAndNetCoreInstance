# 1.数据库
## 1.1新增
> ### SysMenu
```
USE [GCVEPIN]
GO

/****** Object:  Table [dbo].[SysMenu]    Script Date: 2021/5/26 9:44:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysMenu](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentId] [bigint] NOT NULL,
	[MenuName] [nvarchar](50) NOT NULL,
	[MenuIcon] [varchar](50) NULL,
	[MenuUrl] [varchar](100) NULL,
	[MenuTarget] [varchar](50) NULL,
	[MenuSort] [int] NULL,
	[MenuType] [int] NOT NULL,
	[Authorize] [varchar](50) NULL,
	[SystemType] [int] NULL,
	[Remark] [nvarchar](50) NULL,
	[Status] [bigint] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysMenu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父菜单Id(0表示是根菜单)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'ParentId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuIcon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单Url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuUrl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接打开方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuTarget'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuSort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型(1目录 2页面 3按钮)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单状态(0禁用 1启用)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单权限标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'Authorize'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'Remark'
GO
```
> ### SysMenuAuthorize
```
USE [GCVEPIN]
GO

/****** Object:  Table [dbo].[SysMenuAuthorize]    Script Date: 2021/5/26 10:09:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysMenuAuthorize](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MenuId] [bigint] NULL,
	[AuthorizeId] [bigint] NULL,
	[AuthorizeType] [int] NULL,
	[Status] [bigint] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysMenuAuthorize] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenuAuthorize', @level2type=N'COLUMN',@level2name=N'MenuId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权Id(角色Id或者用户Id)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenuAuthorize', @level2type=N'COLUMN',@level2name=N'AuthorizeId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权类型(1角色 2用户)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenuAuthorize', @level2type=N'COLUMN',@level2name=N'AuthorizeType'
GO
```
> ### SysRole
```
USE [GCVEPIN]
GO

/****** Object:  Table [dbo].[SysRole]    Script Date: 2021/5/26 10:13:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysRole](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[RoleSort] [int] NULL,
	[Remark] [nvarchar](50) NULL,
	[Status] [bigint] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysRole', @level2type=N'COLUMN',@level2name=N'RoleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysRole', @level2type=N'COLUMN',@level2name=N'RoleSort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysRole', @level2type=N'COLUMN',@level2name=N'Remark'
GO
```
> ### SysUserBelong
```
USE [GCVEPIN]
GO

/****** Object:  Table [dbo].[SysUserBelong]    Script Date: 2021/5/26 10:15:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysUserBelong](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[BelongId] [bigint] NOT NULL,
	[BelongType] [int] NOT NULL,
	[Status] [bigint] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysUserBelong] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUserBelong', @level2type=N'COLUMN',@level2name=N'UserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位Id或者角色Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUserBelong', @level2type=N'COLUMN',@level2name=N'BelongId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属类型(1职位 2角色)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUserBelong', @level2type=N'COLUMN',@level2name=N'BelongType'
GO
```
> ### SysLogApi
```
USE [GCVEPIN]
GO

/****** Object:  Table [dbo].[SysLogApi]    Script Date: 2021/5/26 14:20:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysLogApi](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BaseCreatorId] bigint  NULL,
	[Remark] [nvarchar](50) NULL,
	[LogStatus] [int] NOT NULL,
	[ExecuteUrl] [varchar](100) NOT NULL,
	[ExecuteParam] [nvarchar](4000) NULL,
	[ExecuteResult] [nvarchar](4000) NULL,
	[ExecuteTime] [int] NOT NULL,
	[Status] [bigint] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysLogApi] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'LogStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接口地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'ExecuteUrl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'ExecuteParam'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求结果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'ExecuteResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'ExecuteTime'
GO
```
> ### SysLogLogin
```
USE [GCVEPIN]
GO

/****** Object:  Table [dbo].[SysLogLogin]    Script Date: 2021/5/26 14:19:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysLogLogin](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BaseCreatorId] bigint  NULL,
	[LogStatus] [int] NULL,
	[IpAddress] [varchar](20) NULL,
	[IpLocation] [nvarchar](50) NULL,
	[Browser] [nvarchar](50) NULL,
	[OS] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[ExtraRemark] [nvarchar](500) NULL,
	[Status] [bigint] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysLogLogin] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'LogStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'IpAddress'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'IpLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览器' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'Browser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作系统' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'OS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'额外备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'ExtraRemark'
GO



```
> ### SysLogOperate
```
USE [GCVEPIN]
GO

/****** Object:  Table [dbo].[SysLogOperate]    Script Date: 2021/5/26 14:19:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysLogOperate](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BaseCreatorId] bigint  NULL,
	[LogStatus] [int] NULL,
	[IpAddress] [varchar](20) NULL,
	[IpLocation] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[LogType] [varchar](50) NULL,
	[BusinessType] [varchar](50) NULL,
	[ExecuteUrl] [nvarchar](100) NULL,
	[ExecuteParam] [nvarchar](4000) NULL,
	[ExecuteResult] [nvarchar](4000) NULL,
	[ExecuteTime] [int] NULL,
	[Status] [bigint] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysLogOperate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'LogStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'IpAddress'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'IpLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志类型(暂未用到)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'LogType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务类型(暂未用到)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'BusinessType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'ExecuteUrl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'ExecuteParam'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求结果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'ExecuteResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'ExecuteTime'
GO
```
> ### Enum_Inpsection_InspectionCount
```
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enum_Inspection_InspectionCount](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Remarks] [text] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Enum_Inspection_InspectionCount] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Enum_Inspection_InspectionCount] ON 

INSERT [dbo].[Enum_Inspection_InspectionCount] ([ID], [Name], [Remarks], [UpdateTime]) VALUES (1, N'初检', NULL, CAST(0x0000AD3B00000000 AS DateTime))
INSERT [dbo].[Enum_Inspection_InspectionCount] ([ID], [Name], [Remarks], [UpdateTime]) VALUES (2, N'复检', NULL, CAST(0x0000AD3B00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[Enum_Inspection_InspectionCount] OFF
```

### Enum_Inpsection_ZhongDianCheXing
```
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Enum_Inspection_ZhongDianCheXing](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Remarks] [text] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Enum_Inspection_ZhongDianCheXing] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Enum_Inspection_ZhongDianCheXing] ON 

INSERT [dbo].[Enum_Inspection_ZhongDianCheXing] ([ID], [Name], [Remarks], [UpdateTime]) VALUES (1, N'初检不合格车辆', NULL, CAST(0x0000AD3B00000000 AS DateTime))
INSERT [dbo].[Enum_Inspection_ZhongDianCheXing] ([ID], [Name], [Remarks], [UpdateTime]) VALUES (2, N'外埠车辆', NULL, CAST(0x0000AD3B00000000 AS DateTime))
INSERT [dbo].[Enum_Inspection_ZhongDianCheXing] ([ID], [Name], [Remarks], [UpdateTime]) VALUES (2, N'运营5年以上柴油车', NULL, CAST(0x0000AD3B00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[Enum_Inspection_ZhongDianCheXing] OFF
```
## 1.2修改
> ### Person
```
ALTER TABLE [Person] ADD Token varchar(32) NULL;		--记录用户Token，控制单用户登陆。
ALTER TABLE [Person] ADD IsSystem bit NULL;				--确定用户是否是最高权限。
ALTER TABLE [Regulator] ADD AreaCode nvarchar(50) NULL;	--用来确定区县环保局
```
> ### Regulator
1.表中的Status字段大小写修改正确，类型修改正确。

2.AreaCode填上对应辖区值，例如370102
> ### Camera
VideoServerID 改成 Bigint
> ### VideoServer
StationID 改为 不可空
> ### Line
1.RegistorID 可为空

2.RegTime 可为空
> ### Device_WJ
Updatetime 改成 UpdateTime
> ### DISCounter
id 改成 ID
> ### VideoServer
ALTER TABLE [VideoServer] ADD SIPPort int NULL;	--本地SIP端口
ALTER TABLE [VideoServer] ADD SIPServerID nvarchar(50) NULL;	--SIP服务器ID
ALTER TABLE [VideoServer] ADD SIPServerDomain nvarchar(50) NULL;	--SIP服务域
ALTER TABLE [VideoServer] ADD SIPServerIP nvarchar(50) NULL;	--SIP服务器地址
ALTER TABLE [VideoServer] ADD SIPServerPort int NULL;	--SIP服务器端口
ALTER TABLE [VideoServer] ADD SIPAuthorizationID nvarchar(50) NULL;	--SIP用户认证ID

update VideoServer set SIPAuthorizationID=convert(varchar(10),IANo)+'001110000001' from station where station.id=videoserver.stationid

> ### Camera
ALTER TABLE [Camera] ADD ChannelCodingID nvarchar(50) NULL;	--视频通道编码ID

update Camera set ChannelCodingID=CONVERT(varchar(10),t.IANo)+'00132000000'+CONVERT(varchar(10),Channel) from (select v.id,s.IANo from VideoServer v join station s on s.id=v.stationid) t where VideoServerID=t.ID and Channel<10

update Camera set ChannelCodingID=CONVERT(varchar(10),t.IANo)+'0013200000'+CONVERT(varchar(10),Channel) from (select v.id,s.IANo from VideoServer v join station s on s.id=v.stationid) t where VideoServerID=t.ID and Channel>=10 and Channel<100

update Camera set ChannelCodingID=CONVERT(varchar(10),t.IANo)+'001320000'+CONVERT(varchar(10),Channel) from (select v.id,s.IANo from VideoServer v join station s on s.id=v.stationid) t where VideoServerID=t.ID and Channel>=100
