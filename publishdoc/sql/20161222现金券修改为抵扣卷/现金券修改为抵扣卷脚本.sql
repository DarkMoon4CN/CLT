--�ʻ���ˮ��
update [bonus_account_water] set award_description=REPLACE(award_description,'�ֽ�ȯ','�ֿ�ȯ') where award_description like '%�ֽ�ȯ%'
select * from [bonus_account_water] where award_description like '%�ֿ�ȯ%'
--��ƻ���
update hx_Activity_schedule set activity_schedule_name=REPLACE(activity_schedule_name,'�ֽ�ȯ','�ֿ�ȯ') where activity_schedule_name like '%�ֽ�ȯ%'
select * from hx_Activity_schedule where activity_schedule_name like '%�ֿ�ȯ%'
--���
update hx_ActivityTable set ActName=REPLACE(ActName,'�ֽ�ȯ','�ֿ�ȯ') where ActName like '%�ֽ�ȯ%'
select * from hx_ActivityTable where ActName like '%�ֿ�ȯ%'
--�Զ����±�
update hx_AppUpdatePackage set [Description]=REPLACE([Description],'�ֽ�ȯ','�ֿ�ȯ') where [Description] like '%�ֽ�ȯ%'
select * from hx_AppUpdatePackage where [Description] like '%�ֿ�ȯ%'
--���
update hx_td_about_news set News_title=REPLACE(News_title,'�ֽ�ȯ','�ֿ�ȯ'),News_Key=REPLACE(News_Key,'�ֽ�ȯ','�ֿ�ȯ'),news_Des=REPLACE(news_Des,'�ֽ�ȯ','�ֿ�ȯ') where News_title like '%�ֽ�ȯ%'  or News_Key  like '%�ֽ�ȯ%' or news_Des  like '%�ֽ�ȯ%'
select * from hx_td_about_news where News_title like '%�ֿ�ȯ%'  or News_Key  like '%�ֿ�ȯ%' or news_Des  like '%�ֿ�ȯ%'
--�绰�طñ�
update hx_td_Phone_records set recordcontext=REPLACE(recordcontext,'�ֽ�ȯ','�ֿ�ȯ') where recordcontext like '%�ֽ�ȯ%'
select * from hx_td_Phone_records where recordcontext like '%�ֿ�ȯ%'
--���ż�¼��
update hx_td_SMS_record set smscontext=REPLACE(smscontext,'�ֽ�ȯ','�ֿ�ȯ') where smscontext like '%�ֽ�ȯ%'
select * from hx_td_SMS_record where smscontext like '%�ֿ�ȯ%'
--����֪ͨ��
update hx_td_SMSEmail set SEContext=REPLACE(SEContext,'�ֽ�ȯ','�ֿ�ȯ') where SEContext like '%�ֽ�ȯ%'
select * from hx_td_SMSEmail where SEContext like '%�ֿ�ȯ%'
--ϵͳ��Ϣ��
update hx_td_System_message set MContext=REPLACE(MContext,'�ֽ�ȯ','�ֿ�ȯ') where MContext like '%�ֽ�ȯ%'
select * from hx_td_System_message where MContext like '%�ֿ�ȯ%'
--����ȯ��
update hx_UserAct set Title=REPLACE(title,'�ֽ�ȯ','�ֿ�ȯ') where Title like '%�ֽ�ȯ%'
select * from hx_UserAct where Title like '%�ֿ�ȯ%'
--��ת�̱�
update LuckDrawRecord set Ldre_AwardName=REPLACE(Ldre_AwardName,'�ֽ�ȯ','�ֿ�ȯ') where Ldre_AwardName like '%�ֽ�ȯ%'
select * from LuckDrawRecord where Ldre_AwardName like '%�ֿ�ȯ%'