-----------------bengin 插入奖励类型数据-----------------------------------
set IDENTITY_INSERT [hx_RewardType]  on
INSERT INTO [hx_RewardType] ([RewTypeID], [RewTypeName]) VALUES ('1', '现金');
INSERT INTO [hx_RewardType] ([RewTypeID], [RewTypeName]) VALUES ('2', '现金券');
INSERT INTO [hx_RewardType] ([RewTypeID], [RewTypeName]) VALUES ('3', '加息券');
set IDENTITY_INSERT [hx_RewardType]  off
-----------------end 插入奖励类型数据------------------------------------
go
-----------------bengin 插入活动类型数据-----------------------------------
set IDENTITY_INSERT [hx_ActivityType]  on
INSERT INTO [hx_ActivityType] ([ActTypeId], [ActName]) VALUES ('1', '新人注册');
INSERT INTO [hx_ActivityType] ([ActTypeId], [ActName]) VALUES ('2', '短期活动');
INSERT INTO [hx_ActivityType] ([ActTypeId], [ActName]) VALUES ('3', '常规活动');
INSERT INTO [hx_ActivityType] ([ActTypeId], [ActName]) VALUES ('4', '邀请活动');
INSERT INTO [hx_ActivityType] ([ActTypeId], [ActName]) VALUES ('5', '系统赠送');
set IDENTITY_INSERT [hx_ActivityType]  off
-----------------end 插入活动类型数据------------------------------------
go 
-----------------------系统活动相关数据补录及历史活动数据迁移脚本 bengin-------------------

set IDENTITY_INSERT [dbo].[hx_ActivityTable]  on--------------------
--------转盘活动所需----
insert into [dbo].[hx_ActivityTable](ActID,ActTypeId,RewTypeID,ActName,ActUser,ActStarttime,ActEndtime,ActRule,ActState,createtime)values(303,2,1,'9月抽奖送50元现金',5,'2016-09-01 00:00:00.000','2016-10-01 00:00:00.000','{"rule":3,"cash":50,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":50,"startAmt":0,"endAmt":0}],"MAmtList":null}',1,'2016-08-30 14:10:00.000')
insert into [dbo].[hx_ActivityTable](ActID,ActTypeId,RewTypeID,ActName,ActUser,ActStarttime,ActEndtime,ActRule,ActState,createtime)values(304,2,2,'9月抽奖送50元现金券',5,'2016-09-01 00:00:00.000','2016-10-01 00:00:00.000','{"rule":3,"cash":50,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":50,"startAmt":5000,"endAmt":0}],"MAmtList":null}',1,'2016-08-30 14:10:00.000')
insert into [dbo].[hx_ActivityTable](ActID,ActTypeId,RewTypeID,ActName,ActUser,ActStarttime,ActEndtime,ActRule,ActState,createtime)values(305,2,2,'9月抽奖送20元现金券',5,'2016-09-01 00:00:00.000','2016-10-01 00:00:00.000','{"rule":3,"cash":20,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":20,"startAmt":2000,"endAmt":0}],"MAmtList":null}',1,'2016-08-30 14:10:00.000')
insert into [dbo].[hx_ActivityTable](ActID,ActTypeId,RewTypeID,ActName,ActUser,ActStarttime,ActEndtime,ActRule,ActState,createtime)values(306,2,2,'9月抽奖送10元现金券',5,'2016-09-01 00:00:00.000','2016-10-01 00:00:00.000','{"rule":3,"cash":10,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":10,"startAmt":500,"endAmt":0}],"MAmtList":null}',1,'2016-08-30 14:10:00.000')
insert into [dbo].[hx_ActivityTable](ActID,ActTypeId,RewTypeID,ActName,ActUser,ActStarttime,ActEndtime,ActRule,ActState,createtime)values(307,2,3,'9月抽奖送1%加息券',5,'2016-09-01 00:00:00.000','2016-10-01 00:00:00.000','{"rule":3,"cash":1,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":1,"startAmt":0,"endAmt":0}],"MAmtList":null}',1,'2016-08-30 14:10:00.000')
insert into [dbo].[hx_ActivityTable](ActID,ActTypeId,RewTypeID,ActName,ActUser,ActStarttime,ActEndtime,ActRule,ActState,createtime)values(308,2,3,'9月抽奖送2%加息券',5,'2016-09-01 00:00:00.000','2016-10-01 00:00:00.000','{"rule":3,"cash":2,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":2,"startAmt":0,"endAmt":0}],"MAmtList":null}',1,'2016-08-30 14:10:00.000')
---后台赠送券所需-----
INSERT INTO [hx_ActivityTable] ([ActID], [ActTypeId], [RewTypeID], [ActName], [ActUser], [ActStarttime], [ActEndtime], [ActRule], [ActState], [createtime]) VALUES ('182', '5', '2', '系统赠送现金券', '5', '2016-01-01 00:00:00.000', '2036-01-01 00:00:00.000', NULL, '1', '2016-08-25 09:07:48.767');
INSERT INTO [hx_ActivityTable] ([ActID], [ActTypeId], [RewTypeID], [ActName], [ActUser], [ActStarttime], [ActEndtime], [ActRule], [ActState], [createtime]) VALUES ('183', '5', '3', '系统赠送加息券', '5', '2016-01-01 00:00:00.000', '2036-01-01 00:00:00.000', NULL, '1', '2016-08-25 09:09:45.913');
insert into [dbo].[hx_ActivityTable](ActID,[ActTypeId],[RewTypeID],[ActName],[ActUser],[ActStarttime],[ActEndtime],[ActRule]
      ,[ActState])values(323,2,1,'复投返现金','2','2016-09-01 00:00:00.000','2016-09-30 23:59:59.999','{"rule":2,"cash":0,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":49,"startAmt":0,"endAmt":0},{"cashAmt":99,"startAmt":0,"endAmt":0},{"cashAmt":199,"startAmt":0,"endAmt":0},{"cashAmt":149,"startAmt":0,"endAmt":0},
{"cashAmt":299,"startAmt":0,"endAmt":0},{"cashAmt":599,"startAmt":0,"endAmt":0},{"cashAmt":249,"startAmt":0,"endAmt":0},{"cashAmt":499,"startAmt":0,"endAmt":0},{"cashAmt":999,"startAmt":0,"endAmt":0},{"cashAmt":0.005,"startAmt":0,"endAmt":0},
{"cashAmt":0.01,"startAmt":0,"endAmt":0},{"cashAmt":0.02,"startAmt":0,"endAmt":0},],"MAmtList":null}',2);


