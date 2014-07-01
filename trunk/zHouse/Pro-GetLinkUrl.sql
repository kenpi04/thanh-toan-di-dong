alter procedure dbo.[GetUrlLink]
(
	@link nvarchar(max),
	@categoryId int out,
	@streetId int out,
	@wardId int out,
	@districtId int out,
	@stateProvinceId int out
 )
as
if Isnull(@link,'') != ''
Begin
--declare @categoryId int = 0;--, @streetId int = 0, @wardId int = 0, @districtId int = 0, @stateProvinceId int = 0;
set @categoryId = 0;
set @streetId = 0;
set @wardId = 0;
set @districtId = 0;
set @stateProvinceId = 0;

declare @categorySlug nvarchar(max), @streetSlug nvarchar(max), @wardSlug nvarchar(max), @districtSlug nvarchar(max), @stateProvinceSlug nvarchar(max);

--cate -> street -> ward -> district -> state
 select @categoryId = c.EntityId, @categorySlug = c.Slug
 from (select top(1) u.EntityId, u.Slug	from UrlRecord u with(nolock)
		where u.EntityName = 'Category'	and @link like u.Slug+'%'
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
			where u.EntityName = 'Street' and @link like u.Slug+'%'
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
						where u.EntityName = 'Ward' and @link like u.Slug+'%'
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
					where u.EntityName = 'District'	and @link like u.Slug+'%'
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
end

/*

declare @sid int, @wid int, @did int, @stid int, @cid int;
execute [GetUrlLink]
@link = 'nha-mat-tien-duong-yet-kieu-8-1',
@categoryId = @cid out,
@streetId = @sid out,
@wardId = @wid out,
@districtId = @did out,
@stateProvinceId = @stid out

select @cid as cate, @sid as street, @wid as ward, @did as distict, @stid as sate
*/

