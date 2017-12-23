use ganxian
drop table Products
-- 产品表
create table Products
(
productId int(4)  NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '产品编号',
productName nvarchar(50) not null comment '产品名称',
specs nvarchar(50)  null comment '规格',
originalPrice varchar(50) not null comment '原价',
discountedPrice  varchar(50)  null comment '折后价',
discountedExpiredDate datetime null comment '折后价过期时间--todo',
pic1 nvarchar(50) null comment '图片1',
pic2 nvarchar(50) null comment '图片2',
pic3 nvarchar(50) null comment '图片3',
pic4 nvarchar(50) null comment '图片4',
origin nvarchar(50) null comment '产地',
nw nvarchar(100) null comment '净重',
storageCondition nvarchar(200) null comment '存放条件',
remark nvarchar(2000) null  comment '信息',
createDate datetime null comment '插入时间',
status int null comment '状态', -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)
CREATE INDEX Products_productName ON Products(productName(50));

-- 首页分类表
create table TypeList
(
typeId int(4) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
typeName nvarchar(50) not null comment '类别名称',
isShow int(2) not null comment '是否需要首页推荐', -- 0否 1是
-- showNum int(4) not null comment '显示张数',
isBanner int(2) not null comment '是否是banner', -- banner 为轮播效果，其他平铺',
remark nvarchar(2000) null  comment '信息',
createDate datetime null comment '插入时间',
status int null comment '状态', -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)
drop table Products2Types
-- 首页分类2产品表
create table Products2Types
(
id int(4) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
productId int(4) not null comment '产品编号',
typeId int(4) not null comment '类别编号',
sort int(4) null comment '排序规则', -- 排序规则，优先时间
createDate datetime not null comment '插入时间', 
status int null comment '状态', -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null, -- 备用字段2

FOREIGN KEY (productId) REFERENCES Products (productId),
FOREIGN KEY (typeId) REFERENCES TypeList (typeId)
)
drop table Stocks
-- 库存表\ 是否需要提醒补货
create table Stocks
(
id int(4) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
productId int(4) not null comment '产品编号',
inventory int(5) not null comment '库存量',
remindNum int(3) null comment '警戒量', -- 低于警戒量时，告警，缺省则为0 不提醒
createDate datetime not null comment '插入时间', 
status int null comment '状态', -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null, -- 备用字段2

FOREIGN KEY (productId) REFERENCES Products (productId)
)

-- 用户
create table Users
(
id int(8) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
openid nvarchar(40) null comment '微信openid',
-- userName nvarchar(40) null comment '用户名',
-- userPic nvarchar(100) null comment '用户头像',
createDate datetime not null comment '插入时间', 
status int null comment '状态', -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null, -- 备用字段2

-- 电话
-- 送货地址

)

-- 用户地址
-- 购物车
-- 购物记录
-- 收藏