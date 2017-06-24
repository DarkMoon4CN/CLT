/*
Navicat SQL Server Data Transfer

Source Server         : 线上测试
Source Server Version : 110000
Source Host           : 118.244.207.21:1433
Source Database       : onchuangtou
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 110000
File Encoding         : 65001

Date: 2016-09-26 20:00:35
*/


-- ----------------------------
-- Table structure for ApplicationAuthorization
-- ----------------------------
DROP TABLE [dbo].[ApplicationAuthorization]
GO
CREATE TABLE [dbo].[ApplicationAuthorization] (
[AppId] int NOT NULL ,
[AppName] nvarchar(250) NULL ,
[AppSecret] nvarchar(68) NULL ,
[AppSafeCode] nvarchar(68) NULL ,
[AppServerIps] nvarchar(800) NULL ,
[IsDelete] int NULL ,
[AppStatus] int NULL ,
[CreatedOn] datetime NULL ,
[UpdatedOn] datetime NULL 
)


GO

-- ----------------------------
-- Indexes structure for table ApplicationAuthorization
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table ApplicationAuthorization
-- ----------------------------
ALTER TABLE [dbo].[ApplicationAuthorization] ADD PRIMARY KEY NONCLUSTERED ([AppId])
GO
