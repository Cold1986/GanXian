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

-- 首页标签2产品表
create table Products2Tabs
(
id int(4) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
productId int(4) not null comment '产品编号',
tabId int(4) not null comment '标签编号',
sort int(4) null comment '排序规则', -- 排序规则，优先时间
isShow int(2) not null comment '是否需要在首页显示', -- 0否 1是
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
nickname nvarchar(100) null comment '用户昵称',
sex varchar(2) null comment '用户的性别，值为1时是男性，值为2时是女性，值为0时是未知',
province varchar(50) null comment '用户个人资料填写的省份',
city varchar(50) null comment '普通用户个人资料填写的城市',
country varchar(50) null comment '国家，如中国为CN',
headimgurl varchar(500) null comment '用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。',
privilege varchar(500) null comment '用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）',
unionid varchar(200) null comment '只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。',
phone varchar(20) null comment '用户手机',
createDate datetime not null comment '插入时间' default now(), 
updateDate datetime  null comment '更新时间' default now(), 
score varchar(200) null comment '用户积分',
status int null comment '状态' default 1, -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)
CREATE INDEX Users_openid ON Users(openid(40));
-- 用户地址

create table UserAddress
(
Id int(8) not null auto_increment primary key comment'编号',
userOpenId nvarchar(50) not null comment '用户微信openid',
receiver nvarchar(30) null comment '收货人',
province varchar(40) null comment '省份',
city varchar(40) null comment '市',
county varchar(40) null comment  '县',
detailAddress nvarchar(400) null comment '详细地址', -- 用户地址
Phone nvarchar(20) null comment '联系电话',-- 用户号码
SetAsDefault varchar(2) default '0' comment '设为默认 0否 1是',
createDate datetime not null comment '插入时间' default now(), 
updateDate datetime  null comment '更新时间' default now(), 
status int null comment '状态' default 1, -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)

-- 购物车
create table ShoppingCart
(
cartId int(8) not  NULL AUTO_INCREMENT PRIMARY KEY comment '编号',  
userOpenId nvarchar(50) not null comment '用户微信openid',
productId int(4) not null comment '产品编号',
num int(4) not null comment '购买数量',
createDate datetime not null comment '插入时间' default now(), 
status int null comment '状态', -- 0无效 1有效 2已转为订单
createdTimePrice DECIMAL(50,2)  null comment '下单时价格', -- 用于提醒用户价格是否有变动
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)

-- 销售单表
create table SalesSlip
(
salesId int(8) not  NULL AUTO_INCREMENT PRIMARY KEY comment '编号',  -- 销售单号
salesNo varchar(40) not null comment '销售编号',-- 界面显示
userOpenId varchar(40) null, -- 用户微信openid,
receiver nvarchar(30) null comment '收货人',
province varchar(40) null comment '省份',
city varchar(40) null comment '市',
county varchar(40) null comment  '县',
detailAddress nvarchar(400) null comment '详细地址', -- 用户地址
Phone nvarchar(20) null comment '联系电话',-- 此单联系电话
amount decimal(10,2) null, -- 总金额
postage decimal(5,2) null comment '邮费',
adminChangeAmount decimal(10,2) null comment '管理员修改价格',
adminChangePostage decimal(5,2) null comment '管理员修改邮费',
wechatOrderNo varchar(40) null comment '微信销售单号',
expressNo varchar(40) null comment '快递单号',
createDate datetime not null comment '创建时间' default now(), 
payDate datetime  null comment '付款时间' ,
deliveryDate datetime null comment '发货时间',
status int null comment '状态', -- 0未付款 1已付款待发货 2 已发货，待收货 3 已完成 4 已删除取消订单 5 预付款 6 已过期  7 已退货
display int null comment '是否显示' default '1' ,-- 0 否 1是
column1 nvarchar(2000) null, -- 备用字段1
column2 nvarchar(2000) null -- 备用字段2
)

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
salesPrice varchar(50) null comment '实际付款价，理论上应该是 折后价*数量',
createDate datetime not null comment '插入时间' default now(), 
createdTimePrice DECIMAL(50,2)  null comment '下单时价格', -- 用于提醒用户价格是否有变动
status int null comment '状态', -- 0未付款 1已付款
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2

)
-- 购物记录
create table OrderLog
(
id  int(8) not  NULL AUTO_INCREMENT PRIMARY KEY comment '编号',  -- 自增
salesNo varchar(40) not null comment '销售编号',-- 界面显示
message nvarchar(1000) null comment '消息内容',
msgType varchar(2) null comment '消息类型 0 正常记录，1 失败，2 异常记录',
createDate datetime not null comment '插入时间' default now(), 
status int null comment '状态', -- 0未付款 1已付款
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)
-- 管理员账号
create table ManagerUsers
(
userId int(8) NOT NULL AUTO_INCREMENT PRIMARY KEY comment '编号',
accountName nvarchar(100) null comment '登陆账号',
psd nvarchar(100) null comment '登陆密码',
createDate datetime not null comment '插入时间' default now(), 
updateDate datetime  null comment '更新时间' default now(), 
status int null comment '状态' default 1, -- 0无效 1有效
column1 nvarchar(100) null, -- 备用字段1
column2 nvarchar(100) null -- 备用字段2
)
