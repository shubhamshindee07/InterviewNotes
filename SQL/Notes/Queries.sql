use TestDB

  select top 1 salary from Table1 where Salary < (select MAX(Salary) from Table1) order by salary desc


WITH ss
AS (select *, Row_Number() over (Partition by FirstName order by FirstName) as RowNumber 
	from [TestDB].[dbo].Table2)
delete 
FROM ss where RowNumber > 1;


select * from Table1
select * from Table2

select * from Table1,Table2

select Max(Salary) from Table2 where Salary < (select Max(Salary) from Table2 where Salary < (select Max(Salary) from Table2))

select * from Table1 cross join Table2



