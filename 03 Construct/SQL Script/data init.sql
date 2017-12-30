truncate table Products2Tabs;
truncate table TabList;
truncate table products;

-- 建立标签
insert into TabList(typeName,isShow,isCarousel,sort) values('海鱼',1,0,2);
insert into TabList(typeName,isShow,isCarousel,sort) values('特产干货',1,0,3);
insert into TabList(typeName,isShow,isCarousel,sort) values('虾蟹',1,0,4);

-- 建立产品
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('辣螺酱','500g','80','65','/img/llj001.jpg','/img/llj002.jpg','/img/llj003.jpg','/img/llj004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('咸呛蟹','300g','120','96','/img/xxx001.jpg','/img/xxx002.jpg','/img/xxx003.jpg','/img/xxx004.jpg','石浦','300g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('红膏蟹股','700g','190','150','/img/xg001.jpg','/img/xg002.jpg','/img/xg003.jpg','/img/xg004.jpg','石浦','700g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('红膏蟹糊','500g','160','128','/img/xh001.jpg','/img/xh002.jpg','/img/xh003.jpg','/img/xh004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('头道紫菜','200g','48','38','/img/zc001.jpg','/img/zc002.jpg','/img/zc003.jpg','/img/zc004.jpg','石浦','200g','0℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('醉泥螺','700g','140','112','/img/znl001.jpg','/img/znl002.jpg','/img/znl003.jpg','/img/znl004.jpg','石浦','700g','-18℃');

insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('鲜活红膏蟹','250g','150','120','/img/hgx001.jpg','/img/hgx002.jpg','/img/hgx003.jpg','/img/hgx004.jpg','石浦','250g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('滑皮虾','500g','70','56','/img/hpx001.jpg','/img/hpx002.jpg','/img/hpx003.jpg','/img/hpx004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('竹节虾','50g','125','100','/img/zjx001.jpg','/img/zjx002.jpg','/img/zjx003.jpg','/img/zjx004.jpg','石浦','500g','-18℃');

insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('白鲳','125g','48','38','/img/bc001.jpg','/img/bc002.jpg','/img/bc003.jpg','/img/bc004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('白鳗','500g','40','32','/img/bm001.jpg','/img/bm002.jpg','/img/bm003.jpg','/img/bm004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('花枝','250g','50','40','/img/hz001.jpg','/img/hz002.jpg','/img/hz003.jpg','/img/hz004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('带鱼','250g','40','32','/img/dy001.jpg','/img/dy002.jpg','/img/dy003.jpg','/img/dy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('海刀鱼','100g','100','80','/img/hdy001.jpg','/img/hdy001.jpg','/img/hdy003.jpg','/img/hdy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('豆腐鱼','50g','20','16','/img/dfy001.jpg','/img/dfy002.jpg','/img/dfy003.jpg','/img/dfy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('方头鱼','250g','50','40','/img/fty001.jpg','/img/fty002.jpg','/img/fty003.jpg','/img/fty004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('海鲈鱼','50g','40','32','/img/hly001.jpg','/img/hly002.jpg','/img/hly003.jpg','/img/hly004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('虎头鱼','500g','80','65','/img/hty001.jpg','/img/hty002.jpg','/img/hty003.jpg','/img/hty004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('黄姑鱼','250g','60','48','/img/hgy001.jpg','/img/hgy002.jpg','/img/hgy003.jpg','/img/hgy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('叽咕','500g','70','56','/img/jg001.jpg','/img/jg002.jpg','/img/jg003.jpg','/img/jg004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('红眼鱼','500g','80','65','/img/hyy001.jpg','/img/hyy002.jpg','/img/hyy003.jpg','/img/hyy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('龙舌鱼','500g','100','80','/img/ls001.jpg','/img/ls002.jpg','/img/ls003.jpg','/img/ls004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('马鲛鱼','500g','30','24','/img/mjy001.jpg','/img/mjy002.jpg','/img/mjy003.jpg','/img/mjy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('梅童鱼','25g','70','56','/img/mty001.jpg','/img/mty002.jpg','/img/mty003.jpg','/img/mty004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('米鱼','500g','40','32','/img/my001.jpg','/img/my002.jpg','/img/my003.jpg','/img/my004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('墨鱼','100g','40','32','/img/myz001.jpg','/img/myz002.jpg','/img/myz003.jpg','/img/myz004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('舌鳎鱼','200g','60','48','/img/sty001.jpg','/img/sty002.jpg','/img/sty003.jpg','/img/sty004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('深水黄鱼','500g','90','72','/img/sshy001.jpg','/img/sshy002.jpg','/img/sshy003.jpg','/img/sshy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('小黄鱼','75g','80','65','/img/xhy001.jpg','/img/xhy002.jpg','/img/xhy003.jpg','/img/xhy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('油鳗','250g','60','48','/img/ym001.jpg','/img/ym002.jpg','/img/ym003.jpg','/img/ym004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('油墨','500g','50','40','/img/ymy001.jpg','/img/ymy002.jpg','/img/ymy003.jpg','/img/ymy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('鱿鱼','250g','50','40','/img/yy001.jpg','/img/yy002.jpg','/img/yy003.jpg','/img/yy004.jpg','石浦','500g','-18℃');
insert  INTO products(productName,specs,originalPrice,discountedPrice,PIC1,PIC2,PIC3,PIC4,origin,nw,storageCondition) values('真鲷','500g','70','56','/img/zd001.jpg','/img/zd002.jpg','/img/zd003.jpg','/img/zd004.jpg','石浦','500g','-18℃');

set SQL_SAFE_UPDATES =0
update products set showPic='pic1'
set SQL_SAFE_UPDATES =1


-- 标签与产品分类
insert into Products2Tabs(productId,tabId,sort,status) values(12,2,1,1);
insert into Products2Tabs(productId,tabId,sort,status) values(13,2,2,1);
insert into Products2Tabs(productId,tabId,sort,status) values(14,2,3,1);
insert into Products2Tabs(productId,tabId,sort,status) values(15,2,4,1);
insert into Products2Tabs(productId,tabId,sort,status) values(16,2,5,1);
insert into Products2Tabs(productId,tabId,sort,status) values(17,2,6,1);

insert into Products2Tabs(productId,tabId,sort,status) values(1,3,1,1);
insert into Products2Tabs(productId,tabId,sort,status) values(2,3,1,1);
insert into Products2Tabs(productId,tabId,sort,status) values(3,3,1,1);
insert into Products2Tabs(productId,tabId,sort,status) values(4,3,1,1);

insert into Products2Tabs(productId,tabId,sort,status) values(7,4,1,1);
insert into Products2Tabs(productId,tabId,sort,status) values(8,4,1,1);
-- test part
select IFNULL(b.num,0) soldNum,a.* from products a 
left join (select sum(num) as num ,productid from sales2products group by productid) as b on a.productid=b.productid
where a.status=1

