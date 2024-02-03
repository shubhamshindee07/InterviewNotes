use TestDB
----Multivalued Table function----

--1)this returns also table like table valued inline function but we cannot update this table.
--2)It encloses within begin and end keyword
--3)Inline table valued function is faster than multivalued table value fn.
--4)If we used the schemaBounding in function then we cannot deleter table used in that function.

create function fn_getEMPData()
returns @TableData Table (ID int, FirstName varchar(50),Salary int)
as
begin

insert into @TableData
select ID,FirstName,Salary from Table1

return

end

--schemabound
create function fn_getEMPData2()
returns @TableData Table (ID int, FirstName varchar(50),Salary int)
as
begin

insert into @TableData
select ID,FirstName,Salary from [TestDB].[dbo].[Table1]

return

end

drop table dbo.Table1

select ID,FirstName,Salary from dbo.fn_getEMPData2()

update dbo.fn_getEMPData2() set FirstName='MITU' where FirstName='Isha'