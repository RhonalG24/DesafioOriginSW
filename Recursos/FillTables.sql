use DesafioOriginSW;
go

insert into card_state (name) values ('activa'), ('bloqueada'), ('cancelada');
go

insert into operation_type (name) values ('balance'), ('retiro');
go

insert into account (balance) values (120000), (50000), (200);
go

insert into bank_card (id_account, number,  pin, [expiry_date], id_card_state) values 
(1, 1256837467897647, 8673, CONVERT(date, DATEADD(YEAR, 10, GETDATE()), 101), 1), 
(2, 1234123412341234, 1234, CONVERT(date, DATEADD(YEAR, 10, GETDATE()), 101), 1),
 (2, 6245000050502020, 2020, CONVERT(date, DATEADD(YEAR, 10, GETDATE()), 101), 1), 
(3, 6245000010104040, 4242, CONVERT(date, DATEADD(YEAR, 10, GETDATE()), 101), 1);
go