set IDENTITY_INSERT [dbo].[hx_ActivityTable]  off
---------------------------------------------------
go 


------迁移数据--------------------------------
----迁移 activity加息券活动（1% 2% 3%）  到hx_ActivityTable 主键偏移 100
set IDENTITY_INSERT [dbo].[hx_ActivityTable]  on--------------------
insert into hx_ActivityTable (ActID,
ActTypeId,RewTypeID,ActName,ActUser,ActStarttime,
ActEndtime,ActRule,ActState,createtime) 
(SELECT ActivityId+ 100 ,5,3,ActivityName,5,StartDate,EndDate, 
'{"rule":3,"cash":'+cast(AddRate as varchar)+',"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":'+cast(AddRate as varchar)+',"startAmt":0,
"endAmt":0],"MAmtList":null}', 
ActStatus, CreatedOn from Activity)
set IDENTITY_INSERT [dbo].[hx_ActivityTable]  off--------------------
go

----迁移 activitylogs加息券奖励记录  到hx_UserAct  为防止id重复，主键偏移 1000 ActivityId+ 100
set IDENTITY_INSERT [dbo].[hx_UserAct]  on-------------------- 
insert into hx_UserAct(
      UserAct,registerid,ActID,
      RewTypeID,ActTypeId,Amt,Uselower,
      Usehight,AmtEndtime,AmtUses,UseState,AmtProid,UseTime,ISSmsOne,
      IsSmsThree,IsSmsSeven,isSmsFifteen,
      isSmsSixteen,OrderID,Createtime,Title)
      select LogId+1000,UserId,ActivityId+100,3,5,AddRate,0,0,UseEndOn,2,UseStatus,UsedTargetId,bid.invest_time,0,    0,0,0,0,UsedRecordId,CreateOn,ActivityName from activitylogs actl LEFT  JOIN hx_Bid_records bid on actl.UsedRecordId=bid.bid_records_id 
set IDENTITY_INSERT [dbo].[hx_UserAct]  off-------------------- 
go

----查找加息券抽奖记录  更新ActID为307-1% 308-2%、ActTypeId=2 短期活动
update hx_UserAct set ActID=307,ActTypeId=2  where title like '%抽奖%' and Amt=1.00 and Createtime>'2016-09-01';
update hx_UserAct set ActID=308,ActTypeId=2  where title like '%抽奖%' and Amt=2.00 and Createtime>'2016-09-01';

--------将老活动的结束时间修改为2016-08-31
update [hx_Activity_schedule] set end_date='2016-08-31 23:59:59' where end_date> getdate() and activity_schedule_id<46
 
