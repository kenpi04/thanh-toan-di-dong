/*
insert into UrlRecord(EntityId, EntityName, IsActive, LanguageId, Slug)

select x.id, 
'StateProvince', 1, 0,
REPLACE(LOWER(x.NonUnicode),' ','-') as link
from (
select s.Id, dbo.ChangeTextToNonUniCode(s.Name) as NonUnicode
from StateProvince s
)x
*/
/*
insert into UrlRecord(EntityId, EntityName, IsActive, LanguageId, Slug)

select x.id, 
'District', 1, 0,
REPLACE(REPLACE(REPLACE(LOWER(x.NonUnicode),' ','-'),'q.','quan'),'h.','huyen') as link
from (
select d.Id, dbo.ChangeTextToNonUniCode(d.Name) as NonUnicode
from District d
)x
*/
/*
insert into UrlRecord(EntityId, EntityName, IsActive, LanguageId, Slug)

select x.id, 
'Ward', 1, 0,
REPLACE(rtrim(ltrim((LOWER(x.NonUnicode)))),' ','-') as link
from (
select d.Id, dbo.ChangeTextToNonUniCode(d.Name) as NonUnicode
from Ward d
)x
*/
create table #s (Id int identity(1,1), StreetId  int, Link nvarchar(max))
create table #t (id int, slug nvarchar(max))

--insert into UrlRecord(EntityId, EntityName, IsActive, LanguageId, Slug)
insert into #s(StreetId, Link)
select x.id, 
--'Street', 1, 0,
'duong-' + REPLACE(rtrim(ltrim((LOWER(x.NonUnicode)))),' ','-') --+'-'+ cast(x.Id as varchar(100)) as link
from (
select d.Id, dbo.ChangeTextToNonUniCode(d.Name) as NonUnicode
from Street d
)x

declare @i int = 1
declare @max int, @id int;
declare @slnew nvarchar(max)

set @max = (select count(1) from #s)
while(@i <= @max)
begin
	select @slnew = s.Link, @id = s.StreetId from #s as s where s.Id = @i
	if exists(select 1 from #t u where u.slug = @slnew)
	begin
		set @slnew = @slnew +'-'+ cast((select count(1) from #t ur where ur.Slug like @slnew+'%') as nvarchar(100))
	end
	insert into #t(id, slug)
	values(@id, @slnew)
 set @i = @i + 1;

end

update #t set slug = REPLACE(slug,'duong-duong-','duong-')
--select * from #t order by #t.slug;
insert into UrlRecord(EntityId, EntityName, IsActive, LanguageId, Slug)
select id, 'Street',  1, 0, slug
from #t
drop table #s;
drop table #t;

