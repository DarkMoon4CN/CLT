
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Channel_Invite'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Channel_Invite'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Channel_Invite'

GO

/****** Object:  View [dbo].[V_Channel_Invite]    Script Date: 2016/11/1 11:52:03 ******/
DROP VIEW [dbo].[V_Channel_Invite]
GO

/****** Object:  View [dbo].[V_Channel_Invite]    Script Date: 2016/11/1 11:52:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_Channel_Invite]
AS
SELECT   ISNULL(b.bid_records_id, 1) AS ID, b.OrdId, m.registerid, m.username, m.realname, c.ChannelID, c.ChannelName, 
                c.CreateTime, c.UpdateTime, c.Status, a.AdminUserName, m.registration_time, b.invest_time, b.ordstate, 
                m.channel_invitedcode, b.investment_amount, b.invest_state, bt.borrowing_title, CONVERT(VARCHAR(19), 
                bt.life_of_loan) + CASE WHEN bt.unit_day = 1 THEN 'ÔÂ' WHEN bt.unit_day = 3 THEN 'Ìì' END AS DeadLine, 
                (CASE WHEN bt.unit_day = 3 THEN life_of_loan WHEN bt.unit_day = 1 THEN life_of_loan * 30 END) 
                AS DeadLineNumber
FROM      dbo.hx_Bid_records AS b LEFT OUTER JOIN
                dbo.hx_member_table AS m ON m.registerid = b.investor_registerid RIGHT OUTER JOIN
                dbo.hx_Channel AS c ON m.channel_invitedcode = c.Invitedcode LEFT OUTER JOIN
                dbo.hx_Channel_AdminUser_Rel AS r ON r.ChannelID = c.ChannelID LEFT OUTER JOIN
                dbo.hx_Channel_AdminUser AS a ON a.AdminUserID = r.AdminUserID LEFT OUTER JOIN
                dbo.hx_borrowing_target AS bt ON b.targetid = bt.targetid
WHERE   (1 = 1) AND (b.bid_records_id IN
                    (SELECT   MIN(bid_records_id) AS Expr1
                     FROM      dbo.hx_Bid_records
                     WHERE   (ordstate = 1)
                     GROUP BY investor_registerid))

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[24] 4[16] 2[56] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "m"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 145
               Right = 500
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 538
               Bottom = 145
               Right = 708
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "r"
            Begin Extent = 
               Top = 6
               Left = 746
               Bottom = 107
               Right = 912
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 950
               Bottom = 145
               Right = 1159
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "bt"
            Begin Extent = 
               Top = 150
               Left = 38
               Bottom = 289
               Right = 341
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 82
         Width = 284
         Width = 2655
         Width = 1500
         Width = 1500
         Width = 1500' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Channel_Invite'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2250
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Channel_Invite'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Channel_Invite'
GO


