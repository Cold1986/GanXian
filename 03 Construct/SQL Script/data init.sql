use ganxian

insert  INTO  products(productName,specs,originalPrice,discountedPrice,discountedExpiredDate,pic1,pic2,pic3,pic4,origin,nw,storageCondition,remark,createDate,status)
 select  '小眼带鱼77' productName ,'5两' specs,'197' originalPrice,'¥179.00' discountedPrice,NULL discountedExpiredDate,'/img/product01.jpg' pic1 ,'' pic2,''pic3,''pic4,'舟山' origin,'2000克' nw, '5度以下 ' storageCondition, '东海本地带鱼，每年入冬后小眼带鱼又肥又鲜，清蒸出油，入口即化，鲜美无比。带鱼的麟含有多种营养成分，千万不要刮掉哦。唯一的问题就是小眼带鱼因为肉质细嫩，出水后很容易破肚，这个我们也没法避免，所以介意的朋友请慎拍。往往大家也会把破肚作为衡量是否为正宗小眼带鱼的标准之一。
带鱼吃口很凶，所以它肚子里小鱼小虾很多，再加上长度不利于真空包装，所以我都是事先称重拍照，再进行剪嘴，清理内脏，切段真空包装，介意的朋友请提前说明。
渔山岛海钓带鱼的最佳时节是每年的789月份，包船夜钓，包船费用4500-5000左右，需要的可事先联系客服咨询。
【本店所售带鱼的常规规格为5-7两/条，需要其他规格的请联系客服】' remark,now(),1 ;

truncate table products