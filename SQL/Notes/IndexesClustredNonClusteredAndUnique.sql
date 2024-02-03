

--Indexex

--1)There are many types of indexes -1.Clustered 2.Nonclustered,unique and non unique

--non clustred index
--1. non clustred index are slighly slower than cluster index.
--2. non clustred index stores address of of each record like in book so we can find the records much faster.
--3. non clustred index occupies extra disk space because of it stores addresses to identify each record uniquely in diff table.
--4. We can cretate.


create NonClustered index IX_Table1_FirstName
on dbo.Table1(FirstName asc)

drop index Table1.IX_Table1_Salary

select * from Table1

--Clustred index

--1.Clustred indexes is faster than non clustred index.
--2.We can create only one clustred index at a time for one table but we pass multiple columns in that so that the index apply on 
--multiple columns.
--3.If we create non clustred index data will get automatically arrange if we fire select * from tablename query.
--4.Cluastered index by default automatically arrange data in asc order.

create clustered index Clustered_IX_Table1_Salary
on Table1(Salary desc)

--unique,non unique index,unique clustered index
--1. we can create unique index on multiple columns like firstname and lastname. combination of both columns need to be different in next rows.
--2. primary key automatically creates unique non clustred index.
--3. If we create unique index then it automatically creates unique non clustered index
--4. uniue clustered index bydefault automatically arrange data in asc order

create unique index UQ_IX_Table2_ID
on Table2(ID)

create unique index UQ_IX_Table2_FirstNameLastName
on Table2(FirstName,LastName)

create unique clustered index UQClustered_IX_Table2_ID
on Table2(ID asc)

select * from Table2




