---- Scalar Inline Function-------
--1) It returns single value is known as scalar inline function
--2) we can used inline function in select query whereas in procedures we cannot used proc in select statememt.
--3) It encloses within begin and end keywords
Create function fn_getAge(@DOB Date)
returns int
as 
begin

return DateDiff(YEAR, @DOB,getdate())

end

select dbo.fn_getAge('2010-04-10 11:35:26.033')

select name,dbo.fn_getAge(Datetime) from tblEmployee