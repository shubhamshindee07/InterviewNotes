----Triggers----



--Types of triggers are DML,DDL,CLR and Logon triggers.

--DML 
--1)DML trigger events executed when insert,update and delete event occures.there are two types of DMl triggers
--which are 1.After trigger 2.instead triggers.

--After triggers or instead triggers gets executed when insert,update,delete event occures.
--same as for instead triggers.

--After triggers gets executed after event occures like insert etc.
--Instead trigger gets executed before event occures like insert,update etc.

--In 'insert' sql generated table.insert the new latest data when we fire update or delete,In 'delete' table insert the old record


--DDL

--which is used to log some one create table,rename table,alter table,create function,proc and many more.
--we can prevent changes in table,alter table,fn,proc and many more by using rollback.


--Logon
--Which is used for login restrictions,set Limited connections  ,tracking login activity
