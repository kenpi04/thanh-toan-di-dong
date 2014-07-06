alter procedure [dbo].[GetSlugFromId]
(
	@domainName nvarchar(100) = 'zhouse.com.vn', --ten domain, ex: zhouse.com
	@categoryId int = 0, --id category
	@stateProvinceId int = 0, --id tinh/thanh
	@districtId int = 0, --id quan/huyen
	@wardId int = 0, -- id phuong/xa
	@streetId int = 0, -- id duong/pho
	@priceString nvarchar(100) = '', --gia, ex: 1000-1500 <=> 1 ty - 1,5 ty
	@attributeOptionIds nvarchar(max) = '', --id specificationAttributeOption, ex: 43
	@slug nvarchar(max) out --url return: http://domain/slugcategory-{streetSlug||wardSlug||districtSlug||stateprovinceSlug||}_pr-1000-1500_sa-1-2-3
)
as
begin
	if(isnull(@slug,'') = '') set @slug = '';
	if(isnull(@domainName,'') != '')
	begin
		if(@categoryId > 0)		
			set @slug = @slug + isnull((select top(1) isnull(u.Slug,'') from UrlRecord u with(nolock) where u.EntityName = 'Category' and u.EntityId = @categoryId and u.IsActive = 1),'')
		if(isnull(@slug,'') != '')
		begin

			if(@streetId > 0 or @wardId > 0)
			begin
				If(@streetId > 0)
					set @slug = @slug + isnull((select top(1) isnull('-'+u.Slug,'') from UrlRecord u with(nolock) where u.EntityName = 'Street' and u.EntityId = @streetId and u.IsActive = 1),'')
				If(@wardId > 0)
					set @slug = @slug + isnull((select top(1) isnull('-'+u.Slug,'') from UrlRecord u with(nolock) where u.EntityName = 'Ward' and u.EntityId = @wardId and u.IsActive = 1),'')
			end
			else if(@districtId > 0)
			begin		
				set @slug = @slug + isnull((select top(1) isnull('-'+u.Slug,'') from UrlRecord u with(nolock) where u.EntityName = 'District' and u.EntityId = @districtId and u.IsActive = 1),'')
			end
			else if(@stateProvinceId > 0)
			begin
				set @slug = @slug + isnull((select top(1) isnull('-'+u.Slug,'') from UrlRecord u with(nolock) where u.EntityName = 'StateProvince' and u.EntityId = @stateProvinceId and u.IsActive = 1),'')
			end

			if(isnull(@priceString,'') != '')
				set @slug = @slug + '_pr-' + @priceString;
			if(isnull(@attributeOptionIds,'') != '')
				set @slug = @slug + '_sa-' + @attributeOptionIds;
		end
				
		if(isnull(@slug,'') != '')
			set @slug = 'http://' + @domainName + '/' + @slug;	 
	end
end
/*
declare @slug nvarchar(max)
execute [GetSlugFromId]
@
@categoryId = 3,
@stateProvinceId = 23,
@districtId = 0,
@wardId = 10,
@streetId = 0,
@priceString = '1000-1500',
@attibuteOptionIds = '77',
@slug = @slug out

print @slug
*/