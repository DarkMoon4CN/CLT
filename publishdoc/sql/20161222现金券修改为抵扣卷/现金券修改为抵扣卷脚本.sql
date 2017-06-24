--帐户流水表
update [bonus_account_water] set award_description=REPLACE(award_description,'现金券','抵扣券') where award_description like '%现金券%'
select * from [bonus_account_water] where award_description like '%抵扣券%'
--活动计划表
update hx_Activity_schedule set activity_schedule_name=REPLACE(activity_schedule_name,'现金券','抵扣券') where activity_schedule_name like '%现金券%'
select * from hx_Activity_schedule where activity_schedule_name like '%抵扣券%'
--活动表
update hx_ActivityTable set ActName=REPLACE(ActName,'现金券','抵扣券') where ActName like '%现金券%'
select * from hx_ActivityTable where ActName like '%抵扣券%'
--自动更新表
update hx_AppUpdatePackage set [Description]=REPLACE([Description],'现金券','抵扣券') where [Description] like '%现金券%'
select * from hx_AppUpdatePackage where [Description] like '%抵扣券%'
--广告
update hx_td_about_news set News_title=REPLACE(News_title,'现金券','抵扣券'),News_Key=REPLACE(News_Key,'现金券','抵扣券'),news_Des=REPLACE(news_Des,'现金券','抵扣券') where News_title like '%现金券%'  or News_Key  like '%现金券%' or news_Des  like '%现金券%'
select * from hx_td_about_news where News_title like '%抵扣券%'  or News_Key  like '%抵扣券%' or news_Des  like '%抵扣券%'
--电话回访表
update hx_td_Phone_records set recordcontext=REPLACE(recordcontext,'现金券','抵扣券') where recordcontext like '%现金券%'
select * from hx_td_Phone_records where recordcontext like '%抵扣券%'
--短信记录表
update hx_td_SMS_record set smscontext=REPLACE(smscontext,'现金券','抵扣券') where smscontext like '%现金券%'
select * from hx_td_SMS_record where smscontext like '%抵扣券%'
--短信通知表
update hx_td_SMSEmail set SEContext=REPLACE(SEContext,'现金券','抵扣券') where SEContext like '%现金券%'
select * from hx_td_SMSEmail where SEContext like '%抵扣券%'
--系统消息表
update hx_td_System_message set MContext=REPLACE(MContext,'现金券','抵扣券') where MContext like '%现金券%'
select * from hx_td_System_message where MContext like '%抵扣券%'
--代金券表
update hx_UserAct set Title=REPLACE(title,'现金券','抵扣券') where Title like '%现金券%'
select * from hx_UserAct where Title like '%抵扣券%'
--大转盘表
update LuckDrawRecord set Ldre_AwardName=REPLACE(Ldre_AwardName,'现金券','抵扣券') where Ldre_AwardName like '%现金券%'
select * from LuckDrawRecord where Ldre_AwardName like '%抵扣券%'