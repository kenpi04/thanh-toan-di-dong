ALTER PROCEDURE [dbo].[GetUrlLink]
(
	@slug				NVARCHAR(MAX),--http://domain/slugcategory-{streetSlug||wardSlug||districtSlug||stateprovinceSlug||}_pr-1000-1500_sa-1-2-3
	@categoryId			INT OUT,
	@streetId			INT OUT,
	@wardId				INT OUT,
	@districtId			INT OUT,
	@stateProvinceId	INT OUT,
	@priceString		NVARCHAR(100) OUT,--100-1500
	@attributeOptions	NVARCHAR(200) OUT--1-2-3-4
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
SET @priceString = '';
SET @attributeOptions = '';
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
	Declare @entityName varchar(50) = 'category';
	declare @lin table(id int identity(1,1), data nvarchar(max))
	insert into @lin(data) select data from dbo.nop_splitstring_to_table(@slug, '_');
	set @link = (select data from @lin where id = 1)
	
	

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
			DECLARE @linkSub nvarchar(max), @count int, @i int = 1;
			set @count = (select count(1) from @lin);
			while(@i < @count)
			begin
				set @i = @i + 1;
				set @linkSub = (select data from @lin where id = @i);
				if(@linkSub like 'pr-%')
					set @priceString = replace(@linkSub,'pr-','');
				else if(@linkSub like 'sa-%')
					set @attributeOptions = replace(@linkSub,'sa-','');
				else begin set @categoryId = 0; break; end;				
			end
			if(@count>1) set @entityName = 'Search';
		end
	if(@categoryId > 0) 
		BEGIN
			SELECT [Id] = 0,
			[EntityId] = @categoryId,
			[EntityName] = @entityName,
			[Slug] = @slug,
			[IsActive] = cast(1 as bit),
			[LanguageId] = 0
		END
	ELSE
		begin
			declare @t table(Id int, EntityId int, EntityName nvarchar(1), Slug nvarchar(1), IsActive bit, LanguageId int)
			select * from @t;
		end
	
	END
END
/*

declare @sid int, @wid int, @did int, @stid int, @cid int, @priceString nvarchar(max), @attributeOptions nvarchar(max)
execute [GetUrlLink]
@slug = 'du-an-nha-o-quan-1_pr-1500-3000',
@categoryId = @cid out,
@streetId = @sid out,
@wardId = @wid out,
@districtId = @did out,
@stateProvinceId = @stid out,
@priceString = @priceString out, 
@attributeOptions = @attributeOptions out

select @cid as cate, @sid as street, @wid as ward, @did as distict, @stid as sate, @priceString as pr, @attributeOptions as saop

*/

