
-- Switch to the system (aka master) database
USE master;
GO

-- Delete the PotHole Database (IF EXISTS)
IF EXISTS(select * from sys.databases where name='PotHoleDB')
DROP DATABASE PotHoleDB;
GO

-- Create a new DemoDB Database
CREATE DATABASE PotHoleDB;
GO

-- Switch to the DemoDB Database
USE PotHoleDB
GO

BEGIN TRANSACTION;

CREATE TABLE users
(
	id			int			identity(1,1),
	username	varchar(50)	not null,
	password	varchar(50)	not null,
	salt		varchar(50)	not null,
	role		varchar(50)	default('user'),

	constraint pk_users primary key (id)
)

create table PotHole(
PotHoleID int primary key identity,
PotHoleLocation nvarchar(100) not null,
DateReported date not null,
Severity int,
RepairStatus nvarchar(100) not null,
RepairBeginDate date,
RepairFinishDate date,
DateInspected date,
IsArchived bit default 0
)

CREATE TABLE Photos(
Id int primary key identity,
PotHoleID int NOT NULL,
PhotoPath nvarchar(max) NOT NULL,
UserId int NOT NULL,
DateUploaded date NOT NULL,
FOREIGN KEY (PotHoleID) REFERENCES PotHole(PotHoleID),
FOREIGN KEY (UserId) REFERENCES users(Id)
)

create table Claims(
ClaimID int primary key identity,
ClaimantName nvarchar(100) not null,
ClaimantAddress nvarchar(max) not null,
DateOfAccident Date not null,
DateFiled Date NOT NULL,
Phone nvarchar(25) NOT NULL,
Cost int not null,
PotHoleId int NOT NULL,
UserId int NOT NULL, 
FOREIGN KEY (PotHoleId) REFERENCES PotHole(PotHoleID),
FOREIGN KEY (UserId) REFERENCES users(Id)
)

INSERT INTO users
  (username,password,salt,role)
VALUES
  ('user', 'jUE98uhvS5tdIlxRsmz1W7/Qaqo=', '9CWPUTvXqQ4=', 'User'),
  ('jcrookston@gmail.com', 'zE61CMPqHjX2Vx8XKbM/8ZFWMzw=', 'aiFrZh9uNeU=', 'Employee'),
  ('joanemily@gmail.com', 'heQQvv6LRKB2IGAqYuiJWtyhD3k=','e5V32dKvQz0=','User')
   
insert into PotHole(PotHoleLocation, DateReported, DateInspected, RepairBeginDate, RepairFinishDate, Severity, RepairStatus, IsArchived)
values
('3798 West 39th St', '2020-04-06', null, null, null, 1, 'Reported', 0),
('7100 Euclid Avenue', '2020-01-01', '2020-01-10', '2020-01-30', '2020-02-01', 3, 'Repaired', 1),
('3900 Wildlife Way', '2019-08-13', '2019-12-01', null, null, 5, 'Inspected', 0),
('E 105th and Woodland Ave', '2020-03-20', null, null, null, 4, 'Reported', 0),
('6601 Franklin Blvd', '2020-02-20', '2020-03-01', '2020-03-29', '2020-03-31', 2, 'Repaired', 1),
('1265 W 58th St', '2020-03-10', '2019-03-30', null, null, 1, 'Inspected', 0),
('123 Fake St', '2020-04-14', null, null, null, 5, 'Reported', 0),
('742 Evergreen Terrace', '2020-01-01', '2020-01-15', null, null, 4, 'Inspected', 0),
('21 Jump Street', '2019-11-23', '2019-11-30', '2019-12-01', '2019-12-03', 5, 'Repaired', 1),
('221B Baker St', '2020-03-15', null, null, null, 2, 'Inspected', 0)

insert into Claims(ClaimantName, ClaimantAddress, Phone, DateOfAccident, DateFiled, Cost, PotHoleId, UserId)
values
('Rob Rosin', '3798 West 39th', '330-867-5309', '2020-04-10', '2020-04-10', 350, 1, 1),
('Bryan Shaver', '123 Fake St', '330-867-5309', '2020-04-10', '2020-04-10', 500, 1, 2)

insert into Photos(PotHoleID, PhotoPath, UserId, DateUploaded)
values
(4, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586887711/fvcutvmhqrrfnx8xlmbh.jpg', 2, '2020-04-14'),
(4, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586893772/caqeacrl9trnzqx7soxb.jpg', 2, '2020-04-14'),
(3, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586895441/uidjpalw7ou3bsoo1pcm.jpg', 2, '2020-04-14'),
(3, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586897036/wgxu0gcl0nzjicoupkpn.jpg', 2, '2020-04-14'),
(6, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586898868/jkeh1x2bm6fkrkl4baaj.jpg', 2, '2020-04-14'),
(1, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586898964/pikcwzyqhgi38cvff5mw.jpg', 2, '2020-04-14'),
(4, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586899261/zvoe8cbacn1xcpvpikgj.jpg', 2, '2020-04-14'),
(1, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586958621/v9vbohv5r5wxyuelrpc2.jpg', 2, '2020-04-15'),
(6, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586959080/afpexumwyun2362g3b6c.jpg', 2, '2020-04-15'),
(6, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586959519/najzfxsaqgs43mdiurst.jpg', 2, '2020-04-15'),
(1, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586959992/v2nlmmmaeykurdlfw47x.jpg', 2, '2020-04-15'),
(4, 'https://res.cloudinary.com/dlobvad5p/image/upload/v1586961107/c9q2zwjrd8z6znmzvotg.jpg', 2, '2020-04-15')

COMMIT TRANSACTION;

select * from PotHole
SELECT * FROM users
select * from Claims
SELECT * FROM Photos

update users set role='Employee' where username='admin'