----迁移 hx_activity_schedule返现/红包活动  数据迁移到hx_ActivityTable activity_schedule_id+偏移0
set IDENTITY_INSERT [dbo].[hx_ActivityTable]  on--------------------
insert into hx_ActivityTable (ActID,
ActTypeId,RewTypeID,ActName,ActUser,ActStarttime,
ActEndtime,ActRule,ActState,createtime) 
(
SELECT activity_schedule_id,2 --hx_ActivityType 活动类型：新人活动/短期活动/常规活动/邀请活动/系统赠送  
,case when use_lower_limit=0 then 1 else 2 end--hx_RewardType 奖励类型：1现金/2现金券/3加息券 有两处（53）需要更换为现金1
,activity_schedule_name,5,start_date,end_date
,'{"rule":3,"cash":'+cast(amount_of_reward as varchar)+',"ISsplit":2,"Uses":2,
"Msplitarr":[{"cashAmt":'+cast(amount_of_reward as varchar)+',
"startAmt":'+cast(use_lower_limit as varchar)+',"endAmt":0],"MAmtList":null}' 
,case when end_date<GETDATE() then 2 else 1 END
--0默认(未上线)  1进行中(上线)   2结束(下线)   3停止
,entry_time from hx_activity_schedule
)
set IDENTITY_INSERT [dbo].[hx_ActivityTable]  off--------------------
go 



----迁移 bonus_account 返现/现金券奖励记录（有效时间、使用限制） 数据迁移到到hx_UserAct 主键偏移 20000 bonus_account_id+20000  activity_schedule_id+偏移0
------清理无效奖励及明细 因自增id获取失败导致的奖励记录错误
------select * from bonus_account_water where  membertable_registerid not in (SELECT registerid from hx_member_table )
delete from bonus_account_water where  membertable_registerid not in (SELECT registerid from hx_member_table )
delete from  bonus_account where membertable_registerid not in (SELECT registerid from hx_member_table )
go 

set IDENTITY_INSERT [dbo].[hx_UserAct]  on-------------------- 
insert into hx_UserAct(
      UserAct,registerid,ActID,
      RewTypeID,ActTypeId,Amt,Uselower,
      Usehight,AmtEndtime,AmtUses,UseState,AmtProid,UseTime,ISSmsOne,
      IsSmsThree,IsSmsSeven,isSmsFifteen,
      isSmsSixteen,OrderID,Createtime,Title)

      select bonus_account_id+20000,membertable_registerid,activity_schedule_id,
      2-----2 RewTypeID 奖励类型关联id：现金/现金券/加息券
      ,2--ActTypeId 活动类型关联id：新人活动/短期活动/常规活动/邀请活动/系统赠送 
      ,amount_of_reward,use_lower_limit,0,end_date,
      2-----AmtUses, 1 仅单独使用  2可组合使用
      ,reward_state,null,bid.invest_time,0, 0,0,0,0,
      proid,entry_time,activity_schedule_name 
      from bonus_account ba LEFT  JOIN hx_Bid_records bid on ba.proid=bid.bid_records_id ;
set IDENTITY_INSERT [dbo].[hx_UserAct]  off--------------------
go 


---区分抽奖活动 和注册活动 
---47    2     2     9月活动送500元奖励
---48    2     2     9月活动送200元奖励
---49    2     2     9月活动送50元奖励-- 抽奖
---50    2     2     9月活动送20元奖励-- 抽奖
---51    2     2     9月活动送10元奖励-- 抽奖
---52    2     2     9月活动送8元奖励
---53    2     1     9月活动送50元红包--------------303

-- 9月活动送50元现金更新为 303
update hx_UserAct set ActID=303,RewTypeID=1,UseState=1 where ActID=53;
delete from hx_ActivityTable where ActID=53; 
-- 9月活动送10元奖励更新为 306
update hx_UserAct set ActID=306 where ActID=51 and Title like '%抽奖%';
-- 9月活动送20元奖励更新为 305
update hx_UserAct set ActID=305 where ActID=50 and Title like '%抽奖%';
-- 9月活动送50元奖励更新为 304
update hx_UserAct set ActID=304 where ActID=49 and Title like '%抽奖%';
go 

---9月注册188 888红包 系统升级前 变更为新人活动
update hx_UserAct set ActTypeId=1 where ActID between 47 and 52
go 

---历史返现活动 状态更新 hx_RewardType----- 奖励类型：1现金/2现金券/3加息券
update hx_UserAct set RewTypeID=1, UseState=1 where Uselower=0 and RewTypeID<>3 and RewTypeID<>1
--------------------------------------------------------------------------------
go 

