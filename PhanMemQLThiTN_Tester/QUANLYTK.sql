USE MASTER
GO
CREATE DATABASE QUANLYTK
GO
USE QUANLYTK
GO
CREATE TABLE NGUOIDUNG
(
	MAHV NVARCHAR(20),
	MATKHAU NVARCHAR(20)
	CONSTRAINT PK_HV PRIMARY KEY(MAHV)

)
CREATE TABLE GIAOVIEN
(
	TAIKHOAN NVARCHAR(20),
	MATKHAU NVARCHAR(20)
	CONSTRAINT PK_TK PRIMARY KEY(TAIKHOAN)

)

INSERT INTO NGUOIDUNG VALUES('HV01','123'), ('HV02','123')
INSERT INTO GIAOVIEN VALUES('TranHoangPhuc','123'), ('MaTrungHieu','1234')

insert into NGUOIDUNG values
('HV03', '123'),
('HV04', '123'),
('HV05', '123'),
('HV06', '123'),
('HV07', '123'),
('HV08', '123'),
('HV09', '123'),
('HV10', '123'),
('HV11', '123'),
('HV12', '123'),
('HV13', '123'),
('HV14', '123'),
('HV15', '123'),
('HV16', '123');
