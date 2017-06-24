

/****** Object:  Table [dbo].[XiCaiUsers]    Script Date: 2016/10/31 12:14:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[XiCaiUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Mobile] [nvarchar](50) NOT NULL,
	[XiCaiUserID] [int] NOT NULL,
	[RegisterUserID] [int] NOT NULL,
	[Display] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_XiCaiUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