----begin 添加9月注册 188红包活动-----
set IDENTITY_INSERT [dbo].[hx_ActivityTable]  on--------------------
INSERT INTO [hx_ActivityTable] ([ActID], [ActTypeId], [RewTypeID], [ActName], [ActUser], [ActStarttime], [ActEndtime], [ActRule], [ActState], [createtime]) VALUES ('309', '1', '2', '9月新人注册送188红包', '0', '2016-09-01 00:00:00.000', '2016-10-01 00:00:00.000', '{"rule":1,"cash":188,"ISsplit":1,"Uses":2,"Msplitarr":[{"cashAmt":50,"startAmt":5000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":50,"startAmt":5000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":50,"startAmt":5000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":20,"startAmt":2000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":10,"startAmt":500,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":8,"startAmt":400,"endAmt":0,"endTime":"\/Date(1475251200000)\/"}],"MAmtList":null}', '1', '2016-09-22 15:50:41.537');
set IDENTITY_INSERT [dbo].[hx_ActivityTable]  off--------------------
----end 添加9月注册 188红包活动-----

----begin 添加9月首投888红包活动---
set IDENTITY_INSERT [dbo].[hx_ActivityTable]  on--------------------
INSERT INTO [hx_ActivityTable] ([ActID], [ActTypeId], [RewTypeID], [ActName], [ActUser], [ActStarttime], [ActEndtime], [ActRule], [ActState], [createtime]) VALUES ('310', '3', '2', '9月首投送888红包test', '1', '2016-09-01 00:00:00.000', '2016-10-01 00:00:00.000', '{"rule":1,"cash":888,"ISsplit":1,"Uses":2,"Msplitarr":[{"cashAmt":500,"startAmt":50000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":200,"startAmt":20000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":50,"startAmt":5000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":20,"startAmt":2000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":10,"startAmt":500,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":8,"startAmt":400,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":50,"startAmt":5000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":20,"startAmt":2000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":20,"startAmt":2000,"endAmt":0,"endTime":"\/Date(1475251200000)\/"},{"cashAmt":10,"startAmt":400,"endAmt":0,"endTime":"\/Date(1475164800000)\/"}],"MAmtList":null}', '1', '2016-09-22 15:50:41.537');
set IDENTITY_INSERT [dbo].[hx_ActivityTable]  off--------------------
----end 添加9月首投888红包活动---
go 

-----------------------系统活动相关数据补录及历史活动数据迁移脚本 end-------------------


-----------------begin 活动相关外键-----------------------------------
alter table hx_ActivityTable add constraint FK_HX_ACTIV_REFERENCE_HX_REWAR foreign key (RewTypeID) references hx_RewardType (RewTypeID);
alter table hx_ActivityTable add constraint FK_HX_ACTIV_REFERENCE_HX_ACTIV foreign key (ActTypeId) references hx_ActivityType (ActTypeId);

alter table hx_UserAct add constraint FK_HX_USERA_REFERENCE_HX_REWAR foreign key (RewTypeID) references hx_RewardType (RewTypeID);
alter table hx_UserAct add constraint FK_HX_USERA_REFERENCE_HX_MEMBE foreign key (registerid) references hx_member_table (registerid);
alter table hx_UserAct add constraint FK_HX_USERA_REFERENCE_HX_ACTIV2 foreign key (ActID) references hx_ActivityTable (ActID);
alter table hx_UserAct add constraint FK_HX_USERA_REFERENCE_HX_ACTIV foreign key (ActTypeId) references hx_ActivityType (ActTypeId);
go 

-----------------end 活动相关外键-----------------------------------



---------------------------------begin 追加 复投返现活动9月复投返现活动---------------------
--insert into [dbo].[hx_ActivityTable]([ActTypeId],[RewTypeID],[ActName],[ActUser],[ActStarttime],[ActEndtime],[ActRule],[ActState])values(4,1,'复投返现金','2','2016-09-01 00:00:00.000','2016-09-30 23:59:59.999','{"rule":2,"cash":0,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":49,"startAmt":0,"endAmt":0},{"cashAmt":99,"startAmt":0,"endAmt":0},{"cashAmt":199,"startAmt":0,"endAmt":0},{"cashAmt":149,"startAmt":0,"endAmt":0},
--{"cashAmt":299,"startAmt":0,"endAmt":0},{"cashAmt":599,"startAmt":0,"endAmt":0},{"cashAmt":249,"startAmt":0,"endAmt":0},{"cashAmt":499,"startAmt":0,"endAmt":0},{"cashAmt":999,"startAmt":0,"endAmt":0},{"cashAmt":0.005,"startAmt":0,"endAmt":0},{"cashAmt":0.01,"startAmt":0,"endAmt":0},{"cashAmt":0.02,"startAmt":0,"endAmt":0},],"MAmtList":null}',2)
---------------------------------begin  9月复投返现活动---------------------
