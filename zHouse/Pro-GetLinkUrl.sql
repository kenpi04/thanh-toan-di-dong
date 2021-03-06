USE [DB_9B0E81_zhouse]
GO
/****** Object:  StoredProcedure [dbo].[GetUrlLink]    Script Date: 7/2/2014 9:05:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetUrlLink]
(
	@slug				NVARCHAR(MAX),
	@categoryId			INT OUT,
	@streetId			INT OUT,
	@wardId				INT OUT,
	@districtId			INT OUT,
	@stateProvinceId	INT OUT
 )
AS
IF(ISNULL(@slug,'') != '')
BEGIN
DECLARE @link NVARCHAR(MAX)
--set default value
SET @categoryId = 0;
SET @streetId = 0;
SET @wardId = 0;
SET @districtId = 0;
SET @stateProvinceId = 0;
SET @link = @slug;

BEGIN
	IF EXISTS(SELECT 1 FROM UrlRecord u WITH(NOLOCK) WHERE u.Slug = @link)
	BEGIN
		set @categoryId = -1;
		SELECT TOP(1) * FROM UrlRecord u WITH(NOLOCK) WHERE u.Slug = @link
	END
END

IF(@categoryId != -1)
BEGIN
DECLARE @categorySlug nvarchar(max), @streetSlug nvarchar(max), @wardSlug nvarchar(max), @districtSlug nvarchar(max), @stateProvinceSlug nvarchar(max);

--cate -> street -> ward -> district -> state
 select @categoryId = c.EntityId, @categorySlug = c.Slug
 from (select top(1) u.EntityId, u.Slug	from UrlRecord u with(nolock)
		where u.EntityName = 'Category'	and u.IsActive = 1 and @link like u.Slug+'%'
		order by Slug desc) c
		
--if categoryId >0 do next find address
 if isnull(@categoryId,0) > 0
	begin
	 --replace
	 set @link = REPLACE(@link, @categorySlug, '')
	 
	 --print @link;	 
	if(isnull(@link,'')<>'')
	begin

		--next for street
		if (left(@link,1) = '-') set @link = RIGHT(@link, len(@link)-1)		
		select @streetId = c.EntityId, @streetSlug = c.Slug
		from (select top(1) u.EntityId, u.Slug from UrlRecord u with(nolock)
			where u.EntityName = 'Street' and u.IsActive = 1 and @link like u.Slug+'%'
			order by Slug desc) c
		if (isnull(@streetId,0) > 0)
			begin
				set @link = REPLACE(@link, @streetSlug, '')				
				set @districtId = (select top(1)s.DistrictId from Street s with(nolock) where s.Id = @streetId)
				set @stateProvinceId = (select top(1)d.StateProvinceId from District d with(nolock) where d.Id = @districtId)
			end
		
		--next for ward
		if(isnull(@link,'') <>'')
			begin
				if (left(@link,1) = '-') set @link = RIGHT(@link, len(@link)-1)
				select @wardId = c.EntityId, @wardSlug = c.Slug
				from (select top(1) u.EntityId, u.Slug from UrlRecord u with(nolock) 
						where u.EntityName = 'Ward' and u.IsActive = 1 and @link like u.Slug+'%'
						order by Slug desc) c
				if (isnull(@wardId,0) > 0 and isnull(@streetId,0) = 0)
					begin						
						set @link = REPLACE(@link, @wardSlug, '')
						if(isnull(@link,'')='')
							begin
								set @districtId = (select top(1)w.DistrictId from Ward w with(nolock) where w.Id = @wardId)
								set @stateProvinceId = (select top(1)d.StateProvinceId from District d with(nolock) where d.Id = @districtId)
							end
						else set @categoryId = 0;
							
					end
				else if(isnull(@wardId,0) = 0 and isnull(@streetId,0) <> 0)
					begin
						set @categoryId = 0;
					end
			end
		
		--elseif for district
		if(Isnull(@streetId,0) = 0 and isnull(@wardId,0) = 0)
			begin
				if (left(@link,1) = '-') set @link = RIGHT(@link, len(@link)-1)
				select @districtId = c.EntityId, @districtSlug = c.Slug
				from (select top(1) u.EntityId, u.Slug from UrlRecord u with(nolock)
					where u.EntityName = 'District'	and u.IsActive = 1 and @link like u.Slug+'%'
					order by Slug desc) c
				if(isnull(@districtId,0) > 0)
					begin
						set @link = REPLACE(@link, @districtSlug, '')
						if(ISNULL(@link,'')='')
							set @stateProvinceId = (select top(1)d.StateProvinceId from District d with(nolock) where d.Id = @districtId)
						else set @categoryId = 0;
					end

				--elseif for state
				else 
					begin
						select @stateProvinceId = c.EntityId, @stateProvinceSlug = c.Slug
						from (select top(1) u.EntityId, u.Slug from UrlRecord u with(nolock)
							where u.EntityName = 'StateProvince' and @link like u.Slug+'%'
							order by Slug desc) c
						if (isnull(@stateProvinceId,0) > 0)
							begin
								set @link = REPLACE(@link, @stateProvinceSlug, '')
								if (ISNULL(@link,'')<>'')
									set @categoryId = 0;
							end
						else set @categoryId = 0;

					end
			end
	end
end
	
IF(@categoryId > 0)
	BEGIN
		SELECT [Id] = 0,
		[EntityId] = 0,
		[EntityName] = 'Search',
		[Slug] = @slug,
		[IsActive] = cast(1 as bit),
		[LanguageId] = 0
	END
END
END
/*

declare @sid int, @wid int, @did int, @stid int, @cid int;
execute [GetUrlLink]
@slug = 'nha-mat-tien-quan-1',
@categoryId = @cid out,
@streetId = @sid out,
@wardId = @wid out,
@districtId = @did out,
@stateProvinceId = @stid out

select @cid as cate, @sid as street, @wid as ward, @did as distict, @stid as sate

*/

