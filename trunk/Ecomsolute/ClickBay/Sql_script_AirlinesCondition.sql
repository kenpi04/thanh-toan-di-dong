SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AirlinesCondition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AirlinesId] [nvarchar](50) NOT NULL,
	[ConditionName] [nvarchar](100) NOT NULL,
	[ConditionDescription] [nvarchar](400) NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_AirlinesCondition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AirlinesCondition] ADD  CONSTRAINT [DF_AirlinesCondition_DisplayOrder]  DEFAULT ((0)) FOR [DisplayOrder]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id hang hang khong _ Arilines' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AirlinesCondition', @level2type=N'COLUMN',@level2name=N'AirlinesId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dieu kien g' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AirlinesCondition', @level2type=N'COLUMN',@level2name=N'ConditionName'
GO


