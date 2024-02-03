----Inline Table valued function -----
--1)It returns table which is why we called Inline Table Valued fn.
--2)It does not contains begin and end keyword
--3)This table is just like a view we can also update this table.

create function fn_getEmployeeData(@Salary int)
returns Table
as 

return (select [FirstName] from Table1 where Salary=@Salary)

select * from fn_getEmployeeData(2000)

update fn_getEmployeeData(2000) set FirstName='SAM' where  FirstName='sam'