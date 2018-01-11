$(function () {
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
        $(this).attr("disabled", "true"); //设置变灰按钮  
        var prodId = $("#currProdId").val();//商品Id
        var prodCount = $("#prodCount").val();//购买数量
        if (prodCount == 0) {
            return;
        }

        $.ajax({
            url: "../AddShopcart",
            data: {
                "prodId": prodId,
                "num": prodCount
            },
            type: 'post',
            async: false, //默认为true 异步   
            dataType: 'json',
            error: function (data) {

            },
            success: function (retData) {
                if (retData == 'false') {
                    alert("添加失败，请稍后尝试");
                }
                else {
                    $('#totalNum').html(retData);
                    alert("已添加成功");
                }
            },
            complete: function () {
                $('#prodCount').val(1);
                $('.btn-cart').removeAttr("disabled"); //设置变灰按钮  
            }

        });
    });

});

//立即购买
function buyNow() {
    $(".btn-buy").attr("disabled", "true"); //设置变灰按钮  
    var prodId = $("#currProdId").val();//商品Id
    var prodCount = $("#prodCount").val();//购买数量

    $.ajax({
        url: "../CreateOrder",
        data: {
            "prodId": prodId,
            "num": prodCount
        },
        type: 'post',
        async: false, //默认为true 异步   
        dataType: 'json',
        error: function (data) {

        },
        success: function (retData) {
            if (retData == 'false') {
                alert("添加失败，请稍后尝试");
            }
            else {
                window.location.href = "../Checkout?orderId=" + retData;
            }
        },
        complete: function () {
            $(".btn-buy").removeAttr("disabled"); //设置变灰按钮  
        }
    });
}
