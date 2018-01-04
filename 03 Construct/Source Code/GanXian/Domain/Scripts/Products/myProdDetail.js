﻿$(function () {
    // 详情数量减少
    $('.details_con .minus,.cart_count .minus').click(function () {
        var _index = $(this).parent().parent().index() - 1;
        var _count = $(this).parent().find('.count');
        var _val = _count.val();
        if (_val > 1) {
            _count.val(_val - 1);
            $('.details_con .selected span').eq(_index).text(_val - 1);

        }
        if (_val <= 2) {
            $(this).addClass('disabled');
        }

    });

    // 详情数量添加
    $('.details_con .add,.cart_count .add').click(function () {
        var _index = $(this).parent().parent().index() - 1;
        var _count = $(this).parent().find('.count');
        var _val = _count.val();
        $(this).parent().find('.minus').removeClass('disabled');
        _count.val(_val - 0 + 1);
        $('.details_con .selected span').eq(_index).text(_val - 0 + 1);

    });

   //插件：图片轮播
    TouchSlide({
        slideCell: "#slide",
        titCell: ".hd ul", //开启自动分页 autoPage:true ，此时设置 titCell 为导航元素包裹层
        mainCell: ".bd ul",
        effect: "left",
        autoPlay: true,//自动播放
        autoPage: true, //自动分页
        switchLoad: "_src" //切换加载，真实图片路径为"_src"
    });

    $('.btn-cart').click(function () {
        var prodId = $("#currProdId").val();//商品Id
        var prodCount = $("#prodCount").val();//购买数量
        if (prodCount == 0) {
            return;
        }

        alert(prodId);
        alert(prodCount);
    });
    
});

function addShopCart() {
    if (!allSelected) {//是否全部选中
        return;
    }
    var prodId = $("#currProdId").val();//商品Id
    var prodCount = $("#prodCount").val();//购买数量
    alert(prodId);
    alert(prodCount);
    //jQuery.ajax({
    //    url: contextPath + "/addShopBuy",
    //    data: {
    //        "prodId": prodId,
    //        "count": prodCount,
    //        "sku_id": $("#currSkuId").val(),
    //        "distUserName": distUserName
    //    },
    //    type: 'post',
    //    async: false, //默认为true 异步   
    //    dataType: 'json',
    //    error: function (data) {
    //    },
    //    success: function (retData) {
    //        if (retData.status == 'LESS') {
    //            floatNotify.simple(prodLessMsg);
    //        } else if (retData.status == 'OWNER') {
    //            floatNotify.simple(failedOwnerMsg);
    //        } else if (retData.status == 'MAX') {
    //            floatNotify.simple(failedBasketMaxMsg);
    //        } else if (retData.status == 'ERR') {
    //            floatNotify.simple(failedBasketErrorMsg);
    //        } else if (retData.status == 'NO_SHOP') {
    //            floatNotify.simple("商家不存在");
    //        } else if (retData.status == 'OFFLINE') {
    //            floatNotify.simple("该商品已经下线,不能购买！");
    //        } else if (retData.status == "OK") {
    //            floatNotify.simple("成功加入购物车！");
    //            var basketCount = $("#totalNum").html();
    //            $("#totalNum").html(Number(basketCount) + Number(prodCount));
    //        }
    //    }
    //});
}