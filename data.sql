--create database MovieRating
go
use MovieRating
--用户表
create table Users(
 ID int primary key identity(1,1),
 UserName varchar(20) not null,--用户名
 [Password] varchar(50) not null,
 Roles int not null,
 Email varchar(50) not null,
 Gender bit not null,
 Nickname varchar(15) not null,--昵称
 Birthday varchar(10) null,
 [Address] varchar(20) null,
 Header varchar(100) null,--头像
 Remark varchar(max) null
)
go
--电影表
create table Movie
(
 ID int primary key identity(1,1),
 Name varchar(100) not null,
 Cover varchar(100) not null,--封面
 Director varchar(20) not null,--导演
 Writers varchar(50) null,--编剧
 Actor varchar(MAX) null,--主演
 [Types] varchar(10) not null,--类型1,2,3
 Area varchar(20) null,--国家/地区
 Languages varchar(20) null,--语言
 Show varchar(10) null,--上映日期
 Long varchar(10) null,--片长
 Abstract text null,--简介
 Stills0 varchar(100) null,--剧照
 Stills1 varchar(100) null,--剧照
 Stills2 varchar(100) null,--剧照
 Stills3 varchar(100) null,--剧照
 Stills4 varchar(100) null,--剧照
 Averige decimal(10,1) default(0),--平均分
 Valid bit not null,
 CreateOn datetime null,
 CreateBy varchar(20) null
)
--分类表
create table Category
(
 ID int primary key identity(1,1),
 TypeName varchar(10) not null,
 Valid bit not null
)
go
--评分表
create table Rating
(
 ID int primary key identity(1,1),
 Movie int not null,
 Score int not null,
 Comment text null,
 Praise int not null default(0),--赞
 Dated datetime not null,
 Users int not null
)
go
--图片轮播
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
('爱情',1),
('喜剧',1),
('剧情',1),
('动画',1),
('科幻',1),
('动作',1),
('经典',1),
('悬疑',1),
('青春',1),
('犯罪',1),
('惊悚',1),
('文艺',1),
('搞笑',1),
('纪录片',1),
('励志',1),
('恐怖',1),
('战争',1),
--('短片',1),
--('黑色幽默',1),
('魔幻',1),
('传记',1),
('情色',1),
('感人',1),
('暴力',1),
('家庭',1),
('音乐',1),
--('动画短片',1),
('童年',1),
('浪漫',1),
('女性',1),
--('黑帮',1),
--('同志',1),
--('烂片',1),
('史诗',1),
('童话',1)
--('西部',1)