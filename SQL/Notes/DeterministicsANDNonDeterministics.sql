---Deterministics and nonDeterministics--


---Deterministics
--1)The value of output cannot be changed everytime when you execute the query or function until and unless the database state is change
--2)Sum,SQUARE,power,AVG, and count all of the aggregate functions comes under this.

select count(*) from tblEmployee


---nonDeterministics
--1)The value of output can be change whener evrytime you excute the statement
--1)eg DateTime

select GETDATE()

--Rand function
--1)If we pass seed to it it always act as deterministics if we dont pass then it acts like nondeterministics

select Rand()
select Rand(1)--seed