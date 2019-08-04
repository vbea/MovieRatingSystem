--create database MovieRating
go
use MovieRating
--�û���
create table Users(
 ID int primary key identity(1,1),
 UserName varchar(20) not null,--�û���
 [Password] varchar(50) not null,
 Roles int not null,
 Email varchar(50) not null,
 Gender bit not null,
 Nickname varchar(15) not null,--�ǳ�
 Birthday varchar(10) null,
 [Address] varchar(20) null,
 Header varchar(100) null,--ͷ��
 Remark varchar(max) null
)
go
--��Ӱ��
create table Movie
(
 ID int primary key identity(1,1),
 Name varchar(100) not null,
 Cover varchar(100) not null,--����
 Director varchar(20) not null,--����
 Writers varchar(50) null,--���
 Actor varchar(MAX) null,--����
 [Types] varchar(10) not null,--����1,2,3
 Area varchar(20) null,--����/����
 Languages varchar(20) null,--����
 Show varchar(10) null,--��ӳ����
 Long varchar(10) null,--Ƭ��
 Abstract text null,--���
 Stills0 varchar(100) null,--����
 Stills1 varchar(100) null,--����
 Stills2 varchar(100) null,--����
 Stills3 varchar(100) null,--����
 Stills4 varchar(100) null,--����
 Averige decimal(10,1) default(0),--ƽ����
 Valid bit not null,
 CreateOn datetime null,
 CreateBy varchar(20) null
)
--�����
create table Category
(
 ID int primary key identity(1,1),
 TypeName varchar(10) not null,
 Valid bit not null
)
go
--���ֱ�
create table Rating
(
 ID int primary key identity(1,1),
 Movie int not null,
 Score int not null,
 Comment text null,
 Praise int not null default(0),--��
 Dated datetime not null,
 Users int not null
)
go
--ͼƬ�ֲ�
create table Poster
(
 ID int primary key identity(1,1),
 Title varchar(50) null,
 Picture varchar(50) not null,
 Url varchar(50) not null,
 Valid bit not null,
 CreateOn datetime null,
 CreateBy varchar(20) null
)
alter table Rating Add constraint FK_Rating_User foreign key (Users) references Users(ID)
go
insert into Category (TypeName, Valid)  values 
('����',1),
('ϲ��',1),
('����',1),
('����',1),
('�ƻ�',1),
('����',1),
('����',1),
('����',1),
('�ഺ',1),
('����',1),
('���',1),
('����',1),
('��Ц',1),
('��¼Ƭ',1),
('��־',1),
('�ֲ�',1),
('ս��',1),
--('��Ƭ',1),
--('��ɫ��Ĭ',1),
('ħ��',1),
('����',1),
('��ɫ',1),
('����',1),
('����',1),
('��ͥ',1),
('����',1),
--('������Ƭ',1),
('ͯ��',1),
('����',1),
('Ů��',1),
--('�ڰ�',1),
--('ͬ־',1),
--('��Ƭ',1),
('ʷʫ',1),
('ͯ��',1)
--('����',1)