
ALTER VIEW [dbo].[V_type_news]
AS
SELECT  dbo.hx_td_about_news.newid, dbo.hx_td_about_news.web_Type_menu_id, dbo.hx_td_about_news.News_title, 
	dbo.hx_td_about_news.News_Key, dbo.hx_td_about_news.news_Des, dbo.hx_td_about_news.context, 
	dbo.hx_td_about_news.createtime, dbo.hx_td_about_news.adminuserid, 
	dbo.hx_td_web_type.menu_name, dbo.hx_td_web_type.parentid, 
	dbo.hx_td_web_type.rootid, dbo.hx_td_web_type.path1, dbo.hx_td_web_type.orderid,
	hx_td_web_type_1.menu_name AS topmenuname, dbo.hx_td_about_news.comm,
	dbo.hx_td_about_news.listcomm, dbo.hx_td_about_news.newimg,dbo.hx_td_about_news.ClickCount
FROM	hx_td_about_news 
	left join hx_td_web_type 
		ON hx_td_about_news.web_Type_menu_id=hx_td_web_type.menu_id 
	left join hx_td_web_type AS hx_td_web_type_1
		ON dbo.hx_td_web_type.rootid = hx_td_web_type_1.menu_id

GO


