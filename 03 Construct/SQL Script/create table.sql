use ganxian
drop table Products
-- 产品表
create table Products
(
productId int(4)  NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '产品编号',
productName nvarchar(50) not null comment '产品名称',
specs nvarchar(50)  null comment '规格',
originalPrice DECIMAL(50,2) not null comment '原价',
discountedPrice  DECIMAL(50,2)  null comment '折后价',
discountedExpiredDate datetime null comment '折后价过期时间--todo',
pic1 nvarchar(50) null comment '图片1',
pic2 nvarchar(50) null comment '图片2',
pic3 nvarchar(50) null comment '图片3',
pic4 nvarchar(50) null comment '图片4',
showPic nvarchar(50) null comment '设为封面图的照片',
origin nvarchar(50) null comment '产地',
nw nvarchar(100) null comment '净重',
storageCondition nvarchar(200) null comment '存放条件',
remark nvarchar(2000) null  comment '信息',
createDate datetime null comment '插入时间' default now(),
status int null comment '状态' default 1, -- 0无效 1有效 2 草稿未发布
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)
CREATE INDEX Products_productName ON Products(productName(50));
-- 标签表
drop table TabList
create table TabList
(
tabId int(4) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
typeName nvarchar(50) not null comment '标签名称',
isShow int(2) not null comment '是否需要首页推荐', -- 0否 1是
-- showNum int(4) not null comment '显示张数',
isCarousel int(2)  null comment '是否是轮播' default 0, -- banner 为轮播效果，其他平铺',
remark nvarchar(2000) null  comment '信息',
createDate datetime null comment '插入时间' default now(),
sort int null comment '排序标识',
status int null comment '状态' default 1, -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)
drop table Products2Tabs
-- 首页标签2产品表
create table Products2Tabs
(
id int(4) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
productId int(4) not null comment '产品编号',
tabId int(4) not null comment '标签编号',
sort int(4) null comment '排序规则', -- 排序规则，优先时间
createDate datetime not null comment '插入时间' default now(), 
status int null comment '状态' default 1, -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2

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
column2 nvarchar(100) null -- 备用字段2
)

-- 用户
create table Users
(
userId int(8) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
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
create table ShoppingCart
(
cartId int(8) not  NULL AUTO_INCREMENT PRIMARY KEY comment '编号',  
userOpenId nvarchar(50) not null comment '用户微信openid',
productId int(4) not null comment '产品编号',
num int(4) not null comment '购买数量',
createDate datetime not null comment '插入时间' default now(), 
status int null comment '状态', -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)
-- 销售单表
create table SalesSlip
(
salesId int(8) not  NULL AUTO_INCREMENT PRIMARY KEY comment '编号',  -- 销售单号
userId int(8) null, -- 用户id,
userAddress nvarchar(400), -- 用户地址
userPhone nvarchar(20),-- 用户号码
amount decimal(10,2) null, -- 总金额
postage decimal(5,2) null comment '邮费',
wechatSalesId varchar(40) null,-- 微信交易id
createDate datetime not null comment '插入时间' default now(), 
status int null comment '状态', -- 0未付款 1已付款
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)

drop table Sales2Products
-- 销售单产品明细表
create table Sales2Products
(
id  int(8) not  NULL AUTO_INCREMENT PRIMARY KEY comment '编号',  -- 自增
salesId int(8) not null comment '销售单号',
productId int(4) not null comment '产品编号',
originalPrice varchar(50)  null comment '原价',
discountedPrice  varchar(50)  null comment '折后价',
nw nvarchar(100) null comment '净重',
num int(4) not null comment '购买数量',
salesPrice varchar(50) not null comment '实际付款价，理论上应该是 折后价*数量',
createDate datetime not null comment '插入时间' default now(), 
status int null comment '状态', -- 0未付款 1已付款
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2

)
-- 购物记录
-- 收藏