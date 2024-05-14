--drop database DesafioOriginSW;
--go

create database DesafioOriginSW;
go

use DesafioOriginSW;
go

CREATE TABLE account (
  id_account int PRIMARY KEY IDENTITY(1, 1),
  balance float NOT NULL
);
GO

CREATE TABLE bank_card (
  id_bank_card int PRIMARY KEY IDENTITY(1, 1),
  id_account int NOT NULL,
  number varchar(16) UNIQUE NOT NULL,
  pin varchar(4) NOT NULL,
  id_card_state int NOT NULL,
  [expiry_date] date NOT NULL,
  failed_attempts int NOT NULL default 0
);
GO

CREATE TABLE card_state (
  id_card_state int PRIMARY KEY IDENTITY(1, 1),
  [name] varchar(50) NOT NULL
);
GO

CREATE TABLE operation (
  id_operation int PRIMARY KEY IDENTITY(1, 1),
  id_bank_card int NOT NULL,
  id_operation_type int NOT NULL,
  [date] datetime NOT NULL,
  amount float
);
GO

CREATE TABLE operation_type (
  id_operation_type int PRIMARY KEY IDENTITY(1, 1),
  [name] varchar(50) NOT NULL
);
GO

ALTER TABLE bank_card ADD FOREIGN KEY (id_account) REFERENCES account (id_account);
GO

ALTER TABLE bank_card ADD FOREIGN KEY (id_card_state) REFERENCES card_state (id_card_state);
GO

ALTER TABLE operation ADD FOREIGN KEY (id_bank_card) REFERENCES bank_card (id_bank_card);
GO

ALTER TABLE operation ADD FOREIGN KEY (id_operation_type) REFERENCES operation_type (id_operation_type);
GO
