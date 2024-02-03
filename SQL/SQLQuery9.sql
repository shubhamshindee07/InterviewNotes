use TestDB

DECLARE @TempTable TABLE([FirstName] [nvarchar] (500)) 
	insert into @TempTable([FirstName]) select FirstName from Table1

select * from @TempTable

Declare @Fname varchar(MAX)

--select Count(*) from @TempTable

while EXISTS(select top 1 FirstName from @TempTable)
Begin
select top 1 @Fname=FirstName from @TempTable
print @Fname
delete from @TempTable where FirstName = @Fname
print @Fname
END

select * from @TempTable

DECLARE @stringToSplit VARCHAR(MAX)
DECLARE @pos INT
 DECLARE @name NVARCHAR(255)

 select @stringToSplit = '1,30,2,0'

 SELECT @pos  = CHARINDEX(',', @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)


  print  @pos
  print @name
  print  LEN(@stringToSplit)
  print  Substring(@stringToSplit,@pos + 1, LEN(@stringToSplit))
  print SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)

  select * from table1
  select * from table2

  select distinct DepartmenName, count(*) from table1 t1 inner join Table2 t2 on t1.FirstName=t2.FirstName group by DepartmenName

  select distinct Salary from Table1 where salary in (Select Salary from table2 where Salary > 1000)

