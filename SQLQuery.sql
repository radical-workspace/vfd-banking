select * from [AspNetUsers] a inner join Admins ad on a.Id = ad.Id

select * from [AspNetUsers] a inner join Managers m on a.Id = m.Id

select * from [AspNetUsers] a inner join Tellers t on a.Id = t.Id

select * from [AspNetUsers] a inner join Customers c on a.Id = c.Id


select * from AspNetUsers 
select * from [dbo].[AspNetRoles]


select * from Admins
select * from Managers
select * from Customers
select * from [dbo].[Tellers]


select * from Accounts
select * from Branches
select * from Banks

