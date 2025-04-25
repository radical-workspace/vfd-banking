select * from [AspNetUsers] a inner join Admins ad on a.Id = ad.Id

select * from [AspNetUsers] a inner join Managers m on a.Id = m.Id

select * from [AspNetUsers] a inner join Tellers t on a.Id = t.Id

select * from [AspNetUsers] a inner join Customers c on a.Id = c.Id


-- delete from Managers


select * from AspNetUsers 
select * from [dbo].[AspNetRoles]


select * from Admins
select * from Managers
select * from Customers
select * from [dbo].[Tellers]


select * from Accounts
select * from Cards
select * from Branches
select * from Banks
select * from SupportTickets
select * from GeneralCertificates


select u.UserName [Teller Name], t.Id [Teller ID], u.Email, c.Id [CID], b.Name [Branch], a.*
from AspNetUsers u inner join Tellers t
	on u.Id = t.Id inner join Branches b
	on t.BranchId = b.Id inner join Customers c
	on b.Id = c.BranchId inner join Accounts a
	on a.CustomerId = c.Id
	where t.Id like '571b33ca-09fd-4a41-99ee-d0d7aa882716'



select * 
from Accounts a inner join Customers cu
	on a.CustomerId = cu.Id


select * 
from Accounts a inner join Cards c
	on c.AccountNumber = a.Number inner join Customers cu
	on a.CustomerId = cu.Id


begin tran
delete from Cards
delete from Accounts
delete from SupportTickets
-- commit
-- rollback

select len(17733955482855)

select * from Transactions


begin tran
delete from Customers where BranchId is null
delete from Transactions